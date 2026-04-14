namespace ProjectManager.Application.DTOs.Auth
{
    public sealed class SelectOrganizationResponse
    {
        public string Token { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
        public int ExpiresInMinutes { get; set; }
    }
}
