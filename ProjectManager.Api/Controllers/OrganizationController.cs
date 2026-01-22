using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ProjectManager.Api.Controllers
{
    [ApiController]
    [Route("organizations")]
    [Authorize]
    public class OrganizationController : ControllerBase
    {
        //Methot to test organization endpoint
        [HttpGet("{orgId}")]
        public async Task<IActionResult> GetOrganization(Guid orgId)
        {
            var orgClaim = User.FindFirst("OrganizationId")?.Value;

            if (Guid.TryParse(orgClaim, out var organizationId) && organizationId != orgId)
            {
                return BadRequest();
            }
            return Ok(organizationId);
        }
    }
}
