using ProjectManager.Domain.Entities;

namespace ProjectManager.Application.Ports
{
    public interface IWorkItemRepository
    {
        Task<WorkItem?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<IEnumerable<WorkItem>> GetByProjectAsync(Guid projectId, CancellationToken ct = default);
        Task AddAsync(WorkItem workItem, CancellationToken ct = default);
        Task UpdateAsync(WorkItem workItem, CancellationToken ct = default);
    }
}
