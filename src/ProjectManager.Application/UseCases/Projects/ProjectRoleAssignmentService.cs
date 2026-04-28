using ProjectManager.Application.DTOs.Projects;
using ProjectManager.Application.Ports;
using ProjectManager.Domain.Entities;

namespace ProjectManager.Application.UseCases.Projects
{
    public sealed class ProjectRoleAssignmentService(
        IProjectRepository projectRepository
    ) : IProjectRoleAssignmentService
    {
        private readonly IProjectRepository _projectRepository = projectRepository;

        public async Task AssignRoleAsync(AssignProjectRoleRequest request, CancellationToken ct = default)
        {
            await _projectRepository.RemoveMembershipAsync(request.UserId, request.ProjectId, ct);
            var membership = new ProjectMembership
            {
                Id = Guid.NewGuid(),
                ProjectId = request.ProjectId,
                UserId = request.UserId,
                RoleId = request.RoleId,
                CreatedAt = DateTime.UtcNow
            };
            await _projectRepository.AddMembershipAsync(membership, ct);
        }

        public async Task RemoveRoleAsync(RemoveProjectRoleRequest request, CancellationToken ct = default)
        {
            await _projectRepository.RemoveMembershipAsync(request.UserId, request.ProjectId, ct);
        }
    }
}
