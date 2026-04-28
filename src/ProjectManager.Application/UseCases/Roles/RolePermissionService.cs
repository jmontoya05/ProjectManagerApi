using ProjectManager.Application.DTOs.Permissions;
using ProjectManager.Application.Ports;

namespace ProjectManager.Application.UseCases.Roles
{
    public sealed class RolePermissionService(
        IRolePermissionRepository rolePermissionRepository
    ) : IRolePermissionService
    {
        private readonly IRolePermissionRepository _rolePermissionRepository = rolePermissionRepository;

        public async Task<IEnumerable<PermissionDto>> GetPermissionsByRoleAsync(Guid roleId, CancellationToken ct = default)
        {
            var permissions = await _rolePermissionRepository.GetPermissionsByRoleAsync(roleId, ct);
            return permissions.Select(p => new PermissionDto { Id = p.Id, Name = p.Name, Description = p.Description });
        }

        public async Task AddPermissionToRoleAsync(Guid roleId, Guid permissionId, CancellationToken ct = default)
        {
            await _rolePermissionRepository.AddPermissionToRoleAsync(roleId, permissionId, ct);
        }

        public async Task RemovePermissionFromRoleAsync(Guid roleId, Guid permissionId, CancellationToken ct = default)
        {
            await _rolePermissionRepository.RemovePermissionFromRoleAsync(roleId, permissionId, ct);
        }
    }
}
