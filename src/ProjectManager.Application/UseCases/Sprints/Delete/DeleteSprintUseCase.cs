using ProjectManager.Application.Ports;
using ProjectManager.Application.Exceptions;
using ProjectManager.Domain.Enums;

namespace ProjectManager.Application.UseCases.Sprints.Delete
{
    public sealed class DeleteSprintUseCase(
        ISprintRepository sprintRepository,
        IProjectRepository projectRepository
    ) : IDeleteSprintUseCase
    {
        private readonly ISprintRepository _sprintRepository = sprintRepository;
        private readonly IProjectRepository _projectRepository = projectRepository;

        public async Task Execute(Guid projectId, Guid sprintId, CancellationToken ct = default)
        {
            _ = await _projectRepository.GetByIdAsync(projectId, ct)
                ?? throw new NotFoundException("Project not found", "Project", projectId);

            var sprint = await _sprintRepository.GetByIdAsync(sprintId, ct)
                ?? throw new NotFoundException("Sprint not found", "Sprint", sprintId);

            if (sprint.ProjectId != projectId)
                throw new ForbiddenException("Sprint does not belong to the specified project.");
            
            if (sprint.Status != SprintStatus.Planning)
                throw new BusinessRuleException("Can only delete sprints in planning status.", "SPRINT_NOT_IN_PLANNING");

            await _sprintRepository.DeleteAsync(sprint.Id, ct);
        }
    }
}
