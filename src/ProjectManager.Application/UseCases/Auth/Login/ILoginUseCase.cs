using ProjectManager.Application.DTOs.Auth;

namespace ProjectManager.Application.UseCases.Auth.Login
{
    public interface ILoginUseCase
    {
        Task<LoginResponse> Execute(LoginRequest request, CancellationToken ct = default);
    }
}
