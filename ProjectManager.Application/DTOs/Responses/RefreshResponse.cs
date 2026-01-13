namespace ProjectManager.Application.DTOs.Responses
{
    public sealed class RefreshResponse
    {
        public string AccesToken { get; set; } = null!;
        public int ExpiresInSeconds { get; set; }
        public string RefreshToken { get; set; } = null!;
    }
}
