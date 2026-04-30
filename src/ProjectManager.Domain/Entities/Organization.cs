namespace ProjectManager.Domain.Entities
{
    public class Organization : EntityBase
    {
        public string Name { get; init; } = null!;
        public string Status { get; init; } = null!;
        public Guid OwnerId { get; init; }
        //Navigation properties
        public virtual User Owner { get; init; } = null!;
        public virtual ICollection<OrganizationMembership> OrganizationMemberships { get; init; } = [];
        public virtual ICollection<Team> Teams { get; init; } = [];
        public virtual ICollection<Project> Projects { get; init; } = [];
        public virtual ICollection<Role> Roles { get; init; } = [];
    }
}
