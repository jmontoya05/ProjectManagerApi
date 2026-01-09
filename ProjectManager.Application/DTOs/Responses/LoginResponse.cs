namespace ProjectManager.Application.DTOs.Responses
{
    public sealed class LoginResponse
    {
        public string AccessToken { get; set; } = null!;
        public int ExpiresInSeconds { get; set; }
        public string RefreshToken { get; set; } = null!;
        public UserDto User { get; set; } = null!;
    }
}
