using ProjectManager.Application.DTOs.Roles;

namespace ProjectManager.Application.UseCases.Roles.GetById
{
    public interface IGetRoleByIdUseCase
    {
        Task<RoleDto?> Execute(Guid id, CancellationToken ct = default);
    }
}
