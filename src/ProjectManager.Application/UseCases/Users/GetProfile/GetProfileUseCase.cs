using ProjectManager.Application.DTOs.Users;
using ProjectManager.Application.Ports;
using ProjectManager.Application.Exceptions;
using ProjectManager.Application.Services;

namespace ProjectManager.Application.UseCases.Users.GetProfile
{
    public sealed class GetProfileUseCase(
        IUserRepository userRepository,
        ITenantContext tenantContext
    ) : IGetProfileUseCase
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly ITenantContext _tenantContext = tenantContext;

        public async Task<GetProfileResponse> Execute(CancellationToken ct = default)
        {
            var userId = _tenantContext.GetUserIdOrThrow();
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
