using Microsoft.EntityFrameworkCore;
using ProjectManager.Application.Ports;
using ProjectManager.Domain.Entities;
using ProjectManager.Infrastructure.Persistence.Context;
using ProjectManager.Application.Services;

namespace ProjectManager.Infrastructure.Persistence.Repositories
{
    public sealed class TeamRepository(
        ProjectManagerDbContext context, 
        ITenantContext tenantContext
    ) : ITeamRepository
    {
        private readonly ProjectManagerDbContext _context = context;
        private readonly ITenantContext _tenantContext = tenantContext;
        
        public async Task AddAsync(Team team, CancellationToken ct = default)
        {
            var orgId = _tenantContext.GetOrganizationIdOrThrow();
            team.OrganizationId = orgId;
            await _context.Teams.AddAsync(team, ct);
            await SaveChangesAsync(ct);
        }

        public async Task<Team?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
            await _context.Teams
                .Include(t => t.Members)
                .FirstOrDefaultAsync(t => t.Id == id && t.OrganizationId == _tenantContext.GetOrganizationIdOrThrow(), ct);
        
        public async Task<IEnumerable<Team>> GetAllAsync(CancellationToken ct = default) =>
            await _context.Teams
                .Where(t => t.OrganizationId == _tenantContext.GetOrganizationIdOrThrow())
                .ToListAsync(ct);
        
        private Task<int> SaveChangesAsync(CancellationToken ct = default) =>
            _context.SaveChangesAsync(ct);
    }
}
