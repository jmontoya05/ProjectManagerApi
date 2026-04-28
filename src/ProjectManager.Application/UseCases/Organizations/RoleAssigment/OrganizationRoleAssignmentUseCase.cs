using ProjectManager.Application.DTOs.Organizations;
using ProjectManager.Application.Ports;
using ProjectManager.Domain.Entities;

namespace ProjectManager.Application.UseCases.Organizations.RoleAssigment
{
    public sealed class OrganizationRoleAssignmentUseCase(
        IUserRepository userRepository
    ) : IOrganizationRoleAssignmentUseCase
    {
        private readonly IUserRepository _userRepository = userRepository;

        public async Task AssignRoleAsync(AssignOrganizationRoleRequest request, CancellationToken ct = default)
        {
            await _userRepository.RemoveMembershipAsync(request.UserId, request.OrganizationId, ct);
            var membership = new OrganizationMembership
            {
                Id = Guid.NewGuid(),
                OrganizationId = request.OrganizationId,
                UserId = request.UserId,
                RoleId = request.RoleId,
                CreatedAt = DateTime.UtcNow
            };
            await _userRepository.AddMembershipAsync(membership, ct);
        }

        public async Task RemoveRoleAsync(RemoveOrganizationRoleRequest request, CancellationToken ct = default)
        {
            await _userRepository.RemoveMembershipAsync(request.UserId, request.OrganizationId, ct);
        }
    }
}
