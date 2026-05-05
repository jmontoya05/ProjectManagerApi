using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.Application.DTOs.Roles;
using ProjectManager.Application.UseCases.Roles.Create;
using ProjectManager.Application.UseCases.Roles.Delete;
using ProjectManager.Application.UseCases.Roles.GetAll;
using ProjectManager.Application.UseCases.Roles.GetAllByOrganization;
using ProjectManager.Application.UseCases.Roles.GetById;
using ProjectManager.Application.UseCases.Roles.Update;

namespace ProjectManager.Api.Controllers
{
    [ApiController]
    [Route("roles")]
    [Authorize(Policy = "OrgAdmin")]
    public class RolesController(
        IGetAllRolesUseCase getAllRolesUseCase,
        IGetAllRolesByOrganizationUseCase getAllRolesByOrganizationUseCase,
        IGetRoleByIdUseCase getRoleByIdUseCase,
        ICreateRoleUseCase createRoleUseCase,
        IUpdateRoleUseCase updateRoleUseCase,
        IDeleteRoleUseCase deleteRoleUseCase
    ) : ControllerBase
    {
        private readonly IGetAllRolesUseCase _getAllRolesUseCase = getAllRolesUseCase;
        private readonly IGetAllRolesByOrganizationUseCase _getAllRolesByOrganizationUseCase = getAllRolesByOrganizationUseCase;
        private readonly IGetRoleByIdUseCase _getRoleByIdUseCase = getRoleByIdUseCase;
        private readonly ICreateRoleUseCase _createRoleUseCase = createRoleUseCase;
        private readonly IUpdateRoleUseCase _updateRoleUseCase = updateRoleUseCase;
        private readonly IDeleteRoleUseCase _deleteRoleUseCase = deleteRoleUseCase;

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken ct)
            => Ok(await _getAllRolesUseCase.Execute(ct));

        [HttpGet("{roleId:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid roleId, CancellationToken ct)
        {
            var role = await _getRoleByIdUseCase.Execute(roleId, ct);
            return role == null ? NotFound() : Ok(role);
        }

        [HttpGet("by-organization/{orgId:guid}")]
        public async Task<IActionResult> GetAllByOrganization([FromRoute] Guid orgId, CancellationToken ct)
            => Ok(await _getAllRolesByOrganizationUseCase.Execute(orgId, ct));

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRoleRequest request, CancellationToken ct)
        {
            var id = await _createRoleUseCase.Execute(request, ct);
            return CreatedAtAction(nameof(GetById), new { roleId = id }, new { id });
        }

        [HttpPut("{roleId:guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid roleId, [FromBody] UpdateRoleRequest request, CancellationToken ct)
        {
            await _updateRoleUseCase.Execute(roleId, request, ct);
            return NoContent();
        }

        [HttpDelete("{roleId:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid roleId, CancellationToken ct)
        {
            await _deleteRoleUseCase.Execute(roleId, ct);
            return NoContent();
        }
    }
}
