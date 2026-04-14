namespace ProjectManager.Application.DTOs.WorkItems
{
    public sealed class CreateWorkItemRequest
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Type { get; set; } = "Task";
        public string Priority { get; set; } = "Medium";
        public int? StoryPoints { get; set; }
        public int? TimeEstimateMinutes { get; set; }
        public Guid? ParentWorkItemId { get; set; }
        public Guid? AssigneeId { get; set; }
        public Guid? TeamId { get; set; }
    }
}
