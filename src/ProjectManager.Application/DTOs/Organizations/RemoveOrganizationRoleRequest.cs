namespace ProjectManager.Application.DTOs.Organizations;

public sealed class RemoveOrganizationRoleRequest
{
    public Guid UserId { get; set; }
    public Guid OrganizationId { get; set; }
}