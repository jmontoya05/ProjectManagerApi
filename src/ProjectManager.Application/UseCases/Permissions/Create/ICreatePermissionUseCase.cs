using ProjectManager.Application.DTOs.Permissions;

namespace ProjectManager.Application.UseCases.Permissions.Create
{
    public interface ICreatePermissionUseCase
    {
        Task<Guid> Execute(CreatePermissionRequest request, CancellationToken ct = default);
    }
}
