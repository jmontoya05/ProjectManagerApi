using ProjectManager.Application.DTOs.Roles;
using ProjectManager.Application.Ports;
using ProjectManager.Domain.Entities;

namespace ProjectManager.Application.UseCases.Roles
{
    public sealed class RoleService(
        IRoleRepository roleRepository
    ) : IRoleService
    {
        private readonly IRoleRepository _roleRepository = roleRepository;

        public async Task<IEnumerable<RoleDto>> GetAllAsync(CancellationToken ct = default)
        {
            var roles = await _roleRepository.GetAllAsync(ct);
            return roles.Select(r => new RoleDto{Id = r.Id, Name = r.Name, Description = r.Description, OrganizationId = r.OrganizationId});
        }

        public async Task<IEnumerable<RoleDto>> GetAllByOrganizationAsync(Guid organizationId, CancellationToken ct = default)
        {
            var roles = await _roleRepository.GetAllByOrganizationAsync(organizationId, ct);
            return roles.Select(r => new RoleDto { Id = r.Id, Name = r.Name, Description = r.Description, OrganizationId = r.OrganizationId });
        }

        public async Task<RoleDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            var role = await _roleRepository.GetByIdAsync(id, ct);
            return role == null ? null : new RoleDto { Id = role.Id, Name = role.Name, Description = role.Description, OrganizationId = role.OrganizationId };
        }

        public async Task<Guid> CreateAsync(CreateRoleRequest request, CancellationToken ct = default)
        {
            var role = new Role
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                OrganizationId = request.OrganizationId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            await _roleRepository.AddAsync(role, ct);
            return role.Id;
        }

        public async Task UpdateAsync(Guid id, UpdateRoleRequest request, CancellationToken ct = default)
        {
            var role = await _roleRepository.GetByIdAsync(id, ct);
            if (role == null) throw new InvalidOperationException("Role not found");
            role.Name = request.Name;
            role.Description = request.Description;
            role.UpdatedAt = DateTime.UtcNow;
            await _roleRepository.UpdateAsync(role, ct);
        }

        public async Task DeleteAsync(Guid id, CancellationToken ct = default)
        {
            await _roleRepository.DeleteAsync(id, ct);
        }
    }
}
