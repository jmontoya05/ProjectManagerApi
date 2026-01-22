using ProjectManager.Domain.Entities;

namespace ProjectManager.Application.Ports
{
    public interface ITeamRepository
    {
        Task<Team?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<IEnumerable<Team>> GetByOrganizationAsync(Guid organizationId, CancellationToken ct = default);
        Task AddAsync(Team team, CancellationToken ct = default);
    }
}
