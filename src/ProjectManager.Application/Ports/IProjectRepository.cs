using ProjectManager.Domain.Entities;

namespace ProjectManager.Application.Ports
{
    public interface IProjectRepository
    {
        Task Addasync(Project project, CancellationToken ct = default);
        Task<Project?> GetByIdAsync(Guid projectId, CancellationToken ct = default);
        Task<IEnumerable<Project>> GetAllAsync(CancellationToken ct = default);
        Task UpdateAsync(Project project, CancellationToken ct = default);
        Task AddMembershipAsync(ProjectMembership membership, CancellationToken ct = default);
        Task RemoveMembershipAsync(Guid userId, Guid projectId, CancellationToken ct = default);
    }
}
