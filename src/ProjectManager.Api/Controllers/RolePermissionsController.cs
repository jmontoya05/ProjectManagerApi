using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.Application.UseCases.Roles;

namespace ProjectManager.Api.Controllers
{
    [ApiController]
    [Route("roles/{roleId:guid}/permissions")]
    [Authorize(Policy = "OrgAdmin")]
    public class RolePermissionsController(IRolePermissionService rolePermissionService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetPermissions(Guid roleId, CancellationToken ct)
            => Ok(await rolePermissionService.GetPermissionsByRoleAsync(roleId, ct));

        [HttpPost]
        public async Task<IActionResult> AddPermission(Guid roleId, [FromQuery] Guid permissionId, CancellationToken ct)
        {
            await rolePermissionService.AddPermissionToRoleAsync(roleId, permissionId, ct);
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> RemovePermission(Guid roleId, [FromQuery] Guid permissionId, CancellationToken ct)
        {
            await rolePermissionService.RemovePermissionFromRoleAsync(roleId, permissionId, ct);
            return NoContent();
        }
    }
}
