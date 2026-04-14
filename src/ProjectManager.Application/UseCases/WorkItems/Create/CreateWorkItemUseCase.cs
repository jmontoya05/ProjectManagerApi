using ProjectManager.Application.DTOs.WorkItems;
using ProjectManager.Application.Ports;
using ProjectManager.Application.Exceptions;
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
                ?? throw new NotFoundException("Project not found", "Project", projectId);

            if (request.ParentWorkItemId.HasValue)
            {
                var parent = await _workItemRepository.GetByIdAsync(request.ParentWorkItemId.Value, ct)
                    ?? throw new NotFoundException("Parent work item not found", "WorkItem", request.ParentWorkItemId.Value);

                if (parent.ProjectId != projectId)
                    throw new BusinessRuleException("Parent work item belongs to a different project", "PARENT_PROJECT_MISMATCH");
            }

            if (request.AssigneeId.HasValue)
            {
                _ = await _userRepository.GetByIdAsync(request.AssigneeId.Value, ct)
                    ?? throw new NotFoundException("Assignee not found", "User", request.AssigneeId.Value);
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
