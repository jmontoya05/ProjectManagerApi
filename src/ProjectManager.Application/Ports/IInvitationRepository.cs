using ProjectManager.Domain.Entities;

namespace ProjectManager.Application.Ports
{
    public interface IInvitationRepository
    {
        Task AddAsync(Invitation invitation, CancellationToken ct = default);
        Task<Invitation?> GetByTokenAsync(string token, CancellationToken ct = default);
        Task UpdateAsync(Invitation invitation, CancellationToken ct = default);
    }
}
