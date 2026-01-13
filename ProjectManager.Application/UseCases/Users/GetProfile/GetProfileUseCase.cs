using ProjectManager.Application.DTOs;
using ProjectManager.Application.Ports;

namespace ProjectManager.Application.UseCases.Users.GetProfile
{
    public sealed class GetProfileUseCase(IUserRepository userRepository) : IGetProfileUseCase
    {
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<UserDto> Execute(Guid userId, CancellationToken ct = default)
        {
            var user = await _userRepository.GetByIdAsync(userId, ct)
                ?? throw new InvalidOperationException("User not found");

            return new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                DisplayName = user.DisplayName,
                Status = user.Status
            };
        }
    }
}
