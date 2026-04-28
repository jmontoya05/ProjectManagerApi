namespace ProjectManager.Domain.Entities
{
    public class ProjectMembership
    {
        public Guid Id { get; init; }
        public Guid ProjectId { get; init; }
        public Guid UserId { get; init; }
        public Guid RoleId { get; init; }
        public DateTime CreatedAt { get; init; }
        // Navigation properties
        public virtual Project Project { get; init; } = null!;
        public virtual User User { get; init; } = null!;
        public virtual Role Role { get; init; } = null!;
    }
}
