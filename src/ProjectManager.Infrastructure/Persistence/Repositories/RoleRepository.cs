using Microsoft.EntityFrameworkCore;
using ProjectManager.Application.Ports;
using ProjectManager.Domain.Entities;
using ProjectManager.Infrastructure.Persistence.Context;

namespace ProjectManager.Infrastructure.Persistence.Repositories
{
    public sealed class RoleRepository(
        ProjectManagerDbContext context
    ) : IRoleRepository
    {
        private readonly ProjectManagerDbContext _context = context;
        
        public async Task<IEnumerable<Role>> GetAllAsync(CancellationToken ct = default)
            => await _context.Roles.ToListAsync(ct);

        public async Task<IEnumerable<Role>> GetAllByOrganizationAsync(Guid organizationId, CancellationToken ct = default)
            => await _context.Roles
                .Where(r => r.OrganizationId == organizationId || r.OrganizationId == null)
                .ToListAsync(ct);

        public async Task<Role?> GetByIdAsync(Guid id, CancellationToken ct = default)
            => await _context.Roles.FirstOrDefaultAsync(r => r.Id == id, ct);

        public async Task AddAsync(Role role, CancellationToken ct = default)
        {
            await _context.Roles.AddAsync(role, ct);
            await _context.SaveChangesAsync(ct);
        }

        public async Task UpdateAsync(Role role, CancellationToken ct = default)
        {
            _context.Roles.Update(role);
            await _context.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(Guid id, CancellationToken ct = default)
        {
            var role = await _context.Roles.FindAsync([id], ct);
            if (role != null)
            {
                _context.Roles.Remove(role);
                await _context.SaveChangesAsync(ct);
            }
        }
    }
}
