using ProjectManager.Application.DTOs.Projects;

namespace ProjectManager.Application.UseCases.Projects.Update
{
    public interface IUpdateProjectUseCase
    {
        Task Execute(UpdateProjectRequest request, Guid projectId, CancellationToken ct = default);
    }
}
