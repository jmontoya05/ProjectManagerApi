namespace ProjectManager.Application.UseCases.Auth.Logout
{
    public sealed class LogoutRequest
    {
        public string RefreshToken { get; set; } = null!;
    }
}
