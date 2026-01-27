using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.Api.Middlewares;
using ProjectManager.Application.UseCases.Projects.Create;
using System.Security.Claims;

namespace ProjectManager.Api.Controllers
{
    [ApiController]
    [Route("org/{orgId}/projects")]
    [Authorize]
    public sealed class ProjectController(ICreateProjectUseCase createProjectUseCase) : ControllerBase
    {
        private readonly ICreateProjectUseCase _createProjectUseCase = createProjectUseCase;

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

        private string GetCorrelationId() =>
                ExceptionHandlingMiddleware.GetCorrelationId(HttpContext);
    }
}
