using ProjectManager.Application.DTOs.Roles;
using ProjectManager.Application.Ports;
using ProjectManager.Domain.Entities;

namespace ProjectManager.Application.UseCases.Roles.Create
{
    public sealed class CreateRoleUseCase(
        IRoleRepository roleRepository
    ) : ICreateRoleUseCase
    {
        private readonly IRoleRepository _roleRepository = roleRepository;

        public async Task<Guid> Execute(CreateRoleRequest request, CancellationToken ct = default)
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
    }
}
