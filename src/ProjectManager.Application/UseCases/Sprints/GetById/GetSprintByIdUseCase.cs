using ProjectManager.Application.DTOs.Sprints;
using ProjectManager.Application.Ports;
using ProjectManager.Application.Exceptions;

namespace ProjectManager.Application.UseCases.Sprints.GetById
{
    public sealed class GetSprintByIdUseCase(
        ISprintRepository sprintRepository,
        IProjectRepository projectRepository
    ) : IGetSprintByIdUseCase
    {
        private readonly ISprintRepository _sprintRepository = sprintRepository;
        private readonly IProjectRepository _projectRepository = projectRepository;

        public async Task<SprintResponse?> Execute(Guid projectId, Guid sprintId, CancellationToken ct = default)
        {
            var project = await _projectRepository.GetByIdAsync(projectId, ct)
                ?? throw new NotFoundException("Project not found", "Project", projectId);

            var sprint = await _sprintRepository.GetByIdAsync(sprintId, ct);
            if (sprint == null || sprint.ProjectId != projectId)
                return null;

            return new SprintResponse
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
            };
        }
    }
}
