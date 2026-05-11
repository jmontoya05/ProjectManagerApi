using ProjectManager.Application.DTOs.Sprints;

namespace ProjectManager.Application.UseCases.Sprints.AddWorkItem
{
    public interface IAddWorkItemToSprintUseCase
    {
        Task Execute(Guid projectId, Guid sprintId, AddWorkItemToSprintRequest request, CancellationToken ct = default);
    }
}
