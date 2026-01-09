namespace ProjectManager.Domain.Entities
{
    public class OrganizationMembership
    {
        public Guid Id { get; set; }
        public Guid OrganizationId { get; set; }
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
        public DateTime CreatedAt { get; set; }

        //Navigation properties
        //public virtual Organization Organization { get; set; } = null!;
        //public virtual User User { get; set; } = null!;
        //public virtual Role Role { get; set; } = null!;
    }
}
