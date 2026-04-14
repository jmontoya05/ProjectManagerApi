using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.Application.DTOs.Projects;
using ProjectManager.Application.Exceptions;
using ProjectManager.Application.UseCases.Projects.Create;
using ProjectManager.Application.UseCases.Projects.Get;
using ProjectManager.Application.UseCases.Projects.List;
using ProjectManager.Application.UseCases.Projects.Update;
using System.Security.Claims;

namespace ProjectManager.Api.Controllers
{
    [ApiController]
    [Route("org/{orgId}/projects")]
    [Authorize]
    public sealed class ProjectController(
        ICreateProjectUseCase createProjectUseCase,
        IListProjectsUseCase listProjectsUseCase,
        IGetProjectByIdUseCase getProjectByIdUseCase,
        IUpdateProjectUseCase updateProjectUseCase
        ) : ControllerBase
    {
        private readonly ICreateProjectUseCase _createProjectUseCase = createProjectUseCase;
        private readonly IListProjectsUseCase _listProjectsUseCase = listProjectsUseCase;
        private readonly IGetProjectByIdUseCase _getProjectByIdUseCase = getProjectByIdUseCase;
        private readonly IUpdateProjectUseCase _updateProjectUseCase = updateProjectUseCase;

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProjectRequest request, [FromRoute] Guid orgId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!Guid.TryParse(userIdClaim, out var userId))
                throw new UnauthorizedException("Invalid token");

            var projectId = await _createProjectUseCase.Execute(request, userId);
            return CreatedAtAction(nameof(Create), new { id = projectId }, new { id = projectId });
        }

        [HttpGet]
        public async Task<IActionResult> List([FromRoute] Guid orgId)
        {
            var response = await _listProjectsUseCase.Execute(orgId);
            return Ok(response);
        }

        [HttpGet("{projectId}")]
        public async Task<IActionResult> GetById([FromRoute] Guid orgId, [FromRoute] Guid projectId)
        {
            var project = await _getProjectByIdUseCase.Execute(projectId);
            return Ok(project);
        }

        [HttpPut("{projectId}")]
        public async Task<IActionResult> Update([FromBody] UpdateProjectRequest request, [FromRoute] Guid projectId, [FromRoute] Guid orgId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!Guid.TryParse(userIdClaim, out var userId))
                throw new UnauthorizedException("Invalid token");

            await _updateProjectUseCase.Execute(request, projectId, orgId, userId);
            return NoContent();
        }
    }
}
