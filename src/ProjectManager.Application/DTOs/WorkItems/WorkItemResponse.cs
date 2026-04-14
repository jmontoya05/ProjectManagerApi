namespace ProjectManager.Application.DTOs.WorkItems
{
    public sealed class WorkItemResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Type { get; set; } = null!;
        public string Priority { get; set; } = null!;
        public string Status { get; set; } = null!;
        public int? StoryPoints { get; set; }
        public int? TimeEstimateMinutes { get; set; }
        public Guid ProjectId { get; set; }
        public Guid? ParentWorkItemId { get; set; }
        public string? ParentWorkItemTitle { get; set; }
        public Guid? AssigneeId { get; set; }
        public string? AssigneeName { get; set; }
        public Guid? TeamId { get; set; }
        public string? TeamName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Cursor { get; set; } = null!;
        public int SubtaskCount { get; set; }
    }
}
