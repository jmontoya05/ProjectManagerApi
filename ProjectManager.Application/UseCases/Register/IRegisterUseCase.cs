using ProjectManager.Application.DTOs.Requests;

namespace ProjectManager.Application.UseCases.Register
{
    public interface IRegisterUseCase
    {
        Task<Guid> Execute(RegisterRequest registerRequest, CancellationToken ct = default);
    }
}
