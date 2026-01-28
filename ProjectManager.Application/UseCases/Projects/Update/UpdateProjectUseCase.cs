using ProjectManager.Application.Ports;

namespace ProjectManager.Application.UseCases.Projects.Update
{
    public sealed class UpdateProjectUseCase(IProjectRepository projectRepository) : IUpdateProjectUseCase
    {
        private readonly IProjectRepository _projectRepository = projectRepository;

        public async Task Execute(UpdateProjectRequest request, Guid projectId, Guid organizationId, Guid currentUserId, CancellationToken ct = default)
        {
            var project = await _projectRepository.GetByIdAsync(projectId, ct)
                ?? throw new InvalidOperationException("Project not found");

            if (project.OrganizationId != organizationId)
                throw new InvalidOperationException("The project don't belong to the organization");

            if (!string.IsNullOrWhiteSpace(request.Name))
                project.Name = request.Name;
            if (!string.IsNullOrWhiteSpace(request.Description))
                project.Description = request.Description;
            if (!string.IsNullOrWhiteSpace(request.Status))
                project.Status = request.Status;

            project.UpdatedAt = DateTime.UtcNow;
            project.UpdatedBy = currentUserId;

            await _projectRepository.UpdateAsync(project, ct);
        }
    }
}
