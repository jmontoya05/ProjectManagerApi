using ProjectManager.Application.Exceptions;
using ProjectManager.Application.Ports;

namespace ProjectManager.Application.UseCases.Sprints.RemoveWorkItem
{
    public sealed class RemoveWorkItemFromSprintUseCase(
        ISprintRepository sprintRepository,
        IProjectRepository projectRepository
    ) : IRemoveWorkItemFromSprintUseCase
    {
        private readonly ISprintRepository _sprintRepository = sprintRepository;
        private readonly IProjectRepository _projectRepository = projectRepository;

        public async Task Execute(Guid projectId, Guid sprintId, Guid workItemId, CancellationToken ct = default)
        {
            _ = await _projectRepository.GetByIdAsync(projectId, ct)
                ?? throw new NotFoundException("Project not found", "Project", projectId);

            var sprint = await _sprintRepository.GetByIdAsync(sprintId, ct)
                ?? throw new NotFoundException("Sprint not found", "Sprint", sprintId);

            if (sprint.ProjectId != projectId)
                throw new ForbiddenException("Sprint does not belong to the specified project.");

            sprint.RemoveWorkItem(workItemId);
            await _sprintRepository.UpdateAsync(sprint, ct);
        }
    }
}

