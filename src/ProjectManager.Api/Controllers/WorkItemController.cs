using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.Api.Middlewares;
using ProjectManager.Application.DTOs.WorkItems;
using ProjectManager.Application.UseCases.WorkItems.Create;
using ProjectManager.Application.UseCases.WorkItems.List;
using ProjectManager.Application.UseCases.WorkItems.Update;
using System.Security.Claims;

namespace ProjectManager.Api.Controllers
{
    [ApiController]
    [Route("organizations/{orgId:guid}/projects/{projectId:guid}/workitems")]
    [Authorize(Policy = "ProjectMember")]
    public sealed class WorkItemController(
        IListWorkItemsUseCase listWorkItemsUseCase, 
        ICreateWorkItemUseCase createWorkItemUseCase, 
        IUpdateWorkItemStatusUseCase updateWorkItemStatusUseCase
    ) : ControllerBase
    {
        private readonly IListWorkItemsUseCase _listWorkItemsUseCase = listWorkItemsUseCase;
        private readonly ICreateWorkItemUseCase _createWorkItemUseCase = createWorkItemUseCase;
        private readonly IUpdateWorkItemStatusUseCase _updateWorkItemStatusUseCase = updateWorkItemStatusUseCase;

        [HttpGet]
        public async Task<IActionResult> List([FromRoute] Guid projectId, [FromQuery] WorkItemFilter filter)
        {
            var workItems = await _listWorkItemsUseCase.Execute(projectId, filter);
            return Ok(workItems);
        }

        [HttpPost]
        [Authorize(Policy = "ProjectManager")]
        public async Task<IActionResult> Create([FromRoute] Guid orgId, [FromRoute] Guid projectId, [FromBody] CreateWorkItemRequest request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userIdClaim, out var userId))
                return Unauthorized(
                    new 
                    {
                        correlationId = GetCorrelationId(), 
                        errorCode = "INVALID_TOKEN", 
                        message = "Invalid token."
                    });

            var workItemId = await _createWorkItemUseCase.Execute(projectId, request, userId);

            return CreatedAtAction(nameof(Create), new { orgId, projectId, id = workItemId }, new { id = workItemId });
        }

        [HttpPatch("{workItemId:guid}/status")]
        [Authorize(Policy = "ProjectManager")]
        public async Task<IActionResult> UpdateStatus([FromRoute] Guid workItemId, [FromBody] UpdateWorkItemStatusRequest request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userIdClaim, out var userId))
                return Unauthorized(
                    new 
                    {
                        correlationId = GetCorrelationId(), 
                        errorCode = "INVALID_TOKEN", 
                        message = "Invalid token."
                    });

            await _updateWorkItemStatusUseCase.Execute(workItemId, request, userId);

            return NoContent();
        }

        private string GetCorrelationId() =>
            ExceptionHandlingMiddleware.GetCorrelationId(HttpContext);
    }
}
