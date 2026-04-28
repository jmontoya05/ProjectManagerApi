using ProjectManager.Application.DTOs.Auth;
using ProjectManager.Application.Ports;
using ProjectManager.Application.Services;
using ProjectManager.Application.Exceptions;

namespace ProjectManager.Application.UseCases.Auth.Refresh
{
    public sealed class RefreshUseCase(
        IUserRepository userRepository, 
        ITokenService tokenService, 
        IOrganizationRepository organizationRepository
    ) : IRefreshUseCase
        {
            private readonly IUserRepository _userRepository = userRepository;
            private readonly ITokenService _tokenService = tokenService;
            private readonly IOrganizationRepository _organizationRepository = organizationRepository;

            public async Task<RefreshResponse> Execute(RefreshRequest request, CancellationToken ct = default)
            {
                var stored = await _userRepository.GetValidRefreshTokenAsync(request.RefreshToken, ct)
                    ?? throw new NotFoundException("Invalid or expired refresh token", "RefreshToken", request.RefreshToken);

                var orgId = request.OrganizationId;
                _ = await _organizationRepository.GetByIdAsync(orgId, ct)
                    ?? throw new NotFoundException("Organization not found", "Organization", orgId);

                var roles = await _userRepository.GetUserRolesByOrganizationAsync(stored.UserId, orgId, ct);
                var enumerable = roles as string[] ?? roles.ToArray();
                if (roles == null || enumerable.Length == 0)
                    throw new ForbiddenException("User does not have access to this organization.");

                var newAccessToken = _tokenService.GenerateAccessToken(stored.UserId, stored.User.Email, orgId, enumerable);
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
