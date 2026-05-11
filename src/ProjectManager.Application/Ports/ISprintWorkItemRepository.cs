using ProjectManager.Domain.ValueObjects;

namespace ProjectManager.Application.Ports
{
    public interface ISprintWorkItemRepository
    {
        Task AddAsync(SprintWorkItem sprintWorkItem, CancellationToken ct = default);
        Task<SprintWorkItem?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<IEnumerable<SprintWorkItem>> GetBySprintIdAsync(Guid sprintId, CancellationToken ct = default);
        Task UpdateAsync(SprintWorkItem sprintWorkItem, CancellationToken ct = default);
        Task DeleteAsync(Guid id, CancellationToken ct = default);
    }
}

