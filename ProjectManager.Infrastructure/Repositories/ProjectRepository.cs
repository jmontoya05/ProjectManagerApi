using Microsoft.EntityFrameworkCore;
using ProjectManager.Application.Ports;
using ProjectManager.Domain.Entities;
using ProjectManager.Infrastructure.Data;

namespace ProjectManager.Infrastructure.Repositories
{
    public sealed class ProjectRepository(ProjectManagerDbContext context) : IProjectRepository
    {
        private readonly ProjectManagerDbContext _context = context;

        public async Task Addasync(Project project, CancellationToken ct = default)
        {
            await _context.Projects.AddAsync(project, ct);
            await SaveChangesAsync(ct);
        }

        public async Task<IEnumerable<Project>> GetAllByOrganizationIdAsync(Guid organizationId, CancellationToken ct = default) =>
            await _context.Projects
                .Where(p => p.OrganizationId == organizationId)
                .ToListAsync(ct);

        public async Task<Project?> GetByIdAsync(Guid projectId, CancellationToken ct = default) =>
            await _context.Projects
                .FirstOrDefaultAsync(p => p.Id == projectId, ct);

        public async Task UpdateAsync(Project project, CancellationToken ct = default)
        {
            _context.Projects.Update(project);
            await SaveChangesAsync(ct);
        }

        private Task<int> SaveChangesAsync(CancellationToken ct = default) =>
            _context.SaveChangesAsync(ct);
    }
}
