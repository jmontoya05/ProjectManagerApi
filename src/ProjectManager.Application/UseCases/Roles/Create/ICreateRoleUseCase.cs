using ProjectManager.Application.DTOs.Roles;

namespace ProjectManager.Application.UseCases.Roles.Create
{
    public interface ICreateRoleUseCase
    {
        Task<Guid> Execute(CreateRoleRequest request, CancellationToken ct = default);
    }
}
