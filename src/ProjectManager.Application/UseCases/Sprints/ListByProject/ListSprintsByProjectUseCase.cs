using ProjectManager.Application.DTOs.Sprints;
using ProjectManager.Application.Ports;
using ProjectManager.Application.Exceptions;

namespace ProjectManager.Application.UseCases.Sprints.ListByProject
{
    public sealed class ListSprintsByProjectUseCase(
        ISprintRepository sprintRepository,
        IProjectRepository projectRepository
    ) : IListSprintsByProjectUseCase
    {
        private readonly ISprintRepository _sprintRepository = sprintRepository;
        private readonly IProjectRepository _projectRepository = projectRepository;

        public async Task<IEnumerable<SprintResponse>> Execute(Guid projectId, CancellationToken ct = default)
        {
            _ = await _projectRepository.GetByIdAsync(projectId, ct)
                ?? throw new NotFoundException("Project not found", "Project", projectId);

            var sprints = await _sprintRepository.GetByProjectIdAsync(projectId, ct);

            return sprints.Select(sprint => new SprintResponse
            {
                Id = sprint.Id,
                Name = sprint.Name,
                Goal = sprint.Goal,
                ProjectId = sprint.ProjectId,
                Status = sprint.Status.ToString(),
                StartDate = sprint.StartDate,
                EndDate = sprint.EndDate,
                Capacity = sprint.Capacity,
                CurrentCapacity = sprint.CalculateCurrentCapacity(),
                Velocity = sprint.CalculateVelocity(),
                WorkItemCount = sprint.GetActiveWorkItemCount(),
                CreatedAt = sprint.CreatedAt,
                UpdatedAt = sprint.UpdatedAt
            });
        }
    }
}
