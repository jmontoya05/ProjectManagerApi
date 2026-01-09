using ProjectManager.Application.DTOs;

namespace ProjectManager.Application.UseCases.Register
{
    public interface IRegisterUseCase
    {
        Task<Guid> Execute(RegisterRequest registerRequest, CancellationToken ct = default);
    }
}
