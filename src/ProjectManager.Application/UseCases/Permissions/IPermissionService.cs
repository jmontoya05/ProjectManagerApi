using ProjectManager.Application.DTOs.Permissions;

namespace ProjectManager.Application.UseCases.Permissions
{
    public interface IPermissionService
    {
        Task<IEnumerable<PermissionDto>> GetAllAsync(CancellationToken ct = default);
        Task<PermissionDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<Guid> CreateAsync(CreatePermissionRequest request, CancellationToken ct = default);
        Task UpdateAsync(Guid id, UpdatePermissionRequest request, CancellationToken ct = default);
        Task DeleteAsync(Guid id, CancellationToken ct = default);
    }
}
