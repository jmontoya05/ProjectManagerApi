using ProjectManager.Application.DTOs.Projects;

namespace ProjectManager.Application.UseCases.Projects
{
    public interface IProjectRoleAssignmentService
    {
        Task AssignRoleAsync(AssignProjectRoleRequest request, CancellationToken ct = default);
        Task RemoveRoleAsync(RemoveProjectRoleRequest request, CancellationToken ct = default);
    }
}
