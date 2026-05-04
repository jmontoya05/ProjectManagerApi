using ProjectManager.Application.DTOs;
using ProjectManager.Application.DTOs.WorkItems;
using ProjectManager.Application.Ports;

namespace ProjectManager.Application.UseCases.WorkItems.List
{
    public sealed class ListWorkItemsUseCase(
        IWorkItemRepository workItemRepository
    ) : IListWorkItemsUseCase
    {
        private readonly IWorkItemRepository _workItemRepository = workItemRepository;

        public async Task<PagedResponse<WorkItemResponse>> Execute(Guid projectId, WorkItemFilter filter, CancellationToken ct = default)
        {
            var (items, hasNextPage) = await _workItemRepository.GetPagedByProjectAsync(projectId, filter, ct);

            string? nextCursor = null;
            var workItems = items.ToList();
            if (hasNextPage && workItems.Count != 0)
            {
                nextCursor = workItems.Last().UpdatedAt.ToString("o");
            }

            var workItemsDto = workItems.Select(w => new WorkItemResponse
            {
                Id = w.Id,
                Title = w.Title,
                Description = w.Description,
                Type = w.Type.ToString(),
                Status = w.Status.ToString(),
                Priority = w.Priority.ToString(),
                StoryPoints = w.StoryPoints,
                TimeEstimateMinutes = w.TimeEstimateMinutes,
                ProjectId = w.ProjectId,
                ParentWorkItemId = w.ParentWorkItemId,
                ParentWorkItemTitle = w.ParentWorkItem?.Title,
                AssigneeId = w.AssigneeId,
                AssigneeName = w.Assignee?.DisplayName,
                TeamId = w.TeamId,
                TeamName = w.Team?.Name,
                CreatedAt = w.CreatedAt,
                UpdatedAt = w.UpdatedAt,
                Cursor = w.UpdatedAt.ToString("o"),
                SubtaskCount = w.Subtasks.Count
            });

            return new PagedResponse<WorkItemResponse>
            {
                Items = workItemsDto.ToList(),
                NextCursor = nextCursor
            };
        }
    }
}
