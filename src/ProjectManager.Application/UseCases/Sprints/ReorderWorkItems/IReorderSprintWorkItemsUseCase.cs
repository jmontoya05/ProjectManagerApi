using ProjectManager.Application.DTOs.Sprints;

namespace ProjectManager.Application.UseCases.Sprints.ReorderWorkItems
{
    public interface IReorderSprintWorkItemsUseCase
    {
        Task Execute(Guid projectId, Guid sprintId, ReorderSprintWorkItemsRequest request, CancellationToken ct = default);
    }
}

