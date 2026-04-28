using ProjectManager.Application.DTOs.Permissions;

namespace ProjectManager.Application.UseCases.Roles
{
    public interface IRolePermissionService
    {
        Task<IEnumerable<PermissionDto>> GetPermissionsByRoleAsync(Guid roleId, CancellationToken ct = default);
        Task AddPermissionToRoleAsync(Guid roleId, Guid permissionId, CancellationToken ct = default);
        Task RemovePermissionFromRoleAsync(Guid roleId, Guid permissionId, CancellationToken ct = default);
    }
}
