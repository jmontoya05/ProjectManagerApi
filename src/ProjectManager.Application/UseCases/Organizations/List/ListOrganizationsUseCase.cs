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
            var organizations = await _organizationRepository.GetAllAsync(ct);

            return organizations.Select(o => new ListOrganizationsResponse
            {
                Id = o.Id,
                Name = o.Name,
                Status = o.Status,
                Roles = o.OrganizationMemberships.Select(om => om.Role.Name)
            });
        }
    }
}
