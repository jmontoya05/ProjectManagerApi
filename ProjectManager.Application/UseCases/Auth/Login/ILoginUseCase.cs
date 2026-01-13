using ProjectManager.Application.DTOs.Requests;
using ProjectManager.Application.DTOs.Responses;

namespace ProjectManager.Application.UseCases.Auth.Login
{
    public interface ILoginUseCase
    {
        Task<LoginResponse> Execute(LoginRequest request, CancellationToken ct = default);
    }
}
