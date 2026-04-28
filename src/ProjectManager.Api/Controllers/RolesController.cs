using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.Application.DTOs.Roles;
using ProjectManager.Application.UseCases.Roles;

namespace ProjectManager.Api.Controllers
{
    [ApiController]
    [Route("roles")]
    [Authorize(Policy = "OrgAdmin")]
    public class RolesController(IRoleService roleService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken ct)
            => Ok(await roleService.GetAllAsync(ct));

        [HttpGet("{roleId:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid roleId, CancellationToken ct)
        {
            var role = await roleService.GetByIdAsync(roleId, ct);
            return role == null ? NotFound() : Ok(role);
        }

        [HttpGet("by-organization/{orgId:guid}")]
        public async Task<IActionResult> GetAllByOrganization([FromRoute] Guid orgId, CancellationToken ct)
            => Ok(await roleService.GetAllByOrganizationAsync(orgId, ct));

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRoleRequest request, CancellationToken ct)
        {
            var id = await roleService.CreateAsync(request, ct);
            return CreatedAtAction(nameof(GetById), new { id }, new { id });
        }

        [HttpPut("{roleId:guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid roleId, [FromBody] UpdateRoleRequest request, CancellationToken ct)
        {
            await roleService.UpdateAsync(roleId, request, ct);
            return NoContent();
        }

        [HttpDelete("{roleId:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid roleId, CancellationToken ct)
        {
            await roleService.DeleteAsync(roleId, ct);
            return NoContent();
        }
    }
}
