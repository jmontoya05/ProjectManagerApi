namespace ProjectManager.Application.UseCases.Auth.Logout
{
    public interface ILogoutUseCase
    {
        Task Execute(LogoutRequest request, CancellationToken ct = default);
    }
}
