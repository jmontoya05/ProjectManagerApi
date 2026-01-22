namespace ProjectManager.Domain.Entities
{
    public class Team
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public Guid OrganizationId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        //Navigation Properties
        public virtual Organization Organization { get; set; } = null!;
        public virtual ICollection<TeamMember> Members { get; set; } = [];
    }
}
