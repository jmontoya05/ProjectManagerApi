namespace ProjectManager.Application.UseCases.Organizations.List
{
    public sealed class ListOrganizationsResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Status { get; set; } = null!;
        public IEnumerable<string> Roles { get; set; } = null!;
    }
}
