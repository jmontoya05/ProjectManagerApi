namespace ProjectManager.Application.DTOs.Organizations;

public sealed class AssignOrganizationRoleRequest
{
    public Guid UserId { get; set; }
    public Guid OrganizationId { get; set; }
    public Guid RoleId { get; set; }
}