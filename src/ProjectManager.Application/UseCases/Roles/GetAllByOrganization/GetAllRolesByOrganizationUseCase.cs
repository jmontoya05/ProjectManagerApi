using ProjectManager.Application.DTOs.Roles;
using ProjectManager.Application.Ports;

namespace ProjectManager.Application.UseCases.Roles.GetAllByOrganization
{
    public sealed class GetAllRolesByOrganizationUseCase(
        IRoleRepository roleRepository
    ) : IGetAllRolesByOrganizationUseCase
    {
        private readonly IRoleRepository _roleRepository = roleRepository;

        public async Task<IEnumerable<RoleDto>> Execute(Guid organizationId, CancellationToken ct = default)
        {
            var roles = await _roleRepository.GetAllByOrganizationAsync(organizationId, ct);
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
