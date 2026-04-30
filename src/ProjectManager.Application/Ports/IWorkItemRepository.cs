using ProjectManager.Application.DTOs.WorkItems;
using ProjectManager.Domain.Entities;

namespace ProjectManager.Application.Ports
{
    public interface IWorkItemRepository
    {
        Task<WorkItem?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<(IEnumerable<WorkItem> items, bool hasNextPage)> GetPagedByProjectAsync(Guid projectId, WorkItemFilter filter, CancellationToken ct = default);
        Task AddAsync(WorkItem workItem, CancellationToken ct = default);
        Task UpdateAsync(WorkItem workItem, CancellationToken ct = default);
    }
}
