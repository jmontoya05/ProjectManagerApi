namespace ProjectManager.Application.DTOs.Permissions;

public sealed class CreatePermissionRequest
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
}