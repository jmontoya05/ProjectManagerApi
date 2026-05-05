using Microsoft.EntityFrameworkCore;
using ProjectManager.Application.Services;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Enums;

namespace ProjectManager.Infrastructure.Persistence.Context
{
    public class ProjectManagerDbContext(
        DbContextOptions<ProjectManagerDbContext> options,
        ITenantContext tenantContext
    ) : DbContext(options)
    {
        private readonly ITenantContext _tenantContext = tenantContext;

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
        public DbSet<WorkItem> WorkItems => Set<WorkItem>();
        public DbSet<ProjectMembership> ProjectMemberships => Set<ProjectMembership>();
        public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
        public DbSet<Invitation> Invitations => Set<Invitation>();

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries<EntityBase>();
            foreach (var entry in entries)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = DateTime.UtcNow;
                        entry.Entity.UpdatedAt = DateTime.UtcNow;
                        entry.Entity.CreatedBy = _tenantContext.TryGetUserId();
                        entry.Entity.UpdatedBy = _tenantContext.TryGetUserId();
                        break;
                    case EntityState.Modified:
                        entry.Entity.UpdatedAt = DateTime.UtcNow;
                        entry.Entity.UpdatedBy = _tenantContext.GetUserIdOrThrow();
                        break;
                    case EntityState.Deleted:
                        entry.Entity.DeletedAt = DateTime.UtcNow;
                        entry.Entity.DeletedBy = _tenantContext.GetUserIdOrThrow();
                        break;
                    case EntityState.Detached:
                    case EntityState.Unchanged:
                        break;
                    default:
                        throw new ApplicationException();
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }

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
                        Status = UserStatus.Active,
                        CreatedAt = SeedDate,
                        UpdatedAt = SeedDate
                    }
                );

            modelBuilder.Entity<Organization>().HasData(
                new Organization
                {
                    Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                    Name = "Demo Organization",
                    Status = OrganizationStatus.Active,
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

            modelBuilder.Entity<RolePermission>(entity =>
            {
                entity.HasKey(rp => new { rp.RoleId, rp.PermissionId });
                entity.HasOne(rp => rp.Role)
                      .WithMany(r => r.RolePermissions)
                      .HasForeignKey(rp => rp.RoleId);
                entity.HasOne(rp => rp.Permission)
                      .WithMany(p => p.RolePermissions)
                      .HasForeignKey(rp => rp.PermissionId);
                entity.HasQueryFilter(rp => rp.Permission.DeletedAt == null);
            });
            modelBuilder.Entity<Invitation>(entity =>
            {
                entity.HasKey(i => i.Id);
                entity.Property(i => i.Email).IsRequired();
                entity.Property(i => i.Token).IsRequired();
                entity.Property(i => i.ExpiresAt).IsRequired();
                entity.Property(i => i.Accepted).HasDefaultValue(false);
            });

            modelBuilder.Entity<ProjectMembership>(entity =>
            {
                entity.HasKey(pm => pm.Id);
                entity.HasOne(pm => pm.Project)
                      .WithMany(p => p.ProjectMemberships)
                      .HasForeignKey(pm => pm.ProjectId)
                      .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(pm => pm.User)
                      .WithMany(u => u.ProjectMemberships)
                      .HasForeignKey(pm => pm.UserId)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(pm => pm.Role)
                      .WithMany(r => r.ProjectMemberships)
                      .HasForeignKey(pm => pm.RoleId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

             modelBuilder.Entity<WorkItem>(entity =>
             {
                 entity.HasKey(wi => wi.Id);
                 entity.HasOne(wi => wi.Project)
                       .WithMany(p => p.WorkItems)
                       .HasForeignKey(wi => wi.ProjectId)
                       .OnDelete(DeleteBehavior.Cascade);
                 entity.HasOne(wi => wi.ParentWorkItem)
                       .WithMany(wi => wi.Subtasks)
                       .HasForeignKey(wi => wi.ParentWorkItemId)
                       .OnDelete(DeleteBehavior.Restrict);
                 entity.HasOne(wi => wi.Assignee)
                       .WithMany()
                       .HasForeignKey(wi => wi.AssigneeId)
                       .OnDelete(DeleteBehavior.SetNull);
                 entity.HasOne(wi => wi.Team)
                       .WithMany()
                       .HasForeignKey(wi => wi.TeamId)
                       .OnDelete(DeleteBehavior.SetNull);
             });

            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.HasKey(rt => rt.Id);
                entity.HasOne(rt => rt.User)
                      .WithMany(u => u.RefreshTokens)
                      .HasForeignKey(rt => rt.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(r => r.Id);
                entity.HasOne(r => r.Organization)
                      .WithMany(o => o.Roles)
                      .HasForeignKey(r => r.OrganizationId)
                      .IsRequired(false)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Project>().HasQueryFilter(p => p.DeletedAt == null);
            modelBuilder.Entity<WorkItem>().HasQueryFilter(wi => wi.DeletedAt == null);
            modelBuilder.Entity<Team>().HasQueryFilter(t => t.DeletedAt == null);
            modelBuilder.Entity<OrganizationMembership>().HasQueryFilter(om => om.DeletedAt == null);
            modelBuilder.Entity<User>().HasQueryFilter(u => u.DeletedAt == null);
            modelBuilder.Entity<Organization>().HasQueryFilter(o => o.DeletedAt == null);
            modelBuilder.Entity<Role>().HasQueryFilter(r => r.DeletedAt == null);
            modelBuilder.Entity<Permission>().HasQueryFilter(p => p.DeletedAt == null);
            modelBuilder.Entity<RefreshToken>().HasQueryFilter(rt => rt.DeletedAt == null);
            modelBuilder.Entity<Invitation>().HasQueryFilter(i => i.DeletedAt == null);
            modelBuilder.Entity<TeamMember>().HasQueryFilter(tm => tm.DeletedAt == null);
            modelBuilder.Entity<ProjectMembership>().HasQueryFilter(pm => pm.DeletedAt == null);

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(u => u.Status).HasConversion<string>();
                entity.Ignore(u => u.PhoneNumber);
            });

            modelBuilder.Entity<Organization>(entity =>
            {
                entity.Property(o => o.Status).HasConversion<string>();
                entity.Ignore(o => o.Address);
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.Property(p => p.Status).HasConversion<string>();
            });

            modelBuilder.Entity<WorkItem>(entity =>
            {
                entity.Property(wi => wi.Type).HasConversion<string>();
                entity.Property(wi => wi.Priority).HasConversion<string>();
                entity.Property(wi => wi.Status).HasConversion<string>();
                entity.Property(wi => wi.CompletionPercentageValue).HasPrecision(5, 2);
                entity.Ignore(wi => wi.CompletionPercentage);
            });
        }
    }
}
