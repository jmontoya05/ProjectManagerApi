namespace ProjectManager.Application.DTOs.Auth
{
    public sealed class LogoutRequest
    {
        public string RefreshToken { get; set; } = null!;
    }
}
