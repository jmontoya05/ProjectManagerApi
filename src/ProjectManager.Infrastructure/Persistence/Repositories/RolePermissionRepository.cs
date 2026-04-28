using Microsoft.EntityFrameworkCore;
using ProjectManager.Application.Ports;
using ProjectManager.Domain.Entities;
using ProjectManager.Infrastructure.Persistence.Context;

namespace ProjectManager.Infrastructure.Persistence.Repositories
{
    public sealed class RolePermissionRepository(
        ProjectManagerDbContext context
    ) : IRolePermissionRepository
    {
        private readonly ProjectManagerDbContext _context = context;
        
        public async Task<IEnumerable<Permission>> GetPermissionsByRoleAsync(Guid roleId, CancellationToken ct = default)
        {
            return await _context.RolePermissions
                .Where(rp => rp.RoleId == roleId)
                .Include(rp => rp.Permission)
                .Select(rp => rp.Permission)
                .ToListAsync(ct);
        }

        public async Task AddPermissionToRoleAsync(Guid roleId, Guid permissionId, CancellationToken ct = default)
        {
            if (!await _context.RolePermissions.AnyAsync(rp => rp.RoleId == roleId && rp.PermissionId == permissionId, ct))
            {
                _context.RolePermissions.Add(new RolePermission { RoleId = roleId, PermissionId = permissionId });
                await _context.SaveChangesAsync(ct);
            }
        }

        public async Task RemovePermissionFromRoleAsync(Guid roleId, Guid permissionId, CancellationToken ct = default)
        {
            var rp = await _context.RolePermissions.FirstOrDefaultAsync(rp => rp.RoleId == roleId && rp.PermissionId == permissionId, ct);
            if (rp != null)
            {
                _context.RolePermissions.Remove(rp);
                await _context.SaveChangesAsync(ct);
            }
        }
    }
}
