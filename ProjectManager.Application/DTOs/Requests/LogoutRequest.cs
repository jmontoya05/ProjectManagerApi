namespace ProjectManager.Application.DTOs.Requests
{
    public sealed class LogoutRequest
    {
        public string RefreshToken { get; set; } = null!;
    }
}
