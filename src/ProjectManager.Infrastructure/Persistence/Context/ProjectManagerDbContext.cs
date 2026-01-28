using Microsoft.EntityFrameworkCore;
using ProjectManager.Domain.Entities;

namespace ProjectManager.Infrastructure.Persistence.Context
{
    public class ProjectManagerDbContext(DbContextOptions<ProjectManagerDbContext> options) : DbContext(options)
    {
        private static readonly DateTime SeedDate = new(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        public DbSet<User> Users => Set<User>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<Permission> Permissions => Set<Permission>();
        public DbSet<Organization> Organizations => Set<Organization>();
        public DbSet<OrganizationMembership> OrganizationMemberships => Set<OrganizationMembership>();
        public DbSet<Team> Teams => Set<Team>();
        public DbSet<TeamMember> TeamMembers => Set<TeamMember>();
        public DbSet<Project> Projects => Set<Project>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrganizationMembership>(entity =>
            {
                entity.HasKey(om => om.Id);

                entity.HasOne(om => om.Organization)
                      .WithMany(o => o.OrganizationMemberships)
                      .HasForeignKey(om => om.OrganizationId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(om => om.User)
                      .WithMany(u => u.OrganizationMemberships)
                      .HasForeignKey(om => om.UserId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(om => om.Role)
                      .WithMany(r => r.OrganizationMemberships)
                      .HasForeignKey(om => om.RoleId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Team>(entity =>
            {
                entity.HasKey(t => t.Id);

                entity.HasOne(t => t.Organization)
                      .WithMany(o => o.Teams)
                      .HasForeignKey(t => t.OrganizationId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<TeamMember>(entity =>
            {
                entity.HasKey(tm => tm.Id);

                entity.HasOne(tm => tm.Team)
                      .WithMany(t => t.Members)
                      .HasForeignKey(tm => tm.TeamId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(tm => tm.User)
                      .WithMany(u => u.TeamMemberships)
                      .HasForeignKey(tm => tm.UserId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.HasKey(p => p.Id);

                entity.HasOne(p => p.Organization)
                      .WithMany(o => o.Projects)
                      .HasForeignKey(p => p.OrganizationId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(p => p.Owner)
                      .WithMany()
                      .HasForeignKey(p => p.OwnerId)
                      .OnDelete(DeleteBehavior.Restrict);
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

            modelBuilder.Entity<OrganizationMembership>().HasData(
                new OrganizationMembership
                {
                    Id = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"),
                    OrganizationId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                    UserId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                    RoleId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    CreatedAt = SeedDate
                }
            );
        }
    }
}
