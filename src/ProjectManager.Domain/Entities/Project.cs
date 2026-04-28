namespace ProjectManager.Domain.Entities
{
    public class Project
    {
        public Guid Id { get; init; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Status { get; set; } = "Planning";
        public Guid OrganizationId { get; set; }
        public Guid OwnerId { get; init; }
        public DateTime CreatedAt { get; init; }
        public Guid CreatedBy { get; init; }
        public DateTime UpdatedAt { get; set; }
        public Guid UpdatedBy { get; set; }
        // Navigation properties
        public virtual Organization Organization { get; init; } = null!;
        public virtual User Owner { get; init; } = null!;
        public virtual ICollection<ProjectMembership> ProjectMemberships { get; init; } = [];
    }
}
