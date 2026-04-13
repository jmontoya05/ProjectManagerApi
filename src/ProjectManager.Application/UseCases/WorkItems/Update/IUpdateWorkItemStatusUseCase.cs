using ProjectManager.Application.DTOs.WorkItems.Request;

namespace ProjectManager.Application.UseCases.WorkItems.Update
{
    public interface IUpdateWorkItemStatusUseCase
    {
        Task Execute(Guid workItemId, UpdateWorkItemStatusRequest request, Guid currentUserId, CancellationToken ct = default);
    }
}
