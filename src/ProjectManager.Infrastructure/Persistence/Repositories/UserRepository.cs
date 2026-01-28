using Microsoft.EntityFrameworkCore;
using ProjectManager.Application.Ports;
using ProjectManager.Domain.Entities;
using ProjectManager.Infrastructure.Persistence.Context;

namespace ProjectManager.Infrastructure.Persistence.Repositories
{
    public sealed class UserRepository(ProjectManagerDbContext context) : IUserRepository
    {
        private readonly ProjectManagerDbContext _context = context;

        public async Task AddAsync(User user, CancellationToken ct = default)
        {
            await _context.Users.AddAsync(user, ct);
            await SaveChangesAsync(ct);
        }

        public Task<User?> GetByIdAsync(Guid userId, CancellationToken ct = default) =>
            _context.Users.FirstOrDefaultAsync(u => u.Id == userId, ct);

        public async Task<User?> GetByEmailAsync(string email, CancellationToken ct = default) =>
            await _context.Users.FirstOrDefaultAsync(u => u.Email == email, ct);

        public async Task<bool> ExistsByEmailAsync(string email, CancellationToken ct = default) =>
            await _context.Users.AnyAsync(u => u.Email == email, ct);

        public async Task SaveRefreshTokenAsync(Guid userId, string token, DateTime expiresAt, CancellationToken ct = default)
        {
            var rt = new RefreshToken
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Token = token,
                ExpiresAt = expiresAt,
                CreatedAt = DateTime.UtcNow
            };

            await _context.RefreshTokens.AddAsync(rt, ct);
            await SaveChangesAsync(ct);
        }

        public async Task<RefreshToken?> GetValidRefreshTokenAsync(string token, CancellationToken ct = default) =>
            await _context.RefreshTokens
                .Include(rt => rt.User)
                .FirstOrDefaultAsync(rt => rt.Token == token && rt.RevokedAt == null && rt.ExpiresAt > DateTime.UtcNow, ct);

        public async Task RevokeRefreshTokenAsync(RefreshToken token, CancellationToken ct = default)
        {
            token.RevokedAt = DateTime.UtcNow;
            await SaveChangesAsync(ct);
        }

        public async Task<IEnumerable<string>> GetUserRolesByOrganizationAsync(Guid userId, Guid organizationId, CancellationToken ct = default)
        {
            var query = _context.OrganizationMemberships
                .Where(om => om.UserId == userId && om.OrganizationId == organizationId);

            var roles = await query
                .Select(om => om.Role.Name)
                .ToListAsync(ct);

            return roles.Distinct();
        }

        public async Task<IEnumerable<Organization>> GetUserOrganizationsAsync(Guid userId, CancellationToken ct = default)
        {
            var query = _context.OrganizationMemberships
                .Where(om => om.UserId == userId);

            var organizations = await query
                .Select(om => om.Organization)
                .ToListAsync(ct);

            return organizations.Distinct();
        }

        public async Task<bool> UserBelongsToOrganizationAsync(Guid userId, Guid organizationId, CancellationToken ct = default)
        {
            return await _context.OrganizationMemberships
                .AnyAsync(om => om.UserId == userId && om.OrganizationId == organizationId, ct);
        }

        public async Task AddMembershipAsync(OrganizationMembership membership, string roleName, CancellationToken ct = default)
        {
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == roleName, ct)
                ?? throw new InvalidOperationException($"Role '{roleName}' not found.");

            membership.RoleId = role.Id;
            await _context.OrganizationMemberships.AddAsync(membership, ct);
            await SaveChangesAsync(ct);
        }

        private Task<int> SaveChangesAsync(CancellationToken ct = default) =>
            _context.SaveChangesAsync(ct);
    }
}
