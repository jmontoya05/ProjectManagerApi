using ProjectManager.Application.Ports;
using ProjectManager.Application.Exceptions;
using ProjectManager.Domain.Exceptions;

namespace ProjectManager.Application.UseCases.Sprints.Complete
{
    public sealed class CompleteSprintUseCase(
        ISprintRepository sprintRepository,
        IProjectRepository projectRepository
    ) : ICompleteSprintUseCase
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

            try
            {
                sprint.Complete();
            }
            catch (InvalidSprintStatusTransitionException ex)
            {
                throw new BusinessRuleException(ex.Message, ex.ErrorCode);
            }

            await _sprintRepository.UpdateAsync(sprint, ct);
        }
    }
}
