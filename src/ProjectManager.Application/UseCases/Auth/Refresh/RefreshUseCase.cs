using ProjectManager.Application.DTOs.Auth;
using ProjectManager.Application.Ports;
using ProjectManager.Application.Services;
using ProjectManager.Application.Exceptions;

namespace ProjectManager.Application.UseCases.Auth.Refresh
{
    public sealed class RefreshUseCase(IUserRepository userRepository, ITokenService tokenService, IOrganizationRepository organizationRepository, ITenantContext tenantContext) : IRefreshUseCase
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly ITokenService _tokenService = tokenService;
        private readonly IOrganizationRepository _organizationRepository = organizationRepository;
        private readonly ITenantContext _tenantContext = tenantContext;

        public async Task<RefreshResponse> Execute(RefreshRequest request, CancellationToken ct = default)
        {
            var stored = await _userRepository.GetValidRefreshTokenAsync(request.RefreshToken, ct)
                ?? throw new NotFoundException("Invalid or expired refresh token", "RefreshToken", request.RefreshToken);

            var orgId = Guid.Parse(_tenantContext.OrganizationId!);
            var organization = await _organizationRepository.GetByIdAsync(orgId, ct)
                ?? throw new NotFoundException("Organization not found", "Organization", orgId);

            var roles = await _userRepository.GetUserRolesByOrganizationAsync(stored.UserId, orgId, ct);
            var newAccessToken = _tokenService.GenerateAccessToken(stored.UserId, stored.User.Email, orgId, roles);
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
