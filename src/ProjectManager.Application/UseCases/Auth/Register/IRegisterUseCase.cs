namespace ProjectManager.Application.UseCases.Auth.Register
{
    public interface IRegisterUseCase
    {
        Task<Guid> Execute(RegisterRequest registerRequest, CancellationToken ct = default);
    }
}
