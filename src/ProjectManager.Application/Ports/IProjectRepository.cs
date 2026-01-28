using ProjectManager.Domain.Entities;

namespace ProjectManager.Application.Ports
{
    public interface IProjectRepository
    {
        Task Addasync(Project project, CancellationToken ct = default);
        Task<Project?> GetByIdAsync(Guid projectId, CancellationToken ct = default);
        Task<IEnumerable<Project>> GetAllByOrganizationIdAsync(Guid organizationId, CancellationToken ct = default);
        Task UpdateAsync(Project project, CancellationToken ct = default);
    }
}
