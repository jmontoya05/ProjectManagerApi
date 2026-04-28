namespace ProjectManager.Application.DTOs.Permissions;

public sealed class PermissionDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
}