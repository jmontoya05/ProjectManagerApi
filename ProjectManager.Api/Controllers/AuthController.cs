using Microsoft.AspNetCore.Mvc;
using ProjectManager.Api.Middlewares;
using ProjectManager.Application.DTOs.Requests;
using ProjectManager.Application.UseCases.Auth.Login;
using ProjectManager.Application.UseCases.Auth.Logout;
using ProjectManager.Application.UseCases.Auth.Refresh;
using ProjectManager.Application.UseCases.Auth.Register;
using ProjectManager.Application.UseCases.Auth.SelectOrganization;

namespace ProjectManager.Api.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController(IRegisterUseCase registerUseCase, ILoginUseCase loginUseCase, ISelectOrganizationUseCase selectOrganizationUseCase, IRefreshUseCase refreshUseCase, ILogoutUseCase logoutUseCase) : ControllerBase
    {
        private readonly IRegisterUseCase _registerUseCase = registerUseCase;
        private readonly ILoginUseCase _loginUseCase = loginUseCase;
        private readonly ISelectOrganizationUseCase _selectOrganizationUse = selectOrganizationUseCase;
        private readonly IRefreshUseCase _refreshUseCase = refreshUseCase;
        private readonly ILogoutUseCase _logoutUseCase = logoutUseCase;

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                var userId = await _registerUseCase.Execute(request);
                return CreatedAtAction(nameof(Register), new { id = userId }, new { id = userId });

            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("already registered"))
            {

                return Conflict(
                    new
                    {
                        correlationId = GetCorrelationId(),
                        errorCode = "EMAIL_ALREADY_EXISTS",
                        message = "Email already registered"
                    });
            }
            catch (Exception)
            {
                return StatusCode(
                    500,
                    new
                    {
                        correlationId = GetCorrelationId(),
                        errorCode = "INTERNAL_ERROR",
                        message = "An unexpected error occurred"
                    });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var response = await _loginUseCase.Execute(request);
                return Ok(response);
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("The email doesn't exists") ||
                                                        ex.Message.Contains("Invalid password") ||
                                                        ex.Message.Contains("User is blocked"))
            {
                return Unauthorized(
                    new
                    {
                        correlationId = GetCorrelationId(),
                        errorCode = "UNAUTHORIZED",
                        message = ex.Message
                    });
            }
            catch (Exception)
            {
                return StatusCode(
                    500,
                    new
                    {
                        correlationId = GetCorrelationId(),
                        errorCode = "INTERNAL_ERROR",
                        message = "An unexpected error occurred"
                    });
            }
        }

        [HttpPost("organization")]
        public async Task<IActionResult> SelectOrganization([FromBody] SelectOrganizationRequest request)
        {
            try
            {
                var response = await _selectOrganizationUse.Execute(request);
                return Ok(response);
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("Invalid or expired refresh token") ||
                                                        ex.Message.Contains("User not in organization"))
            {
                return Unauthorized(
                    new
                    {
                        correlationId = GetCorrelationId(),
                        errorCode = "UNAUTHORIZED",
                        message = ex.Message
                    });
            }
            catch (Exception)
            {
                return StatusCode(
                    500,
                    new
                    {
                        correlationId = GetCorrelationId(),
                        errorCode = "INTERNAL_ERROR",
                        message = "An unexpected error occurred"
                    });
            }
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequest request)
        {
            try
            {
                var orgClaim = User.FindFirst("OrganizationId")?.Value;

                if (Guid.TryParse(orgClaim, out var organizationId))
                {
                    return BadRequest();
                }

                var response = await _refreshUseCase.Execute(request, organizationId);
                return Ok(response);
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("Invalid or expired refresh token"))
            {
                return Unauthorized(
                    new
                    {
                        correlationId = GetCorrelationId(),
                        errorCode = "INVALID_REFRESH",
                        message = ex.Message
                    });
            }
            catch (Exception)
            {
                return StatusCode(
                    500,
                    new
                    {
                        correlationId = GetCorrelationId(),
                        errorCode = "INTERNAL_ERROR",
                        message = "An unexpected error occurred"
                    });
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] LogoutRequest request)
        {
            try
            {
                await _logoutUseCase.Execute(request);
                return NoContent();
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("Invalid refresh token"))
            {

                return BadRequest(
                    new
                    {
                        correlationId = GetCorrelationId(),
                        errorCode = "INVALID_TOKEN",
                        message = ex.Message
                    });
            }
            catch (Exception)
            {
                return StatusCode(
                    500,
                    new
                    {
                        correlationId = GetCorrelationId(),
                        errorCode = "INTERNAL_ERROR",
                        message = "An unexpected error occurred"
                    });
            }
        }

        private string GetCorrelationId() =>
            ExceptionHandlingMiddleware.GetCorrelationId(HttpContext);
    }
}
