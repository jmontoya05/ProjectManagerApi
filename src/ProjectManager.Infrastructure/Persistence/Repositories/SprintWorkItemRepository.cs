using Microsoft.EntityFrameworkCore;
using ProjectManager.Application.Ports;
using ProjectManager.Domain.ValueObjects;
using ProjectManager.Infrastructure.Persistence.Context;

namespace ProjectManager.Infrastructure.Persistence.Repositories
{
    public sealed class SprintWorkItemRepository(
        ProjectManagerDbContext context
    ) : ISprintWorkItemRepository
    {
        private readonly ProjectManagerDbContext _context = context;

        public async Task AddAsync(SprintWorkItem sprintWorkItem, CancellationToken ct = default)
        {
            await _context.SprintWorkItems.AddAsync(sprintWorkItem, ct);
            await SaveChangesAsync(ct);
        }

        public async Task<SprintWorkItem?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
            await _context.SprintWorkItems
                .Include(swi => swi.Sprint)
                .Include(swi => swi.WorkItem)
                .FirstOrDefaultAsync(swi => swi.Id == id, ct);

        public async Task<IEnumerable<SprintWorkItem>> GetBySprintIdAsync(Guid sprintId, CancellationToken ct = default) =>
            await _context.SprintWorkItems
                .Where(swi => swi.SprintId == sprintId)
                .Include(swi => swi.WorkItem)
                .OrderBy(swi => swi.OrderIndex)
                .ToListAsync(ct);

        public async Task UpdateAsync(SprintWorkItem sprintWorkItem, CancellationToken ct = default)
        {
            _context.SprintWorkItems.Update(sprintWorkItem);
            await SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(Guid id, CancellationToken ct = default)
        {
            var sprintWorkItem = await _context.SprintWorkItems.FindAsync([id], cancellationToken: ct);
            if (sprintWorkItem != null)
            {
                _context.SprintWorkItems.Remove(sprintWorkItem);
                await SaveChangesAsync(ct);
            }
        }

        private Task<int> SaveChangesAsync(CancellationToken ct = default) =>
            _context.SaveChangesAsync(ct);
    }
}

