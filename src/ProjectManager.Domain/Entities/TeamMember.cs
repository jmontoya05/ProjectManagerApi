namespace ProjectManager.Domain.Entities
{
    public class TeamMember : EntityBase
    {
        public Guid TeamId { get; init; }
        public Guid UserId { get; init; }

        //Navigation Properties
        public virtual Team Team { get; init; } = null!;
        public virtual User User { get; init; } = null!;
    }
}
