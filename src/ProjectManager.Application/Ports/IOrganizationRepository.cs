using ProjectManager.Domain.Entities;

namespace ProjectManager.Application.Ports
{
    public interface IOrganizationRepository
    {
        Task<Organization?> GetByIdAsync(Guid organizationId, CancellationToken ct = default);
        Task<IEnumerable<Organization>> GetAllAsync(CancellationToken ct = default);
        Task AddAsync(Organization organization, CancellationToken ct = default);
    }
}
