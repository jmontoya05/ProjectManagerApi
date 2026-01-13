using ProjectManager.Application.DTOs.Requests;

namespace ProjectManager.Application.UseCases.Auth.Logout
{
    public interface ILogoutUseCase
    {
        Task Execute(LogoutRequest request, CancellationToken ct = default);
    }
}
