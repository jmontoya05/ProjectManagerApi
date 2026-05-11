using ProjectManager.Domain.Entities;

namespace ProjectManager.Application.Ports
{
    public interface ISprintRepository
    {
        Task AddAsync(Sprint sprint, CancellationToken ct = default);
        Task<Sprint?> GetByIdAsync(Guid sprintId, CancellationToken ct = default);
        Task<IEnumerable<Sprint>> GetByProjectIdAsync(Guid projectId, CancellationToken ct = default);
        Task<IEnumerable<Sprint>> GetAllAsync(CancellationToken ct = default);
        Task UpdateAsync(Sprint sprint, CancellationToken ct = default);
        Task DeleteAsync(Guid sprintId, CancellationToken ct = default);
    }
}

