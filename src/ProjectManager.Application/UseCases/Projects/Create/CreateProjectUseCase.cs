using ProjectManager.Application.DTOs.Projects;
using ProjectManager.Application.Ports;
using ProjectManager.Application.Exceptions;
using ProjectManager.Application.Services;
using ProjectManager.Domain.Entities;

namespace ProjectManager.Application.UseCases.Projects.Create
{
    public sealed class CreateProjectUseCase(IProjectRepository projectRepository, IUserRepository userRepository, ITenantContext tenantContext) : ICreateProjectUseCase
    {
        private readonly IProjectRepository _projectRepository = projectRepository;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly ITenantContext _tenantContext = tenantContext;

        public async Task<Guid> Execute(CreateProjectRequest request, Guid currentUserId, CancellationToken ct = default)
        {
            var owner = await _userRepository.GetByIdAsync(request.OwnerId, ct)
                ?? throw new NotFoundException("Owner not found", "User", request.OwnerId);

            var orgId = Guid.Parse(_tenantContext.OrganizationId!);

            var project = new Project
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                OrganizationId = orgId,
                OwnerId = request.OwnerId,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = currentUserId,
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = currentUserId
            };

            await _projectRepository.Addasync(project, ct);
            return project.Id;
        }
    }
}
