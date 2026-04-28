using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.Application.DTOs.Auth;
using ProjectManager.Application.UseCases.Auth.Invite;
using ProjectManager.Application.UseCases.Auth.Login;
using ProjectManager.Application.UseCases.Auth.Logout;
using ProjectManager.Application.UseCases.Auth.Refresh;
using ProjectManager.Application.UseCases.Auth.SelectOrganization;

namespace ProjectManager.Api.Controllers
{
    [ApiController]
    [Route("auth")]
    public sealed class AuthController(
        ILoginUseCase loginUseCase, 
        ISelectOrganizationUseCase selectOrganizationUseCase, 
        IRefreshUseCase refreshUseCase, 
        ILogoutUseCase logoutUseCase, 
        IInviteUserUseCase inviteUserUseCase, 
        ICompleteInvitationUseCase completeInvitationUseCase
    ) : ControllerBase
    {
        private readonly ILoginUseCase _loginUseCase = loginUseCase;
        private readonly ISelectOrganizationUseCase _selectOrganizationUse = selectOrganizationUseCase;
        private readonly IRefreshUseCase _refreshUseCase = refreshUseCase;
        private readonly ILogoutUseCase _logoutUseCase = logoutUseCase;

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var response = await _loginUseCase.Execute(request);
            return Ok(response);
        }

        [HttpPost("organization")]
        public async Task<IActionResult> SelectOrganization([FromBody] SelectOrganizationRequest request)
        {
            var response = await _selectOrganizationUse.Execute(request);
            return Ok(response);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequest request)
        {
            var response = await _refreshUseCase.Execute(request);
            return Ok(response);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] LogoutRequest request)
        {
            await _logoutUseCase.Execute(request);
            return NoContent();
        }

        [HttpPost("invite")]
        public async Task<IActionResult> InviteUser([FromBody] InviteUserRequest request)
        {
            var adminUserId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var response = await inviteUserUseCase.Execute(request, adminUserId);
            return Ok(response);
        }

        [HttpPost("complete-invitation")]
        public async Task<IActionResult> CompleteInvitation([FromBody] CompleteInvitationRequest request)
        {
            var userId = await completeInvitationUseCase.Execute(request);
            return Ok(new { UserId = userId });
        }
    }
}
