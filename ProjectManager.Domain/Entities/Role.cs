namespace ProjectManager.Domain.Entities
{
    public class Role
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Guid? UpdatedBy { get; set; }

        //Navigation properties
        //public virtual ICollection<OrganizationMembership> OrganizationMemberships { get; set; } = [];
    }
}
