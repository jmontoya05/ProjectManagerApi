using ProjectManager.Application.Ports;
using ProjectManager.Application.Exceptions;

namespace ProjectManager.Application.UseCases.Permissions.Delete
{
    public sealed class DeletePermissionUseCase(
        IPermissionRepository permissionRepository
    ) : IDeletePermissionUseCase
    {
        private readonly IPermissionRepository _permissionRepository = permissionRepository;

        public async Task Execute(Guid id, CancellationToken ct = default)
        {
            var permission = await _permissionRepository.GetByIdAsync(id, ct)
                ?? throw new NotFoundException("Permission not found", "Permission", id);

            await _permissionRepository.DeleteAsync(permission.Id, ct);
        }
    }
}
