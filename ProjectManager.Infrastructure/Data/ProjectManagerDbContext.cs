using Microsoft.EntityFrameworkCore;

namespace ProjectManager.Infrastructure.Data
{
    public class ProjectManagerDbContext(DbContextOptions<ProjectManagerDbContext> options) : DbContext(options)
    {
    }
}
