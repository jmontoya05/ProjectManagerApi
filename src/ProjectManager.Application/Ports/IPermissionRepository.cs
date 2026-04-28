using ProjectManager.Domain.Entities;

namespace ProjectManager.Application.Ports
{
    public interface IPermissionRepository
    {
        Task<IEnumerable<Permission>> GetAllAsync(CancellationToken ct = default);
        Task<Permission?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task AddAsync(Permission permission, CancellationToken ct = default);
        Task UpdateAsync(Permission permission, CancellationToken ct = default);
        Task DeleteAsync(Guid id, CancellationToken ct = default);
    }
}
