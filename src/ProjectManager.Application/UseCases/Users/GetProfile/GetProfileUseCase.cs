using ProjectManager.Application.DTOs.Users;
using ProjectManager.Application.Ports;
using ProjectManager.Application.Exceptions;

namespace ProjectManager.Application.UseCases.Users.GetProfile
{
    public sealed class GetProfileUseCase(IUserRepository userRepository) : IGetProfileUseCase
    {
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<GetProfileResponse> Execute(Guid userId, CancellationToken ct = default)
        {
            var user = await _userRepository.GetByIdAsync(userId, ct)
                ?? throw new NotFoundException("User not found", "User", userId);

            return new GetProfileResponse
            {
                Id = user.Id,
                Email = user.Email,
                DisplayName = user.DisplayName,
                Status = user.Status
            };
        }
    }
}
