using ProjectManager.Application.DTOs.Roles;

namespace ProjectManager.Application.UseCases.Roles
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleDto>> GetAllAsync(CancellationToken ct = default);
        Task<RoleDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<Guid> CreateAsync(CreateRoleRequest request, CancellationToken ct = default);
        Task UpdateAsync(Guid id, UpdateRoleRequest request, CancellationToken ct = default);
        Task DeleteAsync(Guid id, CancellationToken ct = default);
        Task<IEnumerable<RoleDto>> GetAllByOrganizationAsync(Guid organizationId, CancellationToken ct = default);
    }
}
