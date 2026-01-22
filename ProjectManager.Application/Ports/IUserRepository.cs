using ProjectManager.Domain.Entities;

namespace ProjectManager.Application.Ports
{
    public interface IUserRepository
    {
        Task AddAsync(User user, CancellationToken ct = default);
        Task<User?> GetByIdAsync(Guid userId, CancellationToken ct = default);
        Task<User?> GetByEmailAsync(string email, CancellationToken ct = default);
        Task<bool> ExistsByEmailAsync(string email, CancellationToken ct = default);
        Task SaveRefreshTokenAsync(Guid userId, string token, DateTime expiresAt, CancellationToken ct = default);
        Task<RefreshToken?> GetValidRefreshTokenAsync(string token, CancellationToken ct = default);
        Task RevokeRefreshTokenAsync(RefreshToken token, CancellationToken ct = default);
        Task<IEnumerable<string>> GetUserRolesByOrganizationAsync(Guid userId, Guid organizationId, CancellationToken ct = default);
        Task<IEnumerable<Organization>> GetUserOrganizationsAsync(Guid userId, CancellationToken ct = default);
        Task<bool> UserBelongsToOrganizationAsync(Guid userId, Guid organizationId, CancellationToken ct = default);
    }
}
