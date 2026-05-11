using ProjectManager.Application.DTOs.Sprints;

namespace ProjectManager.Application.UseCases.Sprints.Create
{
    public interface ICreateSprintUseCase
    {
        Task<Guid> Execute(Guid projectId, CreateSprintRequest request, CancellationToken ct = default);
    }
}
