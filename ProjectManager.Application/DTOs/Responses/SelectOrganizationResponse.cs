namespace ProjectManager.Application.DTOs.Responses
{
    public sealed class SelectOrganizationResponse
    {
        public string Token { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
        public int ExpiresInMinutes { get; set; }
    }
}
