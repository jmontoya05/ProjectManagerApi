namespace ProjectManager.Application.DTOs.Sprints
{
    public sealed class ReorderSprintWorkItemsRequest
    {
        public List<Guid> WorkItemIds { get; set; } = [];
    }
}

