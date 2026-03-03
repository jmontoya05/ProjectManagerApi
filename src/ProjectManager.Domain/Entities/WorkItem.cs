namespace ProjectManager.Domain.Entities
{
    public class WorkItem
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Type { get; set; } = "Task"; // Task, Bug, Story
        public string Priority { get; set; } = "Medium"; // Low, Medium, High, Critical
        public string Status { get; set; } = "Backlog"; // Backlog, InProgress, Done
        public int? StoryPoints { get; set; }
        public int? TimeEstimateMinutes { get; set; }
        public Guid ProjectId { get; set; }
        public Guid? ParentWorkItemId { get; set; }
        public Guid? AssigneeId { get; set; }
        public Guid? TeamId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation
        public virtual Project Project { get; set; } = null!;
        public virtual WorkItem? ParentWorkItem { get; set; }
        public virtual ICollection<WorkItem> Subtasks { get; set; } = [];
        public virtual User? Assignee { get; set; }
        public virtual Team? Team { get; set; }
    }
}
