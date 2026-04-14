using ProjectManager.Application.DTOs;
using ProjectManager.Application.DTOs.WorkItems;
using ProjectManager.Application.Ports;

namespace ProjectManager.Application.UseCases.WorkItems.List
{
    public sealed class ListWorkItemsUseCase(IWorkItemRepository workItemRepository) : IListWorkItemsUseCase
    {
        private readonly IWorkItemRepository _workItemRepository = workItemRepository;

        public async Task<PagedResponse<WorkItemResponse>> Execute(Guid projectId, WorkItemFilter filter, CancellationToken ct = default)
        {
            var query = await _workItemRepository.GetByProjectAsync(projectId, ct);

            if (!string.IsNullOrWhiteSpace(filter.Status))
                query = query.Where(wi => wi.Status == filter.Status);

            if (filter.AssigneeId.HasValue)
                query = query.Where(w => w.AssigneeId == filter.AssigneeId);

            if (filter.TeamId.HasValue)
                query = query.Where(w => w.TeamId == filter.TeamId);

            if (!string.IsNullOrWhiteSpace(filter.Cursor))
            {
                var cursorDateTime = DateTime.Parse(filter.Cursor, System.Globalization.CultureInfo.InvariantCulture);
                query = query.Where(w => w.UpdatedAt < cursorDateTime);
            }

            var items = query
                .Take(filter.PageSize + 1)
                .ToList();

            var hasNextPage = items.Count > filter.PageSize;
            string? nextCursor = null;
            if (hasNextPage)
            {
                items.RemoveAt(items.Count - 1);
                nextCursor = items.Last().UpdatedAt.ToString("o");
            }

            var workItemDtos = items.Select(w => new WorkItemResponse
            {
                Id = w.Id,
                Title = w.Title,
                Description = w.Description,
                Status = w.Status,
                Priority = w.Priority,
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
                Cursor = w.UpdatedAt.ToString("o")
            });

            return new PagedResponse<WorkItemResponse>
            {
                Items = workItemDtos.ToList(),
                NextCursor = nextCursor
            };
        }
    }
}
