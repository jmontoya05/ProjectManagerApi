using Microsoft.EntityFrameworkCore;
using ProjectManager.Infrastructure.Data.Entities;

namespace ProjectManager.Infrastructure.Data
{
    public class ProjectManagerDbContext(DbContextOptions<ProjectManagerDbContext> options) : DbContext(options)
    {
        private static readonly DateTime SeedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        public DbSet<User> Users => Set<User>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<Permission> Permissions => Set<Permission>();
        public DbSet<Organization> Organizations => Set<Organization>();
        public DbSet<OrganizationMembership> OrganizationMemberships => Set<OrganizationMembership>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrganizationMembership>(om =>
            {
                om.HasKey(o => o.Id);

                om.HasOne<Organization>()
                  .WithMany()
                  .HasForeignKey(o => o.OrganizationId)
                  .OnDelete(DeleteBehavior.Cascade);

                om.HasOne<User>()
                  .WithMany()
                  .HasForeignKey(o => o.UserId)
                  .OnDelete(DeleteBehavior.NoAction);

                om.HasOne<Role>()
                  .WithMany()
                  .HasForeignKey(o => o.RoleId)
                  .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<Role>().HasData(
                new Role
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    Name = "OrgOwner",
                    Description = "Organization Owner",
                    CreatedAt = SeedDate,
                    UpdatedAt = SeedDate
                },
                new Role
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    Name = "OrgAdmin",
                    Description = "Organization Admin",
                    CreatedAt = SeedDate,
                    UpdatedAt = SeedDate
                },
                new Role
                {
                    Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    Name = "ProjectMember",
                    Description = "Project Member",
                    CreatedAt = SeedDate,
                    UpdatedAt = SeedDate
                }
            );

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                    Name = "Admin",
                    Email = "admin@demo.com",
                    DisplayName = "Demo Admin",
                    PasswordHash = "$2y$10$stjSoctlKDZo1KlIa1znEuypAG/zsmFZ/YpPLCopo61te0SVMxCeu",
                    Status = "Active",
                    CreatedAt = SeedDate,
                    UpdatedAt = SeedDate
                }
            );

            modelBuilder.Entity<Organization>().HasData(
                new Organization
                {
                    Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                    Name = "Demo Organization",
                    Status = "Active",
                    OwnerId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                    CreatedAt = SeedDate,
                    UpdatedAt = SeedDate
                }
            );
        }
    }
}
