﻿using Microsoft.EntityFrameworkCore;
using ProjectManager.Application.Ports;
using ProjectManager.Domain.Entities;
using ProjectManager.Infrastructure.Persistence.Context;
using ProjectManager.Application.Services;

namespace ProjectManager.Infrastructure.Persistence.Repositories
{
    public sealed class TeamRepository(
        ProjectManagerDbContext context, 
        ITenantContext tenantContext
    ) : ITeamRepository
    {
        private readonly ProjectManagerDbContext _context = context;
        private readonly ITenantContext _tenantContext = tenantContext;
        
        public async Task AddAsync(Team team, CancellationToken ct = default)
        {
            if (!Guid.TryParse(_tenantContext.OrganizationId, out var orgId))
                throw new InvalidOperationException("Organization context is missing or invalid.");

            team.OrganizationId = orgId;
            await _context.Teams.AddAsync(team, ct);
            await SaveChangesAsync(ct);
        }

        public async Task<Team?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            if (!Guid.TryParse(_tenantContext.OrganizationId, out var orgId))
                throw new InvalidOperationException("Organization context is missing or invalid.");
            
            return await _context.Teams
                .Include(t => t.Members)
                .FirstOrDefaultAsync(t => t.Id == id && t.OrganizationId == orgId, ct);
        }

        public async Task<IEnumerable<Team>> GetAllAsync(CancellationToken ct = default)
        {
            if (!Guid.TryParse(_tenantContext.OrganizationId, out var orgId))
                throw new InvalidOperationException("Organization context is missing or invalid.");
            
            return await _context.Teams
                .Where(t => t.OrganizationId == orgId)
                .ToListAsync(ct);
        }

        private Task<int> SaveChangesAsync(CancellationToken ct = default) =>
            _context.SaveChangesAsync(ct);
    }
}
