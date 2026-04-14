using ProjectManager.Application.DTOs.WorkItems;
using ProjectManager.Application.Ports;

namespace ProjectManager.Application.UseCases.WorkItems.Update
{
    public sealed class UpdateWorkItemStatusUseCase(IWorkItemRepository workItemRepository, IUserRepository userRepository) : IUpdateWorkItemStatusUseCase
    {
        private readonly IWorkItemRepository _workItemRepository = workItemRepository;
        private readonly IUserRepository _userRepository = userRepository;
        internal static readonly string[] backlog = ["InProgress"];
        internal static readonly string[] InProgress = ["Backlog", "Done"];
        internal static readonly string[] Done = ["InProgress"];

        public async Task Execute(Guid workItemId, UpdateWorkItemStatusRequest request, Guid currentUserId, CancellationToken ct = default)
        {
            var workItem = await _workItemRepository.GetByIdAsync(workItemId, ct)
                ?? throw new InvalidOperationException("Work item not found.");


            var validTransitions = new Dictionary<string, string[]>
            {
                { "Backlog", backlog },
                { "InProgress", InProgress },
                { "Done", Done }
            };

            if (!validTransitions.TryGetValue(workItem.Status, out string[]? value) || !value.Contains(request.Status))
                throw new InvalidOperationException($"Invalid status transition from {workItem.Status} to {request.Status}.");

            _ = await _userRepository.GetProjectByWorkItemIdAsync(workItemId, ct)
                ?? throw new InvalidOperationException("Project not found.");


            workItem.Status = request.Status;
            workItem.UpdatedAt = DateTime.UtcNow;

            await _workItemRepository.UpdateAsync(workItem, ct);
        }
    }
}
