using ProjectManager.Application.DTOs.Sprints;
using ProjectManager.Application.Exceptions;
using ProjectManager.Application.Ports;

namespace ProjectManager.Application.UseCases.Sprints.ListWorkItems
{
    public sealed class ListSprintWorkItemsUseCase(
        ISprintRepository sprintRepository,
        IProjectRepository projectRepository
    ) : IListSprintWorkItemsUseCase
    {
        private readonly ISprintRepository _sprintRepository = sprintRepository;
        private readonly IProjectRepository _projectRepository = projectRepository;

        public async Task<IEnumerable<ListSprintWorkItemsResponse>> Execute(Guid projectId, Guid sprintId, CancellationToken ct = default)
        {
            _ = await _projectRepository.GetByIdAsync(projectId, ct)
                ?? throw new NotFoundException("Project not found", "Project", projectId);

            var sprint = await _sprintRepository.GetByIdAsync(sprintId, ct)
                ?? throw new NotFoundException("Sprint not found", "Sprint", sprintId);

            if (sprint.ProjectId != projectId)
                throw new ForbiddenException("Sprint does not belong to the specified project.");

            return sprint.WorkItems
                .Where(swi => swi.DeletedAt == null)
                .OrderBy(swi => swi.OrderIndex)
                .Select(swi => new ListSprintWorkItemsResponse
                {
                    WorkItemId = swi.WorkItemId,
                    WorkItemTitle = swi.WorkItem.Title,
                    WorkItemType = swi.WorkItem.Type.ToString(),
                    Status = swi.WorkItem.Status.ToString(),
                    StoryPoints = swi.WorkItem.StoryPoints,
                    OrderIndex = swi.OrderIndex
                })
                .ToList();
        }
    }
}

