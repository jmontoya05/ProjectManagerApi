using ProjectManager.Application.DTOs.Permissions;
using ProjectManager.Application.Ports;
using ProjectManager.Application.Exceptions;

namespace ProjectManager.Application.UseCases.Permissions.Update
{
    public sealed class UpdatePermissionUseCase(
        IPermissionRepository permissionRepository
    ) : IUpdatePermissionUseCase
    {
        private readonly IPermissionRepository _permissionRepository = permissionRepository;

        public async Task Execute(Guid id, UpdatePermissionRequest request, CancellationToken ct = default)
        {
            var permission = await _permissionRepository.GetByIdAsync(id, ct)
                ?? throw new NotFoundException("Permission not found", "Permission", id);

            permission.Name = request.Name;
            permission.Description = request.Description;
            permission.UpdatedAt = DateTime.UtcNow;

            await _permissionRepository.UpdateAsync(permission, ct);
        }
    }
}
