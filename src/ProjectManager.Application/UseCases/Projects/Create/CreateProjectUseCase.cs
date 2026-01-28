using ProjectManager.Application.Ports;
using ProjectManager.Domain.Entities;

namespace ProjectManager.Application.UseCases.Projects.Create
{
    public sealed class CreateProjectUseCase(IProjectRepository projectRepository, IUserRepository userRepository) : ICreateProjectUseCase
    {
        private readonly IProjectRepository _projectRepository = projectRepository;
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<Guid> Execute(CreateProjectRequest request, Guid organizationId, Guid currentUserId, CancellationToken ct = default)
        {
            var owner = await _userRepository.GetByIdAsync(request.OwnerId, ct)
                ?? throw new InvalidOperationException("Owner not found");

            if (!await _userRepository.UserBelongsToOrganizationAsync(owner.Id, organizationId, ct))
                throw new InvalidOperationException("Owner is not a member of this organization.");

            var project = new Project
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                OrganizationId = organizationId,
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
