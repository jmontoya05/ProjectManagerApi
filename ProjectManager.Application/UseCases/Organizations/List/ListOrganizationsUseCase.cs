using ProjectManager.Application.Ports;

namespace ProjectManager.Application.UseCases.Organizations.List
{
    public sealed class ListOrganizationsUseCase(IOrganizationRepository organizationRepository) : IListOrganizationsUseCase
    {
        private readonly IOrganizationRepository _organizationRepository = organizationRepository;

        public async Task<IEnumerable<ListOrganizationsResponse>> Execute(Guid userId, CancellationToken ct = default)
        {
            var organizations = await _organizationRepository.GetByUserAsync(userId, ct);

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
