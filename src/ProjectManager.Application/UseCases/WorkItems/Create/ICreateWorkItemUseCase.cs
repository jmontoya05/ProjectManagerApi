using ProjectManager.Application.DTOs.WorkItems.Request;

namespace ProjectManager.Application.UseCases.WorkItems.Create
{
    public interface ICreateWorkItemUseCase
    {
        Task<Guid> Execute(Guid projectId, CreateWorkItemRequest request, Guid currentUserId, CancellationToken ct = default);
    }
}
