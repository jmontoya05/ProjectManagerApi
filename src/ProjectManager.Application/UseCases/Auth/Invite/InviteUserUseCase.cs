using ProjectManager.Application.DTOs.Auth;
using ProjectManager.Application.Ports;
using ProjectManager.Domain.Entities;
using System.Security.Cryptography;
using System.Text;
using ProjectManager.Application.Services;

namespace ProjectManager.Application.UseCases.Auth.Invite
{
    public sealed class InviteUserUseCase(
        IInvitationRepository invitationRepository, 
        IUserRepository userRepository,
        IEmailService emailService
    ) : IInviteUserUseCase
    {
        private readonly IInvitationRepository _invitationRepository = invitationRepository;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IEmailService _emailService = emailService;

        public async Task<InviteUserResponse> Execute(InviteUserRequest request, Guid adminUserId, CancellationToken ct = default)
        {
            var exists = await _userRepository.ExistsByEmailAsync(request.Email, ct);
            if (exists)
                throw new InvalidOperationException("User with this email already exists.");
            
            var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(48));
            var expiresAt = DateTime.UtcNow.AddDays(3);

            var invitation = new Invitation
            {
                Id = Guid.NewGuid(),
                Email = request.Email,
                OrganizationId = request.OrganizationId,
                RoleId = request.RoleId,
                Token = token,
                ExpiresAt = expiresAt,
                Accepted = false,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = adminUserId
            };

            await _invitationRepository.AddAsync(invitation, ct);
            await _emailService.SendInvitationEmailAsync(request.Email, token);

            return new InviteUserResponse
            {
                InvitationToken = token,
                Email = request.Email,
                ExpiresAt = expiresAt
            };
        }
    }
}
