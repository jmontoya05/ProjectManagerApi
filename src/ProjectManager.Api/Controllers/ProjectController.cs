using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.Application.DTOs.Projects;
using ProjectManager.Application.UseCases.Projects.Create;
using ProjectManager.Application.UseCases.Projects.Get;
using ProjectManager.Application.UseCases.Projects.List;
using ProjectManager.Application.UseCases.Projects.Update;

namespace ProjectManager.Api.Controllers
{
    [ApiController]
    [Route("org/{orgId:guid}/projects")]
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
        public async Task<IActionResult> Create([FromRoute] Guid orgId, [FromBody] CreateProjectRequest request, CancellationToken ct)
        {
            var projectId = await _createProjectUseCase.Execute(request, ct);
            return CreatedAtAction(nameof(Create), new { id = projectId }, new { id = projectId });
        }

        [HttpGet]
        [Authorize(Policy = "ProjectMember")]
        public async Task<IActionResult> List([FromRoute] Guid orgId, CancellationToken ct)
        {
            var response = await _listProjectsUseCase.Execute(ct);
            return Ok(response);
        }

        [HttpGet("{projectId:guid}")]
        [Authorize(Policy = "ProjectMember")]
        public async Task<IActionResult> GetById([FromRoute] Guid orgId, [FromRoute] Guid projectId, CancellationToken ct)
        {
            var project = await _getProjectByIdUseCase.Execute(projectId, ct);
            return Ok(project);
        }

        [HttpPut("{projectId:guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid orgId, [FromRoute] Guid projectId, [FromBody] UpdateProjectRequest request, CancellationToken ct)
        {
            await _updateProjectUseCase.Execute(request, projectId, ct);
            return NoContent();
        }
    }
}
