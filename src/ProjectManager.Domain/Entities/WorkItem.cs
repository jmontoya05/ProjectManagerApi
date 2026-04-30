namespace ProjectManager.Domain.Entities
{
    public class WorkItem : EntityBase
    {
        public string Title { get; init; } = null!;
        public string Description { get; init; } = null!;
        public string Type { get; init; } = "Task"; // Task, Bug, Story
        public string Priority { get; init; } = "Medium"; // Low, Medium, High, Critical
        public string Status { get; set; } = "Backlog"; // Backlog, InProgress, Done
        public int? StoryPoints { get; init; }
        public int? TimeEstimateMinutes { get; init; }
        public Guid ProjectId { get; init; }
        public Guid? ParentWorkItemId { get; init; }
        public Guid? AssigneeId { get; init; }
        public Guid? TeamId { get; init; }

        // Navigation
        public virtual Project Project { get; init; } = null!;
        public virtual WorkItem? ParentWorkItem { get; init; }
        public virtual ICollection<WorkItem> Subtasks { get; init; } = [];
        public virtual User? Assignee { get; init; }
        public virtual Team? Team { get; init; }
    }
}
