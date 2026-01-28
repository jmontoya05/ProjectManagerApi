namespace ProjectManager.Application.UseCases.Auth.Login
{
    public sealed class LoginRequest
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
