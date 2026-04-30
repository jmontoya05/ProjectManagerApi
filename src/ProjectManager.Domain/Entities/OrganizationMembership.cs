namespace ProjectManager.Domain.Entities
{
    public class OrganizationMembership : EntityBase
    {
        public Guid OrganizationId { get; init; }
        public Guid UserId { get; init; }
        public Guid RoleId { get; init; }

        //Navigation properties
        public virtual Organization Organization { get; init; } = null!;
        public virtual User User { get; init; } = null!;
        public virtual Role Role { get; init; } = null!;
    }
}
