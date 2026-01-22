using Microsoft.EntityFrameworkCore;
using ProjectManager.Application.Ports;
using ProjectManager.Domain.Entities;
using ProjectManager.Infrastructure.Data;

namespace ProjectManager.Infrastructure.Repositories
{
    public sealed class TeamRepository(ProjectManagerDbContext context) : ITeamRepository
    {
        private readonly ProjectManagerDbContext _context = context;

        public async Task AddAsync(Team team, CancellationToken ct = default)
        {
            await _context.Teams.AddAsync(team, ct);
            await SaveChangesAsync(ct);
        }

        public async Task<Team?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
            await _context.Teams
                .Include(t => t.Members)
                .FirstOrDefaultAsync(t => t.Id == id, ct);

        public async Task<IEnumerable<Team>> GetByOrganizationAsync(Guid organizationId, CancellationToken ct = default) =>
            await _context.Teams
                .Where(t => t.OrganizationId == organizationId)
                .ToListAsync(ct);

        private Task<int> SaveChangesAsync(CancellationToken ct = default) =>
            _context.SaveChangesAsync(ct);
    }
}
