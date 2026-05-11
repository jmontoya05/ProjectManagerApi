namespace ProjectManager.Application.DTOs.Sprints
{
    public sealed class CreateSprintRequest
    {
        public string Name { get; set; } = null!;
        public string? Goal { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? Capacity { get; set; }
    }
}
