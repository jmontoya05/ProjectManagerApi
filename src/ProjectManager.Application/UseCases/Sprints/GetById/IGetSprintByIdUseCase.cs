using ProjectManager.Application.DTOs.Sprints;

namespace ProjectManager.Application.UseCases.Sprints.GetById
{
    public interface IGetSprintByIdUseCase
    {
        Task<SprintResponse?> Execute(Guid projectId, Guid sprintId, CancellationToken ct = default);
    }
}
