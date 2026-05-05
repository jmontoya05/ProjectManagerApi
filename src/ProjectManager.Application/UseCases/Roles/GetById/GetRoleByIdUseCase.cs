using ProjectManager.Application.DTOs.Roles;
using ProjectManager.Application.Ports;

namespace ProjectManager.Application.UseCases.Roles.GetById
{
    public sealed class GetRoleByIdUseCase(
        IRoleRepository roleRepository
    ) : IGetRoleByIdUseCase
    {
        private readonly IRoleRepository _roleRepository = roleRepository;

        public async Task<RoleDto?> Execute(Guid id, CancellationToken ct = default)
        {
            var role = await _roleRepository.GetByIdAsync(id, ct);
            return role == null ? null : new RoleDto
            {
                Id = role.Id,
                Name = role.Name,
                Description = role.Description,
                OrganizationId = role.OrganizationId
            };
        }
    }
}
