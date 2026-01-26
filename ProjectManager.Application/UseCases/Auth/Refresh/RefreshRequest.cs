namespace ProjectManager.Application.UseCases.Auth.Refresh
{
    public sealed class RefreshRequest
    {
        public string RefreshToken { get; set; } = null!;
    }
}
