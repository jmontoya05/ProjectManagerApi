using ProjectManager.Application.DTOs.Requests;
using ProjectManager.Application.DTOs.Responses;

namespace ProjectManager.Application.UseCases.Auth.Refresh
{
    public interface IRefreshUseCase
    {
        Task<RefreshResponse> Execute(RefreshRequest request, Guid organizationId, CancellationToken ct = default);
    }
}
