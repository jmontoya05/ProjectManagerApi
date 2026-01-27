namespace ProjectManager.Application.UseCases.Auth.Refresh
{
    public interface IRefreshUseCase
    {
        Task<RefreshResponse> Execute(RefreshRequest request, CancellationToken ct = default);
    }
}
