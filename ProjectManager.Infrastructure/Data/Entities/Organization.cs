namespace ProjectManager.Infrastructure.Data.Entities
{
    public class Organization
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Status { get; set; } = null!;
        public Guid OwnerId { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Guid? UpdatedBy { get; set; }

        //Navigation properties
        public virtual User Owner { get; set; } = null!;
        //public virtual ICollection<OrganizationMembership> OrganizationMemberships { get; set; } = [];

    }
}
