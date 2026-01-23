using Microsoft.EntityFrameworkCore;
using ProjectManager.Application.Ports;
using ProjectManager.Domain.Entities;
using ProjectManager.Infrastructure.Data;

namespace ProjectManager.Infrastructure.Repositories
{
    public sealed class TeamMemberRepository(ProjectManagerDbContext context) : ITeamMemberRepository
    {
        private readonly ProjectManagerDbContext _context = context;

        public async Task AddMemberAsync(TeamMember member, CancellationToken ct = default)
        {
            await _context.TeamMembers.AddAsync(member, ct);
            await SaveChangesAsync(ct);
        }

        public async Task<bool> IsUserInTeamAsync(Guid userId, Guid teamId, CancellationToken ct = default)
        {
            return await _context.TeamMembers
                .AnyAsync(tm => tm.TeamId == teamId && tm.UserId == userId, ct);
        }

        public async Task RemoveMemberAsync(Guid userId, Guid teamId, CancellationToken ct = default)
        {
            var member = await _context.TeamMembers
                .FirstOrDefaultAsync(tm => tm.UserId == userId && tm.TeamId == teamId, ct);

            if (member is not null)
            {
                _context.TeamMembers.Remove(member);
                await SaveChangesAsync(ct);
            }

        }

        private Task<int> SaveChangesAsync(CancellationToken ct = default) =>
            _context.SaveChangesAsync(ct);
    }
}
