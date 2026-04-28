namespace ProjectManager.Application.DTOs.Roles;

public sealed class UpdateRoleRequest
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
}