namespace ProjectManager.Application.DTOs.Projects;

public class AssignProjectRoleRequest
{
    public Guid UserId { get; set; }
    public Guid ProjectId { get; set; }
    public Guid RoleId { get; set; }
}