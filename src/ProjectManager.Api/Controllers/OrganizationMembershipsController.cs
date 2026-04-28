using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.Application.DTOs.Organizations;
using ProjectManager.Application.UseCases.Organizations;
using ProjectManager.Application.UseCases.Organizations.RoleAssigment;

namespace ProjectManager.Api.Controllers
{
    [ApiController]
    [Route("organizations/{orgId:guid}/memberships")]
    [Authorize(Policy = "OrgAdmin")]
    public class OrganizationMembershipsController(IOrganizationRoleAssignmentUseCase orgRoleUseCase) : ControllerBase
    {
        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignRole([FromRoute] Guid orgId, [FromBody] AssignOrganizationRoleRequest request, CancellationToken ct)
        {
            request.OrganizationId = orgId;
            await orgRoleUseCase.AssignRoleAsync(request, ct);
            return NoContent();
        }

        [HttpPost("remove-role")]
        public async Task<IActionResult> RemoveRole([FromRoute] Guid orgId, [FromBody] RemoveOrganizationRoleRequest request, CancellationToken ct)
        {
            request.OrganizationId = orgId;
            await orgRoleUseCase.RemoveRoleAsync(request, ct);
            return NoContent();
        }
    }
}
