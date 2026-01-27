namespace ProjectManager.Domain.Entities
{
    public class Project
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Status { get; set; } = "Planning";
        public Guid OrganizationId { get; set; }
        public Guid OwnerId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation properties
        public virtual Organization Organization { get; set; } = null!;
        public virtual User Owner { get; set; } = null!;
    }
}
