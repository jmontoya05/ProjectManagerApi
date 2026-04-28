using ProjectManager.Application.DTOs.WorkItems;
using ProjectManager.Application.Ports;
using ProjectManager.Application.Exceptions;
using ProjectManager.Application.Services;

namespace ProjectManager.Application.UseCases.WorkItems.Update
{
    public sealed class UpdateWorkItemStatusUseCase(IWorkItemRepository workItemRepository, IUserRepository userRepository, ITenantContext tenantContext) : IUpdateWorkItemStatusUseCase
    {
        private readonly IWorkItemRepository _workItemRepository = workItemRepository;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly ITenantContext _tenantContext = tenantContext;
        private static readonly string[] Backlog = ["InProgress"];
        private static readonly string[] InProgress = ["Backlog", "Done"];
        private static readonly string[] Done = ["InProgress"];

        public async Task Execute(Guid workItemId, UpdateWorkItemStatusRequest request, Guid currentUserId, CancellationToken ct = default)
        {
            var workItem = await _workItemRepository.GetByIdAsync(workItemId, ct)
                ?? throw new NotFoundException("Work item not found", "WorkItem", workItemId);
            
            var validTransitions = new Dictionary<string, string[]>
            {
                { "Backlog", Backlog },
                { "InProgress", InProgress },
                { "Done", Done }
            };

            if (!validTransitions.TryGetValue(workItem.Status, out var value) || !value.Contains(request.Status))
                throw new BusinessRuleException($"Invalid status transition from {workItem.Status} to {request.Status}.", "INVALID_STATUS_TRANSITION");

            var project = await _userRepository.GetProjectByWorkItemIdAsync(workItemId, ct)
                ?? throw new NotFoundException("Project not found for this work item.", "WorkItem", workItemId);

            if (project.OrganizationId.ToString() != _tenantContext.OrganizationId)
                throw new ForbiddenException("The project doesn't belong to your current organization context.");

            workItem.Status = request.Status;
            workItem.UpdatedAt = DateTime.UtcNow;

            await _workItemRepository.UpdateAsync(workItem, ct);
        }
    }
}
