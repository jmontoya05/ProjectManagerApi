namespace ProjectManager.Domain.Entities
{
    public class Role
    {
        public Guid Id { get; init; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime CreatedAt { get; init; }
        public Guid? CreatedBy { get; init; }
        public DateTime UpdatedAt { get; set; }
        public Guid? UpdatedBy { get; init; }
        public Guid? OrganizationId { get; init; }
        //Navigation properties
        public virtual Organization? Organization { get; init; }
        public virtual ICollection<OrganizationMembership> OrganizationMemberships { get; init; } = [];
        public virtual ICollection<ProjectMembership> ProjectMemberships { get; init; } = [];
        public virtual ICollection<RolePermission> RolePermissions { get; init; } = [];
    }
}
