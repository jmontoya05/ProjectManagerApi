using ProjectManager.Application.DTOs.Permissions;
using ProjectManager.Application.Ports;
using ProjectManager.Domain.Entities;

namespace ProjectManager.Application.UseCases.Permissions.Create
{
    public sealed class CreatePermissionUseCase(
        IPermissionRepository permissionRepository
    ) : ICreatePermissionUseCase
    {
        private readonly IPermissionRepository _permissionRepository = permissionRepository;

        public async Task<Guid> Execute(CreatePermissionRequest request, CancellationToken ct = default)
        {
            var permission = new Permission
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _permissionRepository.AddAsync(permission, ct);
            return permission.Id;
        }
    }
}
