using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ProjectManager.Api.Controllers
{
    [Controller]
    [Route("organizations")]
    public class OrganizationController : ControllerBase
    {
        //Methot to test organization endpoint
        [HttpGet("{id}")]
        [Authorize(Policy = "OrganizationMember")]
        public async Task<IActionResult> GetOrganization(Guid id)
        {
            var userOrgClaim = User.FindFirst("org")?.Value;

            if (Guid.TryParse(userOrgClaim, out var userOrgId) && userOrgId != id)
            {
                return BadRequest();
            }
            return Ok(userOrgClaim);
        }
    }
}
