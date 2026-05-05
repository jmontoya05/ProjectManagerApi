using ProjectManager.Application.DTOs.Permissions;

namespace ProjectManager.Application.UseCases.Permissions.GetAll
{
    public interface IGetAllPermissionsUseCase
    {
        Task<IEnumerable<PermissionDto>> Execute(CancellationToken ct = default);
    }
}
