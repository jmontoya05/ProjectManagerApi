using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.Api.Middlewares;
using ProjectManager.Application.UseCases.Users.GetProfile;
using System.Security.Claims;

namespace ProjectManager.Api.Controllers
{
    [ApiController]
    [Route("users")]
    [Authorize(Policy = "OrgMember")]
    public sealed class UserController(
        IGetProfileUseCase getProfileUseCase
    ) : ControllerBase
    {
        private readonly IGetProfileUseCase _getProfileUseCase = getProfileUseCase;
        
        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetMe()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!Guid.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(
                    new
                    {
                        correlationId = ExceptionHandlingMiddleware.GetCorrelationId(HttpContext),
                        errorCode = "INVALID_TOKEN",
                        message = "Invalid token."
                    });
            }

            var profile = await _getProfileUseCase.Execute(userId);

            return Ok(profile);
        }
    }
}
