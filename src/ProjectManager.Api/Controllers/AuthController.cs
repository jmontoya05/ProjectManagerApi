using Microsoft.AspNetCore.Mvc;
using ProjectManager.Api.Middlewares;
using ProjectManager.Application.DTOs.Auth;
using ProjectManager.Application.UseCases.Auth.Login;
using ProjectManager.Application.UseCases.Auth.Logout;
using ProjectManager.Application.UseCases.Auth.Refresh;
using ProjectManager.Application.UseCases.Auth.Register;
using ProjectManager.Application.UseCases.Auth.SelectOrganization;

namespace ProjectManager.Api.Controllers
{
    [ApiController]
    [Route("auth")]
    public sealed class AuthController(IRegisterUseCase registerUseCase, ILoginUseCase loginUseCase, ISelectOrganizationUseCase selectOrganizationUseCase, IRefreshUseCase refreshUseCase, ILogoutUseCase logoutUseCase) : ControllerBase
    {
        private readonly IRegisterUseCase _registerUseCase = registerUseCase;
        private readonly ILoginUseCase _loginUseCase = loginUseCase;
        private readonly ISelectOrganizationUseCase _selectOrganizationUse = selectOrganizationUseCase;
        private readonly IRefreshUseCase _refreshUseCase = refreshUseCase;
        private readonly ILogoutUseCase _logoutUseCase = logoutUseCase;

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var userId = await _registerUseCase.Execute(request);
            return CreatedAtAction(nameof(Register), new { id = userId }, new { id = userId });
        }

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

        private string GetCorrelationId() =>
            ExceptionHandlingMiddleware.GetCorrelationId(HttpContext);
    }
}
