namespace ProjectManager.Domain.Entities
{
    public class RolePermission
    {
        public Guid RoleId { get; init; }
        public Role Role { get; init; } = null!;
        public Guid PermissionId { get; init; }
        public Permission Permission { get; init; } = null!;
    }
}
