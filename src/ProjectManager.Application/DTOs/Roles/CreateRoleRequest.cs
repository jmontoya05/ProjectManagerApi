namespace ProjectManager.Application.DTOs.Roles;

public sealed class CreateRoleRequest
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public Guid? OrganizationId { get; set; }
}