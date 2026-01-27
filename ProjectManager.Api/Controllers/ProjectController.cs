using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.Api.Middlewares;
using ProjectManager.Application.UseCases.Projects.Create;
using ProjectManager.Application.UseCases.Projects.Get;
using ProjectManager.Application.UseCases.Projects.List;
using System.Security.Claims;

namespace ProjectManager.Api.Controllers
{
    [ApiController]
    [Route("org/{orgId}/projects")]
    [Authorize]
    public sealed class ProjectController(ICreateProjectUseCase createProjectUseCase, IListProjectsUseCase listProjectsUseCase, IGetProjectByIdUseCase getProjectByIdUseCase) : ControllerBase
    {
        private readonly ICreateProjectUseCase _createProjectUseCase = createProjectUseCase;
        private readonly IListProjectsUseCase _listProjectsUseCase = listProjectsUseCase;
        private readonly IGetProjectByIdUseCase _getProjectByIdUseCase = getProjectByIdUseCase;

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProjectRequest request, [FromRoute] Guid orgId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!Guid.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(
                    new
                    {
                        correlationId = GetCorrelationId(),
                        errorCode = "INVALID_TOKEN",
                        message = "Invalid token."
                    });
            }

            var projectId = await _createProjectUseCase.Execute(request, orgId, userId);

            return CreatedAtAction(nameof(Create), new { id = projectId }, new { id = projectId });
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var orgIdClaim = User.FindFirst("OrganizationId")?.Value;

            if (!Guid.TryParse(orgIdClaim, out var organizationId))
            {
                return Unauthorized(
                    new
                    {
                        correlationId = GetCorrelationId(),
                        errorCode = "INVALID_TOKEN",
                        message = "Invalid token."
                    });
            }

            var response = await _listProjectsUseCase.Execute(organizationId);

            return Ok(response);
        }

        [HttpGet("{projectId}")]
        public async Task<IActionResult> GetById([FromRoute] Guid projectId)
        {
            try
            {
                var project = await _getProjectByIdUseCase.Execute(projectId);
                return Ok(project);
            }
            catch (InvalidOperationException ex)
            {

                return NotFound(
                    new
                    {
                        correlationId = GetCorrelationId(),
                        errorCode = "PROJECT_NOT_FOUND",
                        message = ex.Message
                    });
            }
            catch (Exception)
            {
                return StatusCode(
                    500,
                    new
                    {
                        correlationId = GetCorrelationId(),
                        errorCode = "INTERNAL_ERROR",
                        message = "An unexpected error occurred"
                    });
            }
        }

        private string GetCorrelationId() =>
                ExceptionHandlingMiddleware.GetCorrelationId(HttpContext);
    }
}
