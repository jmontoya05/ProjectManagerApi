namespace ProjectManager.Application.UseCases.Projects.Update
{
    public sealed class UpdateProjectRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}
