using ProjectManager.Application.DTOs.Roles;

namespace ProjectManager.Application.UseCases.Roles.Update
{
    public interface IUpdateRoleUseCase
    {
        Task Execute(Guid id, UpdateRoleRequest request, CancellationToken ct = default);
    }
}
