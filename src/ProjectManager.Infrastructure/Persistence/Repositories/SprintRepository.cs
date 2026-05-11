using Microsoft.EntityFrameworkCore;
using ProjectManager.Application.Ports;
using ProjectManager.Domain.Entities;
using ProjectManager.Infrastructure.Persistence.Context;

namespace ProjectManager.Infrastructure.Persistence.Repositories
{
    public sealed class SprintRepository(
        ProjectManagerDbContext context
    ) : ISprintRepository
    {
        private readonly ProjectManagerDbContext _context = context;

        public async Task AddAsync(Sprint sprint, CancellationToken ct = default)
        {
            await _context.Sprints.AddAsync(sprint, ct);
            await SaveChangesAsync(ct);
        }

        public async Task<Sprint?> GetByIdAsync(Guid sprintId, CancellationToken ct = default) =>
            await _context.Sprints
                .Include(s => s.WorkItems)
                .ThenInclude(swi => swi.WorkItem)
                .FirstOrDefaultAsync(s => s.Id == sprintId, ct);

        public async Task<IEnumerable<Sprint>> GetByProjectIdAsync(Guid projectId, CancellationToken ct = default) =>
            await _context.Sprints
                .Where(s => s.ProjectId == projectId)
                .Include(s => s.WorkItems)
                .ThenInclude(swi => swi.WorkItem)
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync(ct);

        public async Task<IEnumerable<Sprint>> GetAllAsync(CancellationToken ct = default) =>
            await _context.Sprints
                .Include(s => s.WorkItems)
                .ThenInclude(swi => swi.WorkItem)
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync(ct);

        public async Task UpdateAsync(Sprint sprint, CancellationToken ct = default)
        {
            _context.Sprints.Update(sprint);
            await SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(Guid sprintId, CancellationToken ct = default)
        {
            var sprint = await _context.Sprints.FindAsync([sprintId], cancellationToken: ct);
            if (sprint != null)
            {
                _context.Sprints.Remove(sprint);
                await SaveChangesAsync(ct);
            }
        }

        private Task<int> SaveChangesAsync(CancellationToken ct = default) =>
            _context.SaveChangesAsync(ct);
    }
}

