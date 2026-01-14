using ProjectManager.Application.DTOs.Requests;
using ProjectManager.Application.Ports;

namespace ProjectManager.Application.UseCases.Auth.Logout
{
    public sealed class LogoutUseCase(IUserRepository userRepository) : ILogoutUseCase
    {
        private readonly IUserRepository _userRepository = userRepository;

        public async Task Execute(LogoutRequest request, CancellationToken ct = default)
        {
            var token = await _userRepository.GetValidRefreshTokenAsync(request.RefreshToken, ct)
                ?? throw new InvalidOperationException("Invalid refresh token");

            await _userRepository.RevokeRefreshTokenAsync(token, ct);
        }
    }
}
