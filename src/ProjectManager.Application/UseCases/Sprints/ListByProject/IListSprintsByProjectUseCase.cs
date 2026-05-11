using ProjectManager.Application.DTOs.Sprints;

namespace ProjectManager.Application.UseCases.Sprints.ListByProject
{
    public interface IListSprintsByProjectUseCase
    {
        Task<IEnumerable<SprintResponse>> Execute(Guid projectId, CancellationToken ct = default);
    }
}
