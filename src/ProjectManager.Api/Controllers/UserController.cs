using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.Api.Middlewares;
using ProjectManager.Application.UseCases.Users.GetProfile;
using System.Security.Claims;

namespace ProjectManager.Api.Controllers
{
    [ApiController]
    [Route("users")]
    [Authorize]
    public sealed class UserController(IGetProfileUseCase getProfileUseCase) : ControllerBase
    {
        private readonly IGetProfileUseCase _getProfileUseCase = getProfileUseCase;

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetMe()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!Guid.TryParse(userIdClaim, out var userId))
                {
                    return Unauthorized(
                        new
                        {
                            correlationId = GetCorrelationId(),
                            errorCode = "INVALID_TOKEN",
                            message = "Invalid token."
                        });
                }

                var profile = await _getProfileUseCase.Execute(userId);

                return Ok(profile);
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("User not found"))
            {

                return NotFound(
                    new
                    {
                        correlationId = GetCorrelationId(),
                        errorCode = "USER_NOT_FOUND",
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
