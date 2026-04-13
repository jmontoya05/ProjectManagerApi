using Microsoft.AspNetCore.Mvc;
using ProjectManager.Api.Middlewares;
using ProjectManager.Application.DTOs.WorkItems;
using ProjectManager.Application.DTOs.WorkItems.Request;
using ProjectManager.Application.UseCases.WorkItems.Create;
using ProjectManager.Application.UseCases.WorkItems.List;
using ProjectManager.Application.UseCases.WorkItems.Update;
using System.Security.Claims;

namespace ProjectManager.Api.Controllers
{
    [ApiController]
    [Route("organizations/{orgId}/projects/{projectId}/workitems")]
    //[Authorize(Policy = "ProjectMember")]
    public sealed class WorkItemController(IListWorkItemsUseCase listWorkItemsUseCase, ICreateWorkItemUseCase createWorkItemUseCase, IUpdateWorkItemStatusUseCase updateWorkItemStatusUseCase) : ControllerBase
    {
        private readonly IListWorkItemsUseCase _listWorkItemsUseCase = listWorkItemsUseCase;
        private readonly ICreateWorkItemUseCase _createWorkItemUseCase = createWorkItemUseCase;
        private readonly IUpdateWorkItemStatusUseCase _updateWorkItemStatusUseCase = updateWorkItemStatusUseCase;

        [HttpGet]
        public async Task<IActionResult> List([FromRoute] Guid projectId, [FromQuery] WorkItemFilter filter)
        {
            try
            {
                var workItems = await _listWorkItemsUseCase.Execute(projectId, filter);
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

        [HttpPost]
        public async Task<IActionResult> Create([FromRoute] Guid orgId, [FromRoute] Guid projectId, [FromBody] CreateWorkItemRequest request)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!Guid.TryParse(userIdClaim, out var userId))
                    return Unauthorized(new { correlationId = GetCorrelationId(), errorCode = "INVALID_TOKEN", message = "Invalid token." });

                var workItemId = await _createWorkItemUseCase.Execute(projectId, request, userId);

                return CreatedAtAction(nameof(Create), new { orgId, projectId, id = workItemId }, new { id = workItemId });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { correlationId = GetCorrelationId(), errorCode = "BAD_REQUEST", message = ex.Message });
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

        [HttpPatch("{workItemId}/status")]
        public async Task<IActionResult> UpdateStatus([FromRoute] Guid workItemId, [FromBody] UpdateWorkItemStatusRequest request)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!Guid.TryParse(userIdClaim, out var userId))
                    return Unauthorized(new { correlationId = GetCorrelationId(), errorCode = "INVALID_TOKEN", message = "Invalid token." });

                await _updateWorkItemStatusUseCase.Execute(workItemId, request, userId);

                return NoContent();
            }
            catch (InvalidOperationException ex)
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
