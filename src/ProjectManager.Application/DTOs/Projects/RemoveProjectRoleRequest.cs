namespace ProjectManager.Application.DTOs.Projects;

public class RemoveProjectRoleRequest
{
    public Guid UserId { get; set; }
    public Guid ProjectId { get; set; }
}