namespace ProjectManager.Domain.Entities
{
    public class Role : EntityBase
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public Guid? OrganizationId { get; init; }
        //Navigation properties
        public virtual Organization? Organization { get; init; }
        public virtual ICollection<OrganizationMembership> OrganizationMemberships { get; init; } = [];
        public virtual ICollection<ProjectMembership> ProjectMemberships { get; init; } = [];
        public virtual ICollection<RolePermission> RolePermissions { get; init; } = [];
    }
}
