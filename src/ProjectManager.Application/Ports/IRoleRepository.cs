using ProjectManager.Domain.Entities;

namespace ProjectManager.Application.Ports
{
    public interface IRoleRepository
    {
        Task<IEnumerable<Role>> GetAllAsync(CancellationToken ct = default);
        Task<Role?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task AddAsync(Role role, CancellationToken ct = default);
        Task UpdateAsync(Role role, CancellationToken ct = default);
        Task DeleteAsync(Guid id, CancellationToken ct = default);
        Task<IEnumerable<Role>> GetAllByOrganizationAsync(Guid organizationId, CancellationToken ct = default);
    }
}
