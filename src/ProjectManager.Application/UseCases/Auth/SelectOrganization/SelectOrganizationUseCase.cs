using ProjectManager.Application.DTOs.Auth;
using ProjectManager.Application.Ports;
using ProjectManager.Application.Services;
using ProjectManager.Application.Exceptions;

namespace ProjectManager.Application.UseCases.Auth.SelectOrganization
{
    public sealed class SelectOrganizationUseCase(
        IUserRepository userRepository, 
        ITokenService tokenService
    ) : ISelectOrganizationUseCase
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly ITokenService _tokenService = tokenService;

        public async Task<SelectOrganizationResponse> Execute(SelectOrganizationRequest request, CancellationToken ct = default)
        {
            var stored = await _userRepository.GetValidRefreshTokenAsync(request.Refreshtoken, ct)
                ?? throw new NotFoundException("Invalid or expired refresh token", "RefreshToken", request.Refreshtoken);
            var userId = stored.UserId;
            var orgId = request.OrganizationId;
            var isMember = await _userRepository.IsUserMemberOfOrganizationAsync(userId, orgId, ct);
            if (!isMember)
                throw new ForbiddenException("User does not belong to this organization.");
            
            var roles = await _userRepository.GetUserRolesByOrganizationAsync(userId, orgId, ct);
            var enumerable = roles as string[] ?? roles.ToArray();
            if (roles == null || enumerable.Length == 0)
                throw new ForbiddenException("User does not have access to this organization.");

            var permissions = await _userRepository.GetUserPermissionsByOrganizationAsync(userId, orgId, ct);
            var token = _tokenService.GenerateAccessToken(userId, stored.User.Email, orgId, enumerable, permissions);

            return new SelectOrganizationResponse
            {
                Token = token,
                RefreshToken = stored.Token,
                ExpiresInMinutes = 60
            };
        }
    }
}
