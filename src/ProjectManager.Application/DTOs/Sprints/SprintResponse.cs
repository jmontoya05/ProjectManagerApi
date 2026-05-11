namespace ProjectManager.Application.DTOs.Sprints
{
    public sealed class SprintResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Goal { get; set; }
        public Guid ProjectId { get; set; }
        public string Status { get; set; } = null!;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? Capacity { get; set; }
        public int CurrentCapacity { get; set; }
        public int Velocity { get; set; }
        public int WorkItemCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
