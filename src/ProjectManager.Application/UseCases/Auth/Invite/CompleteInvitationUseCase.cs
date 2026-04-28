using ProjectManager.Application.DTOs.Auth;
using ProjectManager.Application.Ports;
using ProjectManager.Domain.Entities;

namespace ProjectManager.Application.UseCases.Auth.Invite
{
    public sealed class CompleteInvitationUseCase(
        IInvitationRepository invitationRepository, 
        IUserRepository userRepository
    ) : ICompleteInvitationUseCase
    {
        private readonly IInvitationRepository _invitationRepository = invitationRepository;
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<Guid> Execute(CompleteInvitationRequest request, CancellationToken ct = default)
        {
            var invitation = await _invitationRepository.GetByTokenAsync(request.InvitationToken, ct);
            if (invitation == null || invitation.ExpiresAt < DateTime.UtcNow || invitation.Accepted)
                throw new InvalidOperationException("Invalid or expired invitation.");

            var exists = await _userRepository.ExistsByEmailAsync(invitation.Email, ct);
            if (exists)
                throw new InvalidOperationException("User with this email already exists.");

            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Email = invitation.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                DisplayName = request.Name,
                Status = "Active",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            
            await _userRepository.AddAsync(user, ct);
            await _userRepository.AddMembershipAsync(new OrganizationMembership
            {
                Id = Guid.NewGuid(),
                OrganizationId = invitation.OrganizationId,
                UserId = user.Id,
                RoleId = invitation.RoleId,
                CreatedAt = DateTime.UtcNow
            }, ct);
            
            invitation.Accepted = true;
            await _invitationRepository.UpdateAsync(invitation, ct);

            return user.Id;
        }
    }
}
