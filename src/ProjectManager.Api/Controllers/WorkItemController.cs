using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.Application.DTOs.WorkItems;
using ProjectManager.Application.UseCases.WorkItems.Create;
using ProjectManager.Application.UseCases.WorkItems.List;
using ProjectManager.Application.UseCases.WorkItems.Update;

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
        public async Task<IActionResult> List([FromRoute] Guid orgId, [FromRoute] Guid projectId, [FromQuery] WorkItemFilter filter, CancellationToken ct)
        {
            var workItems = await _listWorkItemsUseCase.Execute(projectId, filter, ct);
            return Ok(workItems);
        }

        [HttpPost]
        [Authorize(Policy = "ProjectManager")]
        public async Task<IActionResult> Create([FromRoute] Guid orgId, [FromRoute] Guid projectId, [FromBody] CreateWorkItemRequest request, CancellationToken ct)
        {
            var workItemId = await _createWorkItemUseCase.Execute(projectId, request, ct);
            return CreatedAtAction(nameof(Create), new { orgId, projectId, id = workItemId }, new { id = workItemId });
        }

        [HttpPatch("{workItemId:guid}/status")]
        [Authorize(Policy = "ProjectManager")]
        public async Task<IActionResult> UpdateStatus([FromRoute] Guid orgId, [FromRoute] Guid projectId,[FromRoute] Guid workItemId, [FromBody] UpdateWorkItemStatusRequest request, CancellationToken ct)
        {
            await _updateWorkItemStatusUseCase.Execute(workItemId, request, ct);
            return NoContent();
        }
    }
}
