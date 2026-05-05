using ProjectManager.Application.DTOs.Permissions;
using ProjectManager.Application.Ports;

namespace ProjectManager.Application.UseCases.Permissions.GetAll
{
    public sealed class GetAllPermissionsUseCase(
        IPermissionRepository permissionRepository
    ) : IGetAllPermissionsUseCase
    {
        private readonly IPermissionRepository _permissionRepository = permissionRepository;

        public async Task<IEnumerable<PermissionDto>> Execute(CancellationToken ct = default)
        {
            var permissions = await _permissionRepository.GetAllAsync(ct);
            return permissions.Select(p => new PermissionDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description
            });
        }
    }
}
