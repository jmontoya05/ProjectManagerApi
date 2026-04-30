namespace ProjectManager.Domain.Entities
{
    public class Team : EntityBase
    {
        public string Name { get; init; } = null!;
        public Guid OrganizationId { get; set; }
        //Navigation Properties
        public virtual Organization Organization { get; init; } = null!;
        public virtual ICollection<TeamMember> Members { get; init; } = [];
    }
}
