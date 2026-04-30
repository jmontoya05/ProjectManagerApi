using ProjectManager.Application.DTOs.Organizations;
using ProjectManager.Application.Ports;

namespace ProjectManager.Application.UseCases.Organizations.List
{
    public sealed class ListOrganizationsUseCase(
        IOrganizationRepository organizationRepository
    ) : IListOrganizationsUseCase
    {
        private readonly IOrganizationRepository _organizationRepository = organizationRepository;

        public async Task<IEnumerable<ListOrganizationsResponse>> Execute(CancellationToken ct = default)
        {
            var memberships = await _organizationRepository.GetAllAsync(ct);

            return memberships
                .GroupBy(om => om.Organization)
                .Select(g => new ListOrganizationsResponse
                {
                    Id = g.Key.Id,
                    Name = g.Key.Name,
                    Status = g.Key.Status,
                    Roles = g.Select(om => om.Role.Name)
                });
        }
    }
}
