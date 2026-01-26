namespace ProjectManager.Application.UseCases.Auth.Refresh
{
    public sealed class RefreshResponse
    {
        public string Token { get; set; } = null!;
        public int ExpiresInMinutes { get; set; }
        public string RefreshToken { get; set; } = null!;
    }
}
