namespace ProjectManager.Application.UseCases.Auth.SelectOrganization
{
    public sealed class SelectOrganizationRequest
    {
        public string Refreshtoken { get; set; } = null!;
        public required Guid OrganizationId { get; set; }
    }
}
