using ProjectManager.Application.DTOs.Projects;
using ProjectManager.Application.Exceptions;
using ProjectManager.Application.Ports;
using ProjectManager.Application.Services;

namespace ProjectManager.Application.UseCases.Projects.Update
{
    public sealed class UpdateProjectUseCase(
        IProjectRepository projectRepository,
        ITenantContext tenantContext
    ) : IUpdateProjectUseCase
    {
        private readonly IProjectRepository _projectRepository = projectRepository;
        private readonly ITenantContext _tenantContext = tenantContext;

        public async Task Execute(UpdateProjectRequest request, Guid projectId, CancellationToken ct = default)
        {
            var project = await _projectRepository.GetByIdAsync(projectId, ct)
                ?? throw new NotFoundException("Project not found", "Project", projectId);

            if (!string.IsNullOrWhiteSpace(request.Name))
                project.Name = request.Name;
            if (!string.IsNullOrWhiteSpace(request.Description))
                project.Description = request.Description;
            if (!string.IsNullOrWhiteSpace(request.Status))
                project.Status = request.Status;

            project.UpdatedAt = DateTime.UtcNow;
            project.UpdatedBy = GetCurrentUserId();

            await _projectRepository.UpdateAsync(project, ct);
        }

        private Guid GetCurrentUserId() =>
            Guid.TryParse(_tenantContext.UserId, out var id) ? id
            : throw new UnauthorizedException("Invalid user context.");
    }
}
