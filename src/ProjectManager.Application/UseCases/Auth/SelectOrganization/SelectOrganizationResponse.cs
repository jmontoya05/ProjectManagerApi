namespace ProjectManager.Application.UseCases.Auth.SelectOrganization
{
    public sealed class SelectOrganizationResponse
    {
        public string Token { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
        public int ExpiresInMinutes { get; set; }
    }
}
