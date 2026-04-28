using Microsoft.EntityFrameworkCore;
using ProjectManager.Application.Ports;
using ProjectManager.Domain.Entities;
using ProjectManager.Infrastructure.Persistence.Context;

namespace ProjectManager.Infrastructure.Persistence.Repositories
{
    public sealed class PermissionRepository(
        ProjectManagerDbContext context
    ) : IPermissionRepository
    {
        private readonly ProjectManagerDbContext _context = context;
        
        public async Task<IEnumerable<Permission>> GetAllAsync(CancellationToken ct = default)
            => await _context.Permissions.ToListAsync(ct);

        public async Task<Permission?> GetByIdAsync(Guid id, CancellationToken ct = default)
            => await _context.Permissions.FirstOrDefaultAsync(p => p.Id == id, ct);

        public async Task AddAsync(Permission permission, CancellationToken ct = default)
        {
            await _context.Permissions.AddAsync(permission, ct);
            await _context.SaveChangesAsync(ct);
        }

        public async Task UpdateAsync(Permission permission, CancellationToken ct = default)
        {
            _context.Permissions.Update(permission);
            await _context.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(Guid id, CancellationToken ct = default)
        {
            var permission = await _context.Permissions.FindAsync([id], ct);
            if (permission != null)
            {
                _context.Permissions.Remove(permission);
                await _context.SaveChangesAsync(ct);
            }
        }
    }
}
