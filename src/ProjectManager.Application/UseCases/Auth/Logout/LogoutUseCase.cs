using ProjectManager.Application.DTOs.Auth;
using ProjectManager.Application.Ports;
using ProjectManager.Application.Exceptions;

namespace ProjectManager.Application.UseCases.Auth.Logout
{
    public sealed class LogoutUseCase(IUserRepository userRepository) : ILogoutUseCase
    {
        private readonly IUserRepository _userRepository = userRepository;

        public async Task Execute(LogoutRequest request, CancellationToken ct = default)
        {
            var token = await _userRepository.GetValidRefreshTokenAsync(request.RefreshToken, ct)
                ?? throw new NotFoundException("Refresh token not found or already revoked.", "RefreshToken", request.RefreshToken);

            await _userRepository.RevokeRefreshTokenAsync(token, ct);
        }
    }
}
