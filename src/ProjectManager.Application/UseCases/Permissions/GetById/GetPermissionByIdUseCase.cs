using ProjectManager.Application.DTOs.Permissions;
using ProjectManager.Application.Ports;

namespace ProjectManager.Application.UseCases.Permissions.GetById
{
    public sealed class GetPermissionByIdUseCase(
        IPermissionRepository permissionRepository
    ) : IGetPermissionByIdUseCase
    {
        private readonly IPermissionRepository _permissionRepository = permissionRepository;

        public async Task<PermissionDto?> Execute(Guid id, CancellationToken ct = default)
        {
            var permission = await _permissionRepository.GetByIdAsync(id, ct);
            return permission == null ? null : new PermissionDto
            {
                Id = permission.Id,
                Name = permission.Name,
                Description = permission.Description
            };
        }
    }
}
