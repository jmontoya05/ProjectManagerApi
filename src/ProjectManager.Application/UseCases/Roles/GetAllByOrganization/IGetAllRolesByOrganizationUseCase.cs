using ProjectManager.Application.DTOs.Roles;

namespace ProjectManager.Application.UseCases.Roles.GetAllByOrganization
{
    public interface IGetAllRolesByOrganizationUseCase
    {
        Task<IEnumerable<RoleDto>> Execute(Guid organizationId, CancellationToken ct = default);
    }
}
