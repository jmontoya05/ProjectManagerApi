using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.Application.DTOs.Permissions;
using ProjectManager.Application.UseCases.Permissions;

namespace ProjectManager.Api.Controllers
{
    [ApiController]
    [Route("permissions")]
    [Authorize(Policy = "OrgAdmin")]
    public class PermissionsController(
        IPermissionService permissionService
    ) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken ct)
            => Ok(await permissionService.GetAllAsync(ct));

        [HttpGet("{permissionId:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid permissionId, CancellationToken ct)
        {
            var permission = await permissionService.GetByIdAsync(permissionId, ct);
            return permission == null ? NotFound() : Ok(permission);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePermissionRequest request, CancellationToken ct)
        {
            var id = await permissionService.CreateAsync(request, ct);
            return CreatedAtAction(nameof(GetById), new { id }, new { id });
        }

        [HttpPut("{permissionId:guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid permissionId, [FromBody] UpdatePermissionRequest request, CancellationToken ct)
        {
            await permissionService.UpdateAsync(permissionId, request, ct);
            return NoContent();
        }

        [HttpDelete("{permissionId:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid permissionId, CancellationToken ct)
        {
            await permissionService.DeleteAsync(permissionId, ct);
            return NoContent();
        }
    }
}
