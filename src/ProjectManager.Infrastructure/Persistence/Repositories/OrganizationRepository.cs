using Microsoft.EntityFrameworkCore;
using ProjectManager.Application.Ports;
using ProjectManager.Domain.Entities;
using ProjectManager.Infrastructure.Persistence.Context;
using ProjectManager.Application.Services;

namespace ProjectManager.Infrastructure.Persistence.Repositories
{
    public sealed class OrganizationRepository : IOrganizationRepository
    {
        private readonly ProjectManagerDbContext _context;
        private readonly ITenantContext _tenantContext;

        public OrganizationRepository(ProjectManagerDbContext context, ITenantContext tenantContext)
        {
            _context = context;
            _tenantContext = tenantContext;
        }

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
            var memberships = await _context.OrganizationMemberships
                .Where(om => om.UserId == userId)
                .Include(om => om.Organization)
                .Include(om => om.Role)
                .ThenInclude(o => o.OrganizationMemberships)
                .ToListAsync(ct);

            return memberships.Select(om => om.Organization).Distinct();
        }

        private async Task<int> SaveChangesAsync(CancellationToken ct = default) =>
            await _context.SaveChangesAsync(ct);
    }
}
