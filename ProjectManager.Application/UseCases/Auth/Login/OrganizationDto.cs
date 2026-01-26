namespace ProjectManager.Application.UseCases.Auth.Login
{
    public sealed class OrganizationDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
    }
}
