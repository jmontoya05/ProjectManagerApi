using Microsoft.EntityFrameworkCore;
using ProjectManager.Application.Ports;
using ProjectManager.Domain.Entities;
using ProjectManager.Infrastructure.Data;

namespace ProjectManager.Infrastructure.Repositories
{
    public sealed class UserRepository(ProjectManagerDbContext context) : IUserRepository
    {
        private readonly ProjectManagerDbContext _context = context;

        public async Task AddAsync(User user, CancellationToken ct = default) =>
            await _context.Users.AddAsync(user, ct);

        public async Task<bool> ExistsByEmailAsync(string email, CancellationToken ct = default) =>
            await _context.Users.AnyAsync(u => u.Email == email, ct);

        public Task<int> SaveChangesAsync(CancellationToken ct = default) =>
            _context.SaveChangesAsync(ct);
    }
}
