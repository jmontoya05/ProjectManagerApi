using ProjectManager.Application.DTOs.Organizations;
using ProjectManager.Application.Ports;
using ProjectManager.Application.Services;

namespace ProjectManager.Application.UseCases.Organizations.List
{
    public sealed class ListOrganizationsUseCase(IOrganizationRepository organizationRepository, ITenantContext tenantContext) : IListOrganizationsUseCase
    {
        private readonly IOrganizationRepository _organizationRepository = organizationRepository;
        private readonly ITenantContext _tenantContext = tenantContext;

        public async Task<IEnumerable<ListOrganizationsResponse>> Execute(CancellationToken ct = default)
        {
            var userId = _tenantContext.UserId != null ? Guid.Parse(_tenantContext.UserId) : Guid.Empty;
            var organizations = await _organizationRepository.GetAllAsync(ct);

            return organizations
                .Select(o => new ListOrganizationsResponse
                {
                    Id = o.Id,
                    Name = o.Name,
                    Status = o.Status,
                    Roles = o.OrganizationMemberships
                                .Where(om => om.UserId == userId)
                                .Select(om => om.Role.Name)
                });
        }
    }
}
