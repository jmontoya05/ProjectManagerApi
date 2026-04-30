namespace ProjectManager.Application.DTOs.Auth
{
    public sealed class SelectOrganizationRequest
    {
        public string RefreshToken { get; set; } = null!;
        public required Guid OrganizationId { get; set; }
    }
}
