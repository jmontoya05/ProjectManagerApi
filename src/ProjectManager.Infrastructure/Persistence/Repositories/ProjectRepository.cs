using Microsoft.EntityFrameworkCore;
using ProjectManager.Application.Ports;
using ProjectManager.Application.Services;
using ProjectManager.Domain.Entities;
using ProjectManager.Infrastructure.Persistence.Context;

namespace ProjectManager.Infrastructure.Persistence.Repositories
{
    public sealed class ProjectRepository(
        ProjectManagerDbContext context, 
        ITenantContext tenantContext
    ) : IProjectRepository
    {
        private readonly ProjectManagerDbContext _context = context;
        private readonly ITenantContext _tenantContext = tenantContext;
        
        public async Task Addasync(Project project, CancellationToken ct = default)
        {
            project.OrganizationId = Guid.Parse(_tenantContext.OrganizationId!);
            await _context.Projects.AddAsync(project, ct);
            await SaveChangesAsync(ct);
        }

        public async Task<IEnumerable<Project>> GetAllAsync(CancellationToken ct = default) =>
            await _context.Projects
                .Where(p => p.OrganizationId.ToString() == _tenantContext.OrganizationId)
                .ToListAsync(ct);

        public async Task<Project?> GetByIdAsync(Guid projectId, CancellationToken ct = default) =>
            await _context.Projects
                .FirstOrDefaultAsync(p => p.Id == projectId && p.OrganizationId.ToString() == _tenantContext.OrganizationId, ct);

        public async Task UpdateAsync(Project project, CancellationToken ct = default)
        {
            _context.Projects.Update(project);
            await SaveChangesAsync(ct);
        }

        public async Task AddMembershipAsync(ProjectMembership membership, CancellationToken ct = default)
        {
            await _context.ProjectMemberships.AddAsync(membership, ct);
            await SaveChangesAsync(ct);
        }

        public async Task RemoveMembershipAsync(Guid userId, Guid projectId, CancellationToken ct = default)
        {
            var membership = await _context.ProjectMemberships
                .FirstOrDefaultAsync(pm => pm.UserId == userId && pm.ProjectId == projectId, ct);
            if (membership != null)
            {
                _context.ProjectMemberships.Remove(membership);
                await SaveChangesAsync(ct);
            }
        }

        private Task<int> SaveChangesAsync(CancellationToken ct = default) =>
            _context.SaveChangesAsync(ct);
    }
}
