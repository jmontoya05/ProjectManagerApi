using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.Application.UseCases.Users.GetProfile;

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
        public async Task<IActionResult> GetMe(CancellationToken ct)
        {
            var profile = await _getProfileUseCase.Execute(ct);
            return Ok(profile);
        }
    }
}
