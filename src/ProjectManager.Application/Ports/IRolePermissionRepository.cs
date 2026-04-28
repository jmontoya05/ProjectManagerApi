using ProjectManager.Domain.Entities;

namespace ProjectManager.Application.Ports
{
    public interface IRolePermissionRepository
    {
        Task<IEnumerable<Permission>> GetPermissionsByRoleAsync(Guid roleId, CancellationToken ct = default);
        Task AddPermissionToRoleAsync(Guid roleId, Guid permissionId, CancellationToken ct = default);
        Task RemovePermissionFromRoleAsync(Guid roleId, Guid permissionId, CancellationToken ct = default);
    }
}
