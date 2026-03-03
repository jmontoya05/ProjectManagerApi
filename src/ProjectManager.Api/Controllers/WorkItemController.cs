using Microsoft.AspNetCore.Mvc;
using ProjectManager.Api.Middlewares;
using ProjectManager.Application.DTOs.WorkItems;
using ProjectManager.Application.UseCases.WorkItems.List;

namespace ProjectManager.Api.Controllers
{
    [ApiController]
    [Route("organizations/{orgId}/projects/{projectId}/workitems")]
    //[Authorize(Policy = "ProjectMember")]
    public sealed class WorkItemController(IListWorkItemsUseCase listWorkItemsUseCase) : ControllerBase
    {
        private readonly IListWorkItemsUseCase listWorkItemsUseCase = listWorkItemsUseCase;

        [HttpGet]
        public async Task<IActionResult> List([FromRoute] Guid projectId, [FromQuery] WorkItemFilter filter)
        {
            try
            {
                var workItems = await listWorkItemsUseCase.Execute(projectId, filter);
                return Ok(workItems);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { correlationId = GetCorrelationId(), errorCode = "NOT_FOUND", message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { correlationId = GetCorrelationId(), errorCode = "BAD_REQUEST", message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { correlationId = GetCorrelationId(), errorCode = "INTERNAL_ERROR", message = "An unexpected error occurred." });
            }
        }

        private string GetCorrelationId() =>
            ExceptionHandlingMiddleware.GetCorrelationId(HttpContext);
    }
}
