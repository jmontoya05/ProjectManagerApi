namespace ProjectManager.Application.DTOs.Auth
{
    public sealed class SelectOrganizationRequest
    {
        public string Refreshtoken { get; set; } = null!;
        public required Guid OrganizationId { get; set; }
    }
}
