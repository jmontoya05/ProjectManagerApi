using ProjectManager.Application.DTOs.Permissions;

namespace ProjectManager.Application.UseCases.Permissions.Update
{
    public interface IUpdatePermissionUseCase
    {
        Task Execute(Guid id, UpdatePermissionRequest request, CancellationToken ct = default);
    }
}
