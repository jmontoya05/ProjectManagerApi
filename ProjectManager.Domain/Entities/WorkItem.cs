namespace ProjectManager.Domain.Entities
{
    public class WorkItem
    {
        public Guid? AssigneeId { get; set; }
        public Guid? TeamId { get; set; }

        // Navigation
        public virtual User? Assignee { get; set; }
        public virtual Team? Team { get; set; }
    }
}
