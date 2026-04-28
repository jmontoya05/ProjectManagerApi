using ProjectManager.Application.DTOs.Permissions;
using ProjectManager.Application.Ports;
using ProjectManager.Domain.Entities;

namespace ProjectManager.Application.UseCases.Permissions
{
    public sealed class PermissionService(
        IPermissionRepository permissionRepository
    ) : IPermissionService
    {
        private readonly IPermissionRepository _permissionRepository = permissionRepository;

        public async Task<IEnumerable<PermissionDto>> GetAllAsync(CancellationToken ct = default)
        {
            var permissions = await _permissionRepository.GetAllAsync(ct);
            return permissions.Select(p => new PermissionDto { Id = p.Id, Name = p.Name, Description = p.Description });
        }

        public async Task<PermissionDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            var permission = await _permissionRepository.GetByIdAsync(id, ct);
            return permission == null ? null : new PermissionDto { Id = permission.Id, Name = permission.Name, Description = permission.Description };
        }

        public async Task<Guid> CreateAsync(CreatePermissionRequest request, CancellationToken ct = default)
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

        public async Task UpdateAsync(Guid id, UpdatePermissionRequest request, CancellationToken ct = default)
        {
            var permission = await _permissionRepository.GetByIdAsync(id, ct);
            if (permission == null) throw new InvalidOperationException("Permission not found");
            permission.Name = request.Name;
            permission.Description = request.Description;
            permission.UpdatedAt = DateTime.UtcNow;
            await _permissionRepository.UpdateAsync(permission, ct);
        }

        public async Task DeleteAsync(Guid id, CancellationToken ct = default)
        {
            await _permissionRepository.DeleteAsync(id, ct);
        }
    }
}
