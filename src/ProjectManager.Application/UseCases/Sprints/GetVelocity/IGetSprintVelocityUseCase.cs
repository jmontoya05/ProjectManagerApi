using ProjectManager.Application.DTOs.Sprints;

namespace ProjectManager.Application.UseCases.Sprints.GetVelocity
{
    public interface IGetSprintVelocityUseCase
    {
        Task<SprintVelocityResponse> Execute(Guid projectId, Guid sprintId, CancellationToken ct = default);
    }
}
