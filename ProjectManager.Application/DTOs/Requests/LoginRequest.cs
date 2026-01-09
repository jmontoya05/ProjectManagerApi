namespace ProjectManager.Application.DTOs.Requests
{
    public sealed class LoginRequest
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
