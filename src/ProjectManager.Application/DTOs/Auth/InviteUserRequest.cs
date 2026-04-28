namespace ProjectManager.Application.DTOs.Auth;

public sealed class InviteUserRequest
{
    public string Email { get; set; } = null!;
    public Guid OrganizationId { get; set; }
    public Guid RoleId { get; set; }
}