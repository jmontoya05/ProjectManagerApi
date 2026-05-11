using ProjectManager.Application.Ports;
using ProjectManager.Application.Exceptions;
using ProjectManager.Domain.Exceptions;

namespace ProjectManager.Application.UseCases.Sprints.Start
{
    public sealed class StartSprintUseCase(
        ISprintRepository sprintRepository,
        IProjectRepository projectRepository
    ) : IStartSprintUseCase
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
                sprint.Start();
            }
            catch (InvalidSprintStatusTransitionException ex)
            {
                throw new BusinessRuleException(ex.Message, ex.ErrorCode);
            }
            catch (InvalidOperationException ex)
            {
                throw new BusinessRuleException(ex.Message, "SPRINT_INVALID_DATES");
            }

            await _sprintRepository.UpdateAsync(sprint, ct);
        }
    }
}
