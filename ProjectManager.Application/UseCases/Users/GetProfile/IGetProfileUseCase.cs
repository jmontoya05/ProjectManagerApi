using ProjectManager.Application.DTOs;

namespace ProjectManager.Application.UseCases.Users.GetProfile
{
    public interface IGetProfileUseCase
    {
        Task<UserDto> Execute(Guid userId, CancellationToken ct = default);
    }
}
