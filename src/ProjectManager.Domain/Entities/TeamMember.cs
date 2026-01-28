namespace ProjectManager.Domain.Entities
{
    public class TeamMember
    {
        public Guid Id { get; set; }
        public Guid TeamId { get; set; }
        public Guid UserId { get; set; }
        public DateTime JoinedAt { get; set; }

        //Navigation Properties
        public virtual Team Team { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
