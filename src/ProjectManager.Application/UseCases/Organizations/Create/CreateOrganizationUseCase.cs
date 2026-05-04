using ProjectManager.Application.DTOs.Organizations;
using ProjectManager.Application.Ports;
using ProjectManager.Application.Exceptions;
using ProjectManager.Application.Services;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Enums;

namespace ProjectManager.Application.UseCases.Organizations.Create
{
    public sealed class CreateOrganizationUseCase(
        IOrganizationRepository organizationRepository,
        IUserRepository userRepository,
        IRoleRepository roleRepository,
        ITenantContext tenantContext
    ) : ICreateOrganizationUseCase
    {
        private readonly IOrganizationRepository _organizationRepository = organizationRepository;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IRoleRepository _roleRepository = roleRepository;
        private readonly ITenantContext _tenantContext = tenantContext;

        public async Task<Guid> Execute(CreateOrganizationRequest request, CancellationToken ct = default)
        {
            var userId = _tenantContext.GetUserIdOrThrow();
            var user = await _userRepository.GetByIdAsync(userId, ct)
                ?? throw new NotFoundException("User not found.", "User", userId);
            
            if (!user.IsActive)
                throw new BusinessRuleException("User is not active.", "USER_NOT_ACTIVE");

            var ownerRole = await _roleRepository.GetByNameAsync("OrgOwner", ct)
                ?? throw new NotFoundException("System role 'OrgOwner' not found.", "Role", "OrgOwner");

            var organization = new Organization
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Status = OrganizationStatus.Active,
                OwnerId = user.Id,
                CreatedBy = user.Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _organizationRepository.AddAsync(organization, ct);

            await _userRepository.AddMembershipAsync(new OrganizationMembership
            {
                Id = Guid.NewGuid(),
                OrganizationId = organization.Id,
                UserId = user.Id,
                RoleId = ownerRole.Id,
                CreatedAt = DateTime.UtcNow
            }, ct);

            return organization.Id;
        }
    }
}
