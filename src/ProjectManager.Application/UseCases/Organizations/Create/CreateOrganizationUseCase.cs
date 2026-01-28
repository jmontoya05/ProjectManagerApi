using ProjectManager.Application.Ports;
using ProjectManager.Domain.Entities;

namespace ProjectManager.Application.UseCases.Organizations.Create
{
    public sealed class CreateOrganizationUseCase(IOrganizationRepository organizationRepository, IUserRepository userRepository) : ICreateOrganizationUseCase
    {
        private readonly IOrganizationRepository _organizationRepository = organizationRepository;
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<Guid> Execute(CreateOrganizationRequest request, Guid userId, CancellationToken ct = default)
        {
            var user = await _userRepository.GetByIdAsync(userId, ct)
                ?? throw new InvalidOperationException("User not found.");

            if (user.Status != "Active")
                throw new InvalidOperationException("User is not active.");

            var organization = new Organization
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Status = "Active",
                OwnerId = user.Id,
                CreatedBy = user.Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _organizationRepository.AddAsync(organization, ct);

            var membership = new OrganizationMembership
            {
                Id = Guid.NewGuid(),
                OrganizationId = organization.Id,
                UserId = user.Id,
                CreatedAt = DateTime.UtcNow
            };

            await _userRepository.AddMembershipAsync(membership, "OrgOwner", ct);

            return organization.Id;
        }
    }
}
