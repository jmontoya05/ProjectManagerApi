using ProjectManager.Application.DTOs.Sprints;

namespace ProjectManager.Application.UseCases.Sprints.Update
{
    public interface IUpdateSprintUseCase
    {
        Task Execute(Guid projectId, Guid sprintId, UpdateSprintRequest request, CancellationToken ct = default);
    }
}
