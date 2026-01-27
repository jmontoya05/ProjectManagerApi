using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.Api.Middlewares;
using ProjectManager.Application.UseCases.Projects.Create;
using ProjectManager.Application.UseCases.Projects.List;
using System.Security.Claims;

namespace ProjectManager.Api.Controllers
{
    [ApiController]
    [Route("org/{orgId}/projects")]
    [Authorize]
    public sealed class ProjectController(ICreateProjectUseCase createProjectUseCase, IListProjectsUseCase listProjectsUseCase) : ControllerBase
    {
        private readonly ICreateProjectUseCase _createProjectUseCase = createProjectUseCase;
        private readonly IListProjectsUseCase listProjectsUseCase = listProjectsUseCase;

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
        public async Task<IActionResult> Get()
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

            var response = await listProjectsUseCase.Execute(organizationId);

            return Ok(response);
        }

        private string GetCorrelationId() =>
                ExceptionHandlingMiddleware.GetCorrelationId(HttpContext);
    }
}
