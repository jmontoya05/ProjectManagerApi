using ProjectManager.Application.Ports;
using ProjectManager.Domain.Entities;

namespace ProjectManager.Application.UseCases.Projects.Create
{
    public sealed class CreateProjectUseCase(IProjectRepository projectRepository) : ICreateProjectUseCase
    {
        private readonly IProjectRepository _projectRepository = projectRepository;

        public async Task<Guid> Execute(CreateProjectRequest request, Guid organizationId, Guid userId, CancellationToken ct = default)
        {
            var project = new Project
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                OrganizationId = organizationId,
                OwnerId = userId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _projectRepository.Addasync(project, ct);
            return project.Id;
        }
    }
}
