using ProjectManager.Application.DTOs.WorkItems;
using ProjectManager.Application.Ports;
using ProjectManager.Application.Exceptions;
using ProjectManager.Application.Services;
using ProjectManager.Domain.Enums;
using ProjectManager.Domain.Exceptions;

namespace ProjectManager.Application.UseCases.WorkItems.Update
{
    public sealed class UpdateWorkItemStatusUseCase(IWorkItemRepository workItemRepository, IUserRepository userRepository, ITenantContext tenantContext) : IUpdateWorkItemStatusUseCase
    {
        private readonly IWorkItemRepository _workItemRepository = workItemRepository;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly ITenantContext _tenantContext = tenantContext;

        public async Task Execute(Guid workItemId, UpdateWorkItemStatusRequest request, CancellationToken ct = default)
        {
            var orgId = _tenantContext.GetOrganizationIdOrThrow();
            var workItem = await _workItemRepository.GetByIdAsync(workItemId, ct)
                ?? throw new NotFoundException("Work item not found", "WorkItem", workItemId);

            var project = await _userRepository.GetProjectByWorkItemIdAsync(workItemId, ct)
                ?? throw new NotFoundException("Project not found for this work item.", "WorkItem", workItemId);

            if (project.OrganizationId != orgId)
                throw new ForbiddenException("The project doesn't belong to your current organization context.");

            try
            {
                var newStatus = Enum.Parse<WorkItemStatus>(request.Status);
                workItem.TransitionStatus(newStatus);
            }
            catch (InvalidWorkItemStatusTransitionException ex)
            {
                throw new BusinessRuleException(ex.Message, ex.ErrorCode);
            }
            catch (WorkItemCannotBeCompletedWithActiveSubtasksException ex)
            {
                throw new BusinessRuleException(ex.Message, ex.ErrorCode);
            }

            await _workItemRepository.UpdateAsync(workItem, ct);
        }
    }
}
