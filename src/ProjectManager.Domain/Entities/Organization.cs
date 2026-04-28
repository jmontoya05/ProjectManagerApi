namespace ProjectManager.Domain.Entities
{
    public class Organization
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = null!;
        public string Status { get; init; } = null!;
        public Guid OwnerId { get; init; }
        public DateTime CreatedAt { get; init; }
        public Guid? CreatedBy { get; init; }
        public DateTime UpdatedAt { get; init; }
        public Guid? UpdatedBy { get; init; }
        //Navigation properties
        public virtual User Owner { get; init; } = null!;
        public virtual ICollection<OrganizationMembership> OrganizationMemberships { get; init; } = [];
        public virtual ICollection<Team> Teams { get; init; } = [];
        public virtual ICollection<Project> Projects { get; init; } = [];
        public virtual ICollection<Role> Roles { get; init; } = [];
    }
}
