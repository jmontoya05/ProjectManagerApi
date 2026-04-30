namespace ProjectManager.Domain.Entities
{
    public class ProjectMembership : EntityBase
    {
        public Guid ProjectId { get; init; }
        public Guid UserId { get; init; }
        public Guid RoleId { get; init; }
        // Navigation properties
        public virtual Project Project { get; init; } = null!;
        public virtual User User { get; init; } = null!;
        public virtual Role Role { get; init; } = null!;
    }
}
