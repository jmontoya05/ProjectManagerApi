using ProjectManager.Application.Ports;

namespace ProjectManager.Application.UseCases.Projects.List
{
    public sealed class ListProjectsUseCase(IProjectRepository projectRepository) : IListProjectsUseCase
    {
        private readonly IProjectRepository _projectRepository = projectRepository;

        public async Task<IEnumerable<ListProjectsResponse>> Execute(Guid organizationId, CancellationToken ct = default)
        {
            var projects = await _projectRepository.GetAllByOrganizationIdAsync(organizationId, ct);

            return projects
                .Select(p => new ListProjectsResponse
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Status = p.Status,
                    OrganizationId = p.OrganizationId,
                    OwnerId = p.OwnerId,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt
                });
        }
    }
}
