using ProjectManager.Application.DTOs.Auth;

namespace ProjectManager.Application.UseCases.Auth.Register
{
    public interface IRegisterUseCase
    {
        Task<Guid> Execute(RegisterRequest registerRequest, CancellationToken ct = default);
    }
}
