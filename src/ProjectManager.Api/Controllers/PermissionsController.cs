using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.Application.DTOs.Permissions;
using ProjectManager.Application.UseCases.Permissions.Create;
using ProjectManager.Application.UseCases.Permissions.Delete;
using ProjectManager.Application.UseCases.Permissions.GetAll;
using ProjectManager.Application.UseCases.Permissions.GetById;
using ProjectManager.Application.UseCases.Permissions.Update;

namespace ProjectManager.Api.Controllers
{
    [ApiController]
    [Route("permissions")]
    [Authorize(Policy = "OrgAdmin")]
    public class PermissionsController(
        ICreatePermissionUseCase createPermissionUseCase,
        IDeletePermissionUseCase deletePermissionUseCase,
        IGetAllPermissionsUseCase getAllPermissionsUseCase,
        IGetPermissionByIdUseCase getPermissionByIdUseCase,
        IUpdatePermissionUseCase updatePermissionUseCase
    ) : ControllerBase
    {
        private readonly ICreatePermissionUseCase _createPermissionUseCase = createPermissionUseCase;
        private readonly IDeletePermissionUseCase _deletePermissionUseCase = deletePermissionUseCase;
        private readonly IGetAllPermissionsUseCase _getAllPermissionsUseCase = getAllPermissionsUseCase;
        private readonly IGetPermissionByIdUseCase _getPermissionByIdUseCase = getPermissionByIdUseCase;
        private readonly IUpdatePermissionUseCase _updatePermissionUseCase = updatePermissionUseCase;
        
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken ct)
            => Ok(await _getAllPermissionsUseCase.Execute(ct));

        [HttpGet("{permissionId:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid permissionId, CancellationToken ct)
        {
            var permission = await _getPermissionByIdUseCase.Execute(permissionId, ct);
            return permission == null ? NotFound() : Ok(permission);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePermissionRequest request, CancellationToken ct)
        {
            var id = await _createPermissionUseCase.Execute(request, ct);
            return CreatedAtAction(nameof(GetById), new { permissionId = id }, new { id });
        }

        [HttpPut("{permissionId:guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid permissionId, [FromBody] UpdatePermissionRequest request, CancellationToken ct)
        {
            await _updatePermissionUseCase.Execute(permissionId, request, ct);
            return NoContent();
        }

        [HttpDelete("{permissionId:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid permissionId, CancellationToken ct)
        {
            await _deletePermissionUseCase.Execute(permissionId, ct);
            return NoContent();
        }
    }
}
