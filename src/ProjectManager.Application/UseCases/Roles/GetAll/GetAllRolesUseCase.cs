using ProjectManager.Application.DTOs.Roles;
using ProjectManager.Application.Ports;

namespace ProjectManager.Application.UseCases.Roles.GetAll
{
    public sealed class GetAllRolesUseCase(
        IRoleRepository roleRepository
    ) : IGetAllRolesUseCase
    {
        private readonly IRoleRepository _roleRepository = roleRepository;

        public async Task<IEnumerable<RoleDto>> Execute(CancellationToken ct = default)
        {
            var roles = await _roleRepository.GetAllAsync(ct);
            return roles.Select(r => new RoleDto
            {
                Id = r.Id,
                Name = r.Name,
                Description = r.Description,
                OrganizationId = r.OrganizationId
            });
        }
    }
}
