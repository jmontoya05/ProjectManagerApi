using Microsoft.EntityFrameworkCore;
using ProjectManager.Application.DTOs.WorkItems;
using ProjectManager.Application.Ports;
using ProjectManager.Application.Services;
using ProjectManager.Domain.Entities;
using ProjectManager.Infrastructure.Persistence.Context;

namespace ProjectManager.Infrastructure.Persistence.Repositories
{
    public sealed class WorkItemRepository(
        ProjectManagerDbContext context,
        ITenantContext tenantContext
    ) : IWorkItemRepository
    {
        private readonly ProjectManagerDbContext _context = context;
        private readonly ITenantContext _tenantContext = tenantContext;
        
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

        public async Task<(IEnumerable<WorkItem> items, bool hasNextPage)> GetPagedByProjectAsync(Guid projectId, WorkItemFilter filter, CancellationToken ct = default)
        {
            var query = _context.WorkItems
                .Include(wi => wi.Assignee)
                .Include(wi => wi.Team)
                .Include(wi => wi.ParentWorkItem)
                .Where(wi => wi.ProjectId == projectId && wi.Project.OrganizationId == _tenantContext.GetOrganizationIdOrThrow());

            if (!string.IsNullOrWhiteSpace(filter.Status))
                query = query.Where(wi => wi.Status == filter.Status);

            if (filter.AssigneeId.HasValue)
                query = query.Where(wi => wi.AssigneeId == filter.AssigneeId);

            if (filter.TeamId.HasValue)
                query = query.Where(wi => wi.TeamId == filter.TeamId);

            if (!string.IsNullOrWhiteSpace(filter.Cursor))
            {
                var cursorDateTime = DateTime.Parse(filter.Cursor, System.Globalization.CultureInfo.InvariantCulture);
                query = query.Where(wi => wi.UpdatedAt < cursorDateTime);
            }

            query = query.OrderByDescending(wi => wi.UpdatedAt);

            var items = await query
                .Take(filter.PageSize + 1)
                .ToListAsync(ct);

            var hasNextPage = items.Count > filter.PageSize;
            if (hasNextPage)
                items.RemoveAt(items.Count - 1);

            return (items, hasNextPage);
        }

        public async Task UpdateAsync(WorkItem workItem, CancellationToken ct = default)
        {
            _context.WorkItems.Update(workItem);
            await SaveChangesAsync(ct);
        }

        private async Task SaveChangesAsync(CancellationToken ct = default) =>
            await _context.SaveChangesAsync(ct);
    }
}
