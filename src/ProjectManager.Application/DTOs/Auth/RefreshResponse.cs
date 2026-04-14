namespace ProjectManager.Application.DTOs.Auth
{
    public sealed class RefreshResponse
    {
        public string Token { get; set; } = null!;
        public int ExpiresInMinutes { get; set; }
        public string RefreshToken { get; set; } = null!;
    }
}
