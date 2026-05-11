using ProjectManager.Application.DTOs.Sprints;
using ProjectManager.Application.Ports;
using ProjectManager.Application.Exceptions;
using ProjectManager.Domain.Exceptions;

namespace ProjectManager.Application.UseCases.Sprints.AddWorkItem
{
    public sealed class AddWorkItemToSprintUseCase(
        ISprintRepository sprintRepository,
        ISprintWorkItemRepository sprintWorkItemRepository,
        IWorkItemRepository workItemRepository,
        IProjectRepository projectRepository
    ) : IAddWorkItemToSprintUseCase
    {
        private readonly ISprintRepository _sprintRepository = sprintRepository;
        private readonly ISprintWorkItemRepository _sprintWorkItemRepository = sprintWorkItemRepository;
        private readonly IWorkItemRepository _workItemRepository = workItemRepository;
        private readonly IProjectRepository _projectRepository = projectRepository;

        public async Task Execute(Guid projectId, Guid sprintId, AddWorkItemToSprintRequest request, CancellationToken ct = default)
        {
            _ = await _projectRepository.GetByIdAsync(projectId, ct)
                ?? throw new NotFoundException("Project not found", "Project", projectId);

            var sprint = await _sprintRepository.GetByIdAsync(sprintId, ct)
                ?? throw new NotFoundException("Sprint not found", "Sprint", sprintId);

            if (sprint.ProjectId != projectId)
                throw new ForbiddenException("Sprint does not belong to the specified project.");

            var workItem = await _workItemRepository.GetByIdAsync(request.WorkItemId, ct)
                ?? throw new NotFoundException("Work item not found", "WorkItem", request.WorkItemId);

            if (workItem.ProjectId != projectId)
                throw new ForbiddenException("Work item does not belong to the specified project.");

            try
            {
                sprint.AddWorkItem(workItem);
            }
            catch (CannotAddWorkItemToCompletedSprintException ex)
            {
                throw new BusinessRuleException(ex.Message, ex.ErrorCode);
            }
            catch (SprintCapacityExceededException ex)
            {
                throw new BusinessRuleException(ex.Message, ex.ErrorCode);
            }
            
            var sprintWorkItem = sprint.WorkItems.Last();
            await _sprintWorkItemRepository.AddAsync(sprintWorkItem, ct);

            await _sprintRepository.UpdateAsync(sprint, ct);
        }
    }
}
