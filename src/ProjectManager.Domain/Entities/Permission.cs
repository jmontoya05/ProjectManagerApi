namespace ProjectManager.Domain.Entities
{
    public class Permission
    {
        public Guid Id { get; init; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime CreatedAt { get; init; }
        public Guid? CreatedBy { get; init; }
        public DateTime UpdatedAt { get; set; }
        public Guid? UpdatedBy { get; init; }
        //Navigation Properties
        public virtual ICollection<RolePermission> RolePermissions { get; init; } = [];
    }
}
