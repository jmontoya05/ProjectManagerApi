using ProjectManager.Application.DTOs.Sprints;
using ProjectManager.Application.Ports;
using ProjectManager.Application.Exceptions;
using ProjectManager.Domain.Enums;

namespace ProjectManager.Application.UseCases.Sprints.Update
{
    public sealed class UpdateSprintUseCase(
        ISprintRepository sprintRepository,
        IProjectRepository projectRepository
    ) : IUpdateSprintUseCase
    {
        private readonly ISprintRepository _sprintRepository = sprintRepository;
        private readonly IProjectRepository _projectRepository = projectRepository;

        public async Task Execute(Guid projectId, Guid sprintId, UpdateSprintRequest request, CancellationToken ct = default)
        {
            var project = await _projectRepository.GetByIdAsync(projectId, ct)
                ?? throw new NotFoundException("Project not found", "Project", projectId);

            var sprint = await _sprintRepository.GetByIdAsync(sprintId, ct)
                ?? throw new NotFoundException("Sprint not found", "Sprint", sprintId);

            if (sprint.ProjectId != projectId)
                throw new ForbiddenException("Sprint does not belong to the specified project.");
            
            if (sprint.Status != SprintStatus.Planning)
                throw new BusinessRuleException("Can only update sprints in planning status.", "SPRINT_NOT_IN_PLANNING");

            if (!string.IsNullOrWhiteSpace(request.Name))
                sprint.Name = request.Name;

            if (request.Goal != null)
                sprint.Goal = request.Goal;

            if (request.StartDate.HasValue)
                sprint.StartDate = request.StartDate;

            if (request.EndDate.HasValue)
                sprint.EndDate = request.EndDate;

            if (request.Capacity.HasValue)
                sprint.Capacity = request.Capacity;

            sprint.UpdatedAt = DateTime.UtcNow;

            await _sprintRepository.UpdateAsync(sprint, ct);
        }
    }
}
