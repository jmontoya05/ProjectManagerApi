namespace ProjectManager.Application.DTOs.Auth;

public sealed class CompleteInvitationRequest
{
    public string InvitationToken { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Password { get; set; } = null!;
}