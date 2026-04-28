using Microsoft.EntityFrameworkCore;
using ProjectManager.Application.Ports;
using ProjectManager.Domain.Entities;
using ProjectManager.Infrastructure.Persistence.Context;

namespace ProjectManager.Infrastructure.Persistence.Repositories
{
    public sealed class InvitationRepository(
        ProjectManagerDbContext context
    ) : IInvitationRepository
    {
        private readonly ProjectManagerDbContext _context = context;
        
        public async Task AddAsync(Invitation invitation, CancellationToken ct = default)
        {
            await _context.Invitations.AddAsync(invitation, ct);
            await _context.SaveChangesAsync(ct);
        }

        public async Task<Invitation?> GetByTokenAsync(string token, CancellationToken ct = default)
        {
            return await _context.Invitations.FirstOrDefaultAsync(i => i.Token == token, ct);
        }

        public async Task UpdateAsync(Invitation invitation, CancellationToken ct = default)
        {
            _context.Invitations.Update(invitation);
            await _context.SaveChangesAsync(ct);
        }
    }
}
