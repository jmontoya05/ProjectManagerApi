using ProjectManager.Application.DTOs.Projects;
using ProjectManager.Application.Exceptions;
using ProjectManager.Application.Ports;
using ProjectManager.Application.Services;
using ProjectManager.Domain.Enums;
using ProjectManager.Domain.Exceptions;

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
            var currentUserId = _tenantContext.GetUserIdOrThrow();

            if (!string.IsNullOrWhiteSpace(request.Name))
                project.Name = request.Name;
            
            if (!string.IsNullOrWhiteSpace(request.Description))
                project.Description = request.Description;
            
            if (!string.IsNullOrWhiteSpace(request.Status))
            {
                try
                {
                    var newStatus = Enum.Parse<ProjectStatus>(request.Status);
                    project.TransitionStatus(newStatus);
                }
                catch (InvalidProjectStatusTransitionException ex)
                {
                    throw new BusinessRuleException(ex.Message, ex.ErrorCode);
                }
            }

            project.UpdatedBy = currentUserId;

            await _projectRepository.UpdateAsync(project, ct);
        }
    }
}
