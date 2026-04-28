namespace ProjectManager.Application.DTOs.Auth;

public sealed class InviteUserResponse
{
    public string InvitationToken { get; set; } = null!;
    public string Email { get; set; } = null!;
    public DateTime ExpiresAt { get; set; }
}