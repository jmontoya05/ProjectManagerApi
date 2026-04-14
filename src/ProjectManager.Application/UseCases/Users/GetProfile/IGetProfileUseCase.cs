using ProjectManager.Application.DTOs.Users;

namespace ProjectManager.Application.UseCases.Users.GetProfile
{
    public interface IGetProfileUseCase
    {
        Task<GetProfileResponse> Execute(Guid userId, CancellationToken ct = default);
    }
}
