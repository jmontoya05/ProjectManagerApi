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

        [HttpGet("{prmissionId:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid prmissionId, CancellationToken ct)
        {
            var permission = await permissionService.GetByIdAsync(prmissionId, ct);
            return permission == null ? NotFound() : Ok(permission);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePermissionRequest request, CancellationToken ct)
        {
            var id = await permissionService.CreateAsync(request, ct);
            return CreatedAtAction(nameof(GetById), new { id }, new { id });
        }

        [HttpPut("{prmissionId:guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid prmissionId, [FromBody] UpdatePermissionRequest request, CancellationToken ct)
        {
            await permissionService.UpdateAsync(prmissionId, request, ct);
            return NoContent();
        }

        [HttpDelete("{prmissionId:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid prmissionId, CancellationToken ct)
        {
            await permissionService.DeleteAsync(prmissionId, ct);
            return NoContent();
        }
    }
}
