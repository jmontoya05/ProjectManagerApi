namespace ProjectManager.Application.DTOs.Sprints
{
    public sealed class ListSprintWorkItemsResponse
    {
        public Guid WorkItemId { get; set; }
        public string WorkItemTitle { get; set; } = null!;
        public string WorkItemType { get; set; } = null!;
        public string Status { get; set; } = null!;
        public int? StoryPoints { get; set; }
        public int OrderIndex { get; set; }
    }
}

