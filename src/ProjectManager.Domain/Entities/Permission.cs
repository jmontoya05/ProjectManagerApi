namespace ProjectManager.Domain.Entities
{
    public class Permission : EntityBase
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        //Navigation Properties
        public virtual ICollection<RolePermission> RolePermissions { get; init; } = [];
    }
}
