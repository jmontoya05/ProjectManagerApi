using ProjectManager.Application.DTOs.Requests;
using ProjectManager.Application.DTOs.Responses;

namespace ProjectManager.Application.UseCases.Login
{
    public interface ILoginUseCase
    {
        Task<LoginResponse> Execute(LoginRequest request, CancellationToken ct = default);
    }
}
