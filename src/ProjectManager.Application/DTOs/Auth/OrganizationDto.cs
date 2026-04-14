namespace ProjectManager.Application.DTOs.Auth
{
    public sealed class OrganizationDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
    }
}
