using ProjectManager.Application.DTOs.Sprints;
using ProjectManager.Application.Exceptions;
using ProjectManager.Application.Ports;

namespace ProjectManager.Application.UseCases.Sprints.ReorderWorkItems
{
    public sealed class ReorderSprintWorkItemsUseCase(
        ISprintRepository sprintRepository,
        ISprintWorkItemRepository sprintWorkItemRepository,
        IProjectRepository projectRepository
    ) : IReorderSprintWorkItemsUseCase
    {
        private readonly ISprintRepository _sprintRepository = sprintRepository;
        private readonly ISprintWorkItemRepository _sprintWorkItemRepository = sprintWorkItemRepository;
        private readonly IProjectRepository _projectRepository = projectRepository;

        public async Task Execute(Guid projectId, Guid sprintId, ReorderSprintWorkItemsRequest request, CancellationToken ct = default)
        {
            _ = await _projectRepository.GetByIdAsync(projectId, ct)
                ?? throw new NotFoundException("Project not found", "Project", projectId);

            var sprint = await _sprintRepository.GetByIdAsync(sprintId, ct)
                ?? throw new NotFoundException("Sprint not found", "Sprint", sprintId);

            if (sprint.ProjectId != projectId)
                throw new ForbiddenException("Sprint does not belong to the specified project.");

            var sprintWorkItems = sprint.WorkItems
                .Where(swi => swi.DeletedAt == null)
                .ToList();
            
            foreach (var workItemId in request.WorkItemIds.Where(workItemId => sprintWorkItems.All(swi => swi.WorkItemId != workItemId)))
            {
                throw new BusinessRuleException($"Work item {workItemId} is not assigned to this sprint.", "WORK_ITEM_NOT_IN_SPRINT");
            }
            
            for (var i = 0; i < request.WorkItemIds.Count; i++)
            {
                var sprintWorkItem = sprintWorkItems.First(swi => swi.WorkItemId == request.WorkItemIds[i]);
                sprintWorkItem.OrderIndex = i + 1;
                await _sprintWorkItemRepository.UpdateAsync(sprintWorkItem, ct);
            }
        }
    }
}

