using ProjectManager.Application.DTOs.Sprints;

namespace ProjectManager.Application.UseCases.Sprints.ListWorkItems
{
    public interface IListSprintWorkItemsUseCase
    {
        Task<IEnumerable<ListSprintWorkItemsResponse>> Execute(Guid projectId, Guid sprintId, CancellationToken ct = default);
    }
}

