using ProjectManager.Application.DTOs.Sprints;
using ProjectManager.Application.Ports;
using ProjectManager.Application.Exceptions;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Enums;

namespace ProjectManager.Application.UseCases.Sprints.Create
{
    public sealed class CreateSprintUseCase(
        ISprintRepository sprintRepository,
        IProjectRepository projectRepository
    ) : ICreateSprintUseCase
    {
        private readonly ISprintRepository _sprintRepository = sprintRepository;
        private readonly IProjectRepository _projectRepository = projectRepository;

        public async Task<Guid> Execute(Guid projectId, CreateSprintRequest request, CancellationToken ct = default)
        {
            _ = await _projectRepository.GetByIdAsync(projectId, ct)
                ?? throw new NotFoundException("Project not found", "Project", projectId);

            var sprint = new Sprint
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Goal = request.Goal,
                ProjectId = projectId,
                Status = SprintStatus.Planning,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Capacity = request.Capacity,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _sprintRepository.AddAsync(sprint, ct);
            return sprint.Id;
        }
    }
}
