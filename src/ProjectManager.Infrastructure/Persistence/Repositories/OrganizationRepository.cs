using Microsoft.EntityFrameworkCore;
using ProjectManager.Application.Ports;
using ProjectManager.Domain.Entities;
using ProjectManager.Infrastructure.Persistence.Context;
using ProjectManager.Application.Services;

namespace ProjectManager.Infrastructure.Persistence.Repositories
{
    public sealed class OrganizationRepository(
        ProjectManagerDbContext context, 
        ITenantContext tenantContext
    ) : IOrganizationRepository
    {
        private readonly ProjectManagerDbContext _context = context;
        private readonly ITenantContext _tenantContext = tenantContext;

        public async Task AddAsync(Organization organization, CancellationToken ct = default)
        {
            await _context.Organizations.AddAsync(organization, ct);
            await SaveChangesAsync(ct);
        }

        public async Task<Organization?> GetByIdAsync(Guid organizationId, CancellationToken ct = default) =>
            await _context.Organizations.FirstOrDefaultAsync(o => o.Id == organizationId, ct);

        public async Task<IEnumerable<Organization>> GetAllAsync(CancellationToken ct = default)
        {
            var userId = Guid.Parse(_tenantContext.UserId!);
            return await _context.Organizations
                .Where(o => o.OrganizationMemberships.Any(om => om.UserId == userId))
                .Include(o => o.OrganizationMemberships.Where(om => om.UserId == userId))
                    .ThenInclude(om => om.Role)
                .ToListAsync(ct);
        }

        private async Task SaveChangesAsync(CancellationToken ct = default)
        {
            await _context.SaveChangesAsync(ct);
        }
    }
}
