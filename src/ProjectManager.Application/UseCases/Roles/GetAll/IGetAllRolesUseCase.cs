using ProjectManager.Application.DTOs.Roles;

namespace ProjectManager.Application.UseCases.Roles.GetAll
{
    public interface IGetAllRolesUseCase
    {
        Task<IEnumerable<RoleDto>> Execute(CancellationToken ct = default);
    }
}
