namespace ProjectManager.Application.DTOs.WorkItems
{
    public sealed class WorkItemFilter
    {
        public string? Status { get; set; }
        public Guid? AssigneeId { get; set; }
        public Guid? TeamId { get; set; }
        public string? Cursor { get; set; }
        public int PageSize { get; set; } = 20;
    }
}
