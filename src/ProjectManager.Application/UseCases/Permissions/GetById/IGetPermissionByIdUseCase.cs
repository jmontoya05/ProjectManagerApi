using ProjectManager.Application.DTOs.Permissions;

namespace ProjectManager.Application.UseCases.Permissions.GetById
{
    public interface IGetPermissionByIdUseCase
    {
        Task<PermissionDto?> Execute(Guid id, CancellationToken ct = default);
    }
}
