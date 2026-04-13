using ProjectManager.Application.DTOs.WorkItems.Request;
using ProjectManager.Application.Ports;
using ProjectManager.Domain.Entities;

namespace ProjectManager.Application.UseCases.WorkItems.Create
{
    public sealed class CreateWorkItemUseCase(IWorkItemRepository workItemRepository, IUserRepository userRepository, IProjectRepository projectRepository) : ICreateWorkItemUseCase
    {
        private readonly IWorkItemRepository _workItemRepository = workItemRepository;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IProjectRepository _projectRepository = projectRepository;

        public async Task<Guid> Execute(Guid projectId, CreateWorkItemRequest request, Guid currentUserId, CancellationToken ct = default)
        {
            var project = await _projectRepository.GetByIdAsync(projectId, ct)
                ?? throw new InvalidOperationException("Project not found.");

            if (request.ParentWorkItemId.HasValue)
            {
                var parent = await _workItemRepository.GetByIdAsync(request.ParentWorkItemId.Value, ct)
                    ?? throw new InvalidOperationException("Parent work item not found.");

                if (parent.ProjectId != projectId)
                    throw new InvalidOperationException("Parent work item belongs to different project.");
            }

            if (request.AssigneeId.HasValue)
            {
                _ = await _userRepository.GetByIdAsync(request.AssigneeId.Value, ct)
                    ?? throw new InvalidOperationException("Assignee not found.");

                var assigneeRoles = await _userRepository.GetUserRolesByOrganizationAsync(request.AssigneeId.Value, project.OrganizationId, ct);
                if (!assigneeRoles.Any())
                    throw new InvalidOperationException("Assignee is not a member of this organization.");
            }

            var workItem = new WorkItem
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                Description = request.Description,
                Type = request.Type,
                Priority = request.Priority,
                Status = "Backlog",
                StoryPoints = request.StoryPoints,
                TimeEstimateMinutes = request.TimeEstimateMinutes,
                ProjectId = projectId,
                ParentWorkItemId = request.ParentWorkItemId,
                AssigneeId = request.AssigneeId,
                TeamId = request.TeamId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _workItemRepository.AddAsync(workItem, ct);

            return workItem.Id;
        }
    }
}
