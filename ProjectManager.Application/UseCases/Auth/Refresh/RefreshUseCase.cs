using ProjectManager.Application.DTOs.Requests;
using ProjectManager.Application.DTOs.Responses;
using ProjectManager.Application.Ports;
using ProjectManager.Application.Services;

namespace ProjectManager.Application.UseCases.Auth.Refresh
{
    public sealed class RefreshUseCase(IUserRepository userRepository, ITokenService tokenService) : IRefreshUseCase
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly ITokenService _tokenService = tokenService;

        public async Task<RefreshResponse> Execute(RefreshRequest request, Guid organizationId, CancellationToken ct = default)
        {
            var stored = await _userRepository.GetValidRefreshTokenAsync(request.RefreshToken, ct)
                ?? throw new InvalidOperationException("Invalid or expired refresh token");

            var roles = await _userRepository.GetUserRolesByOrganizationAsync(stored.UserId, organizationId, ct);
            var newAccessToken = _tokenService.GenerateAccessToken(stored.UserId, stored.User.Email, organizationId, roles);
            var newRefreshToken = _tokenService.GenerateRefreshToken();
            await _userRepository.RevokeRefreshTokenAsync(stored, ct);
            await _userRepository.SaveRefreshTokenAsync(stored.UserId, newRefreshToken, DateTime.UtcNow.AddDays(7), ct);

            return new RefreshResponse
            {
                Token = newAccessToken,
                ExpiresInMinutes = 60,
                RefreshToken = newRefreshToken
            };
        }
    }
}
