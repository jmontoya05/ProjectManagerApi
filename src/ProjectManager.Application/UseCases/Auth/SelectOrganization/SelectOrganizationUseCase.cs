using ProjectManager.Application.Ports;
using ProjectManager.Application.Services;

namespace ProjectManager.Application.UseCases.Auth.SelectOrganization
{
    public sealed class SelectOrganizationUseCase(IUserRepository userRepository, ITokenService tokenService) : ISelectOrganizationUseCase
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly ITokenService _tokenService = tokenService;

        public async Task<SelectOrganizationResponse> Execute(SelectOrganizationRequest request, CancellationToken ct = default)
        {
            var stored = await _userRepository.GetValidRefreshTokenAsync(request.Refreshtoken, ct)
                ?? throw new InvalidOperationException("Invalid or expired refresh token");
            var userId = stored.UserId;
            var organizationId = request.OrganizationId;

            if (!await _userRepository.UserBelongsToOrganizationAsync(userId, organizationId, ct))
                throw new InvalidOperationException("User not in organization");

            var roles = await _userRepository.GetUserRolesByOrganizationAsync(userId, organizationId, ct);
            var token = _tokenService.GenerateAccessToken(userId, stored.User.Email, organizationId, roles);

            return new SelectOrganizationResponse
            {
                Token = token,
                RefreshToken = stored.Token,
                ExpiresInMinutes = 60
            };
        }
    }
}
