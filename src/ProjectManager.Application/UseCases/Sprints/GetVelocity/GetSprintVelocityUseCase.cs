using ProjectManager.Application.DTOs.Sprints;
using ProjectManager.Application.Exceptions;
using ProjectManager.Application.Ports;
using ProjectManager.Domain.Enums;

namespace ProjectManager.Application.UseCases.Sprints.GetVelocity
{
    public sealed class GetSprintVelocityUseCase(
        ISprintRepository sprintRepository,
        IProjectRepository projectRepository
    ) : IGetSprintVelocityUseCase
    {
        private readonly ISprintRepository _sprintRepository = sprintRepository;
        private readonly IProjectRepository _projectRepository = projectRepository;

        public async Task<SprintVelocityResponse> Execute(Guid projectId, Guid sprintId, CancellationToken ct = default)
        {
            _ = await _projectRepository.GetByIdAsync(projectId, ct)
                ?? throw new NotFoundException("Project not found", "Project", projectId);

            var sprint = await _sprintRepository.GetByIdAsync(sprintId, ct)
                ?? throw new NotFoundException("Sprint not found", "Sprint", sprintId);

            if (sprint.ProjectId != projectId)
                throw new ForbiddenException("Sprint does not belong to the specified project.");

            var velocity = sprint.CalculateVelocity();
            var currentLoad = sprint.CalculateCurrentCapacity();
            var capacity = sprint.Capacity ?? 0;
            var activeWorkItems = sprint.GetActiveWorkItemCount();
            var completedWorkItems = sprint.WorkItems
                .Count(swi => swi.DeletedAt == null && swi.WorkItem.Status == WorkItemStatus.Done);

            return new SprintVelocityResponse
            {
                SprintId = sprintId,
                Velocity = velocity,
                Capacity = capacity,
                CurrentLoad = currentLoad,
                RemainingCapacity = capacity > 0 ? Math.Max(0, capacity - currentLoad) : 0,
                CompletedWorkItems = completedWorkItems,
                TotalWorkItems = activeWorkItems + completedWorkItems
            };
        }
    }
}

