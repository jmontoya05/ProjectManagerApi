using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.Application.DTOs.Sprints;
using ProjectManager.Application.UseCases.Sprints.AddWorkItem;
using ProjectManager.Application.UseCases.Sprints.Complete;
using ProjectManager.Application.UseCases.Sprints.Create;
using ProjectManager.Application.UseCases.Sprints.Delete;
using ProjectManager.Application.UseCases.Sprints.GetById;
using ProjectManager.Application.UseCases.Sprints.GetVelocity;
using ProjectManager.Application.UseCases.Sprints.ListByProject;
using ProjectManager.Application.UseCases.Sprints.ListWorkItems;
using ProjectManager.Application.UseCases.Sprints.RemoveWorkItem;
using ProjectManager.Application.UseCases.Sprints.ReorderWorkItems;
using ProjectManager.Application.UseCases.Sprints.Start;
using ProjectManager.Application.UseCases.Sprints.Update;

namespace ProjectManager.Api.Controllers
{
    [ApiController]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Route("projects/{projectId:guid}/sprints")]
    public class SprintsController(
        ICreateSprintUseCase createSprintUseCase,
        IUpdateSprintUseCase updateSprintUseCase,
        IGetSprintByIdUseCase getSprintByIdUseCase,
        IListSprintsByProjectUseCase listSprintsByProjectUseCase,
        IDeleteSprintUseCase deleteSprintUseCase,
        IStartSprintUseCase startSprintUseCase,
        ICompleteSprintUseCase completeSprintUseCase,
        IAddWorkItemToSprintUseCase addWorkItemToSprintUseCase,
        IRemoveWorkItemFromSprintUseCase removeWorkItemFromSprintUseCase,
        IListSprintWorkItemsUseCase listSprintWorkItemsUseCase,
        IReorderSprintWorkItemsUseCase reorderSprintWorkItemsUseCase,
        IGetSprintVelocityUseCase getSprintVelocityUseCase
    ) : ControllerBase
    {
        private readonly ICreateSprintUseCase _createSprintUseCase = createSprintUseCase;
        private readonly IUpdateSprintUseCase _updateSprintUseCase = updateSprintUseCase;
        private readonly IGetSprintByIdUseCase _getSprintByIdUseCase = getSprintByIdUseCase;
        private readonly IListSprintsByProjectUseCase _listSprintsByProjectUseCase = listSprintsByProjectUseCase;
        private readonly IDeleteSprintUseCase _deleteSprintUseCase = deleteSprintUseCase;
        private readonly IStartSprintUseCase _startSprintUseCase = startSprintUseCase;
        private readonly ICompleteSprintUseCase _completeSprintUseCase = completeSprintUseCase;
        private readonly IAddWorkItemToSprintUseCase _addWorkItemToSprintUseCase = addWorkItemToSprintUseCase;
        private readonly IRemoveWorkItemFromSprintUseCase _removeWorkItemFromSprintUseCase = removeWorkItemFromSprintUseCase;
        private readonly IListSprintWorkItemsUseCase _listSprintWorkItemsUseCase = listSprintWorkItemsUseCase;
        private readonly IReorderSprintWorkItemsUseCase _reorderSprintWorkItemsUseCase = reorderSprintWorkItemsUseCase;
        private readonly IGetSprintVelocityUseCase _getSprintVelocityUseCase = getSprintVelocityUseCase;
        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CreateSprint(
            Guid projectId,
            [FromBody] CreateSprintRequest request,
            CancellationToken ct = default
        )
        {
            var sprintId = await _createSprintUseCase.Execute(projectId, request, ct);
            return CreatedAtAction(nameof(GetSprint), new { projectId, sprintId }, sprintId);
        }
        
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ListSprints(Guid projectId, CancellationToken ct = default)
        {
            var sprints = await _listSprintsByProjectUseCase.Execute(projectId, ct);
            return Ok(sprints);
        }
        
        [HttpGet("{sprintId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSprint(Guid projectId, Guid sprintId, CancellationToken ct = default)
        {
            var sprint = await _getSprintByIdUseCase.Execute(projectId, sprintId, ct);
            return Ok(sprint);
        }
        
        [HttpPut("{sprintId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateSprint(
            Guid projectId,
            Guid sprintId,
            [FromBody] UpdateSprintRequest request,
            CancellationToken ct = default
        )
        {
            await _updateSprintUseCase.Execute(projectId, sprintId, request, ct);
            var sprint = await _getSprintByIdUseCase.Execute(projectId, sprintId, ct);
            return Ok(sprint);
        }
        
        [HttpDelete("{sprintId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteSprint(Guid projectId, Guid sprintId, CancellationToken ct = default)
        {
            await _deleteSprintUseCase.Execute(projectId, sprintId, ct);
            return NoContent();
        }
        
        [HttpPost("{sprintId:guid}/start")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> StartSprint(Guid projectId, Guid sprintId, CancellationToken ct = default)
        {
            await _startSprintUseCase.Execute(projectId, sprintId, ct);
            var sprint = await _getSprintByIdUseCase.Execute(projectId, sprintId, ct);
            return Ok(sprint);
        }
        
        [HttpPost("{sprintId:guid}/complete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CompleteSprint(Guid projectId, Guid sprintId, CancellationToken ct = default)
        {
            await _completeSprintUseCase.Execute(projectId, sprintId, ct);
            var sprint = await _getSprintByIdUseCase.Execute(projectId, sprintId, ct);
            return Ok(sprint);
        }
        
        [HttpPost("{sprintId:guid}/workitems/{workItemId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddWorkItemToSprint(
            Guid projectId,
            Guid sprintId,
            Guid workItemId,
            [FromBody] AddWorkItemToSprintRequest request,
            CancellationToken ct = default
        )
        {
            await _addWorkItemToSprintUseCase.Execute(projectId, sprintId, request, ct);
            return Ok();
        }
        
        [HttpDelete("{sprintId:guid}/workitems/{workItemId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RemoveWorkItemFromSprint(
            Guid projectId,
            Guid sprintId,
            Guid workItemId,
            CancellationToken ct = default
        )
        {
            await _removeWorkItemFromSprintUseCase.Execute(projectId, sprintId, workItemId, ct);
            return NoContent();
        }
        
        [HttpGet("{sprintId:guid}/workitems")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ListSprintWorkItems(Guid projectId, Guid sprintId, CancellationToken ct = default)
        {
            var workItems = await _listSprintWorkItemsUseCase.Execute(projectId, sprintId, ct);
            return Ok(workItems);
        }
        
        [HttpPost("{sprintId:guid}/workitems/reorder")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ReorderSprintWorkItems(
            Guid projectId,
            Guid sprintId,
            [FromBody] ReorderSprintWorkItemsRequest request,
            CancellationToken ct = default
        )
        {
            await _reorderSprintWorkItemsUseCase.Execute(projectId, sprintId, request, ct);
            return Ok();
        }
        
        [HttpGet("{sprintId:guid}/velocity")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSprintVelocity(Guid projectId, Guid sprintId, CancellationToken ct = default)
        {
            var velocity = await _getSprintVelocityUseCase.Execute(projectId, sprintId, ct);
            return Ok(velocity);
        }
    }
}




