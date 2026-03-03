using Microsoft.EntityFrameworkCore;
using ProjectManager.Application.Ports;
using ProjectManager.Domain.Entities;
using ProjectManager.Infrastructure.Persistence.Context;

namespace ProjectManager.Infrastructure.Persistence.Repositories
{
    public sealed class WorkItemRepository(ProjectManagerDbContext context) : IWorkItemRepository
    {
        private readonly ProjectManagerDbContext _context = context;

        public async Task AddAsync(WorkItem workItem, CancellationToken ct = default)
        {
            await _context.WorkItems.AddAsync(workItem, ct);
            await SaveChangesAsync(ct);
        }

        public async Task<WorkItem?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
            await _context.WorkItems
                .Include(wi => wi.Assignee)
                .Include(wi => wi.Team)
                .FirstOrDefaultAsync(wi => wi.Id == id, ct);

        public async Task<IEnumerable<WorkItem>> GetByProjectAsync(Guid projectId, CancellationToken ct = default) =>
            await _context.WorkItems
                .Where(wi => wi.ProjectId == projectId)
                .Include(wi => wi.Assignee)
                .Include(wi => wi.Team)
                .OrderByDescending(wi => wi.UpdatedAt)
                .ToListAsync(ct);

        public async Task UpdateAsync(WorkItem workItem, CancellationToken ct = default)
        {
            _context.WorkItems.Update(workItem);
            await SaveChangesAsync(ct);
        }

        private async Task<int> SaveChangesAsync(CancellationToken ct = default) =>
            await _context.SaveChangesAsync(ct);
    }
}
