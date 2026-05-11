namespace ProjectManager.Application.DTOs.Sprints
{
    public sealed class SprintVelocityResponse
    {
        public Guid SprintId { get; set; }
        public int Velocity { get; set; }
        public int Capacity { get; set; }
        public int CurrentLoad { get; set; }
        public int RemainingCapacity { get; set; }
        public int CompletedWorkItems { get; set; }
        public int TotalWorkItems { get; set; }
    }
}

