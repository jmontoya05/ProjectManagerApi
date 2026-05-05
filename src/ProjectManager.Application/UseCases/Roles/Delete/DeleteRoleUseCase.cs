using ProjectManager.Application.Ports;
using ProjectManager.Application.Exceptions;

namespace ProjectManager.Application.UseCases.Roles.Delete
{
    public sealed class DeleteRoleUseCase(
        IRoleRepository roleRepository
    ) : IDeleteRoleUseCase
    {
        private readonly IRoleRepository _roleRepository = roleRepository;

        public async Task Execute(Guid id, CancellationToken ct = default)
        {
            var role = await _roleRepository.GetByIdAsync(id, ct)
                ?? throw new NotFoundException("Role not found", "Role", id);

            await _roleRepository.DeleteAsync(role.Id, ct);
        }
    }
}
