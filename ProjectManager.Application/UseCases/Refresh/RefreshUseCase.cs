using ProjectManager.Application.DTOs.Requests;
using ProjectManager.Application.DTOs.Responses;
using ProjectManager.Application.Ports;
using ProjectManager.Application.Services;

namespace ProjectManager.Application.UseCases.Refresh
{
    public sealed class RefreshUseCase(IUserRepository userRepository, ITokenService tokenService) : IRefreshUseCase
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly ITokenService _tokenService = tokenService;

        public async Task<RefreshResponse> Execute(RefreshRequest request, CancellationToken ct = default)
        {
            var stored = await _userRepository.GetValidRefreshTokenAsync(request.RefreshToken, ct)
                ?? throw new InvalidOperationException("Invalid or expired refresh token");

            var newAccessToken = _tokenService.GenerateAccessToken(stored.UserId, stored.User.Email);
            var newRefreshToken = _tokenService.GenerateRefreshToken();
            await _userRepository.RevokeRefreshTokenAsync(stored, ct);
            await _userRepository.SaveRefreshTokenAsync(stored.UserId, newRefreshToken, DateTime.UtcNow.AddDays(7), ct);

            return new RefreshResponse
            {
                AccesToken = newAccessToken,
                ExpiresInSeconds = 15 * 60,
                RefreshToken = newRefreshToken
            };
        }
    }
}
