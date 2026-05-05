using ProjectManager.Application.DTOs.Roles;
using ProjectManager.Application.Ports;
using ProjectManager.Application.Exceptions;

namespace ProjectManager.Application.UseCases.Roles.Update
{
    public sealed class UpdateRoleUseCase(
        IRoleRepository roleRepository
    ) : IUpdateRoleUseCase
    {
        private readonly IRoleRepository _roleRepository = roleRepository;

        public async Task Execute(Guid id, UpdateRoleRequest request, CancellationToken ct = default)
        {
            var role = await _roleRepository.GetByIdAsync(id, ct)
                ?? throw new NotFoundException("Role not found", "Role", id);

            role.Name = request.Name;
            role.Description = request.Description;
            role.UpdatedAt = DateTime.UtcNow;

            await _roleRepository.UpdateAsync(role, ct);
        }
    }
}
