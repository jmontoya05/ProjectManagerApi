namespace ProjectManager.Application.DTOs.Projects
{
    public sealed class CreateProjectRequest
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public required Guid OwnerId { get; set; }
    }
}
