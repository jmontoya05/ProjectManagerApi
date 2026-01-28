using ProjectManager.Application.Ports;
using ProjectManager.Application.Services;

namespace ProjectManager.Application.UseCases.Auth.Refresh
{
    public sealed class RefreshUseCase(IUserRepository userRepository, ITokenService tokenService, IOrganizationRepository organizationRepository) : IRefreshUseCase
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly ITokenService _tokenService = tokenService;
        private readonly IOrganizationRepository _organizationRepository = organizationRepository;

        public async Task<RefreshResponse> Execute(RefreshRequest request, CancellationToken ct = default)
        {
            var stored = await _userRepository.GetValidRefreshTokenAsync(request.RefreshToken, ct)
                ?? throw new InvalidOperationException("Invalid or expired refresh token");

            var organization = await _organizationRepository.GetByIdAsync(request.OrganizationId, ct)
                ?? throw new InvalidOperationException("Organization not found");

            if (!await _userRepository.UserBelongsToOrganizationAsync(stored.UserId, organization.Id, ct))
            {
                throw new InvalidOperationException("User does not belong to the specified organization");
            }

            var roles = await _userRepository.GetUserRolesByOrganizationAsync(stored.UserId, organization.Id, ct);
            var newAccessToken = _tokenService.GenerateAccessToken(stored.UserId, stored.User.Email, organization.Id, roles);
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
