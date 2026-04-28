using ProjectManager.Application.DTOs.Organizations;

namespace ProjectManager.Application.UseCases.Organizations.RoleAssigment
{
    public interface IOrganizationRoleAssignmentUseCase
    {
        Task AssignRoleAsync(AssignOrganizationRoleRequest request, CancellationToken ct = default);
        Task RemoveRoleAsync(RemoveOrganizationRoleRequest request, CancellationToken ct = default);
    }
}
