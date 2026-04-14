using ProjectManager.Application.DTOs.Auth;
using ProjectManager.Application.Ports;
using ProjectManager.Application.Services;
using ProjectManager.Application.Exceptions;

namespace ProjectManager.Application.UseCases.Auth.SelectOrganization
{
    public sealed class SelectOrganizationUseCase(IUserRepository userRepository, ITokenService tokenService, ITenantContext tenantContext) : ISelectOrganizationUseCase
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly ITokenService _tokenService = tokenService;
        private readonly ITenantContext _tenantContext = tenantContext;

        public async Task<SelectOrganizationResponse> Execute(SelectOrganizationRequest request, CancellationToken ct = default)
        {
            var stored = await _userRepository.GetValidRefreshTokenAsync(request.Refreshtoken, ct)
                ?? throw new NotFoundException("Invalid or expired refresh token", "RefreshToken", request.Refreshtoken);
            var userId = stored.UserId;
            var orgId = Guid.Parse(_tenantContext.OrganizationId!);

            var roles = await _userRepository.GetUserRolesByOrganizationAsync(userId, orgId, ct);
            var token = _tokenService.GenerateAccessToken(userId, stored.User.Email, orgId, roles);

            return new SelectOrganizationResponse
            {
                Token = token,
                RefreshToken = stored.Token,
                ExpiresInMinutes = 60
            };
        }
    }
}
