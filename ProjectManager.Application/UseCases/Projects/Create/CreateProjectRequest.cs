namespace ProjectManager.Application.UseCases.Projects.Create
{
    public sealed class CreateProjectRequest
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
    }
}
