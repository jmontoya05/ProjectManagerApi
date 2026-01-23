using ProjectManager.Application.DTOs.Requests;
using ProjectManager.Application.Ports;
using ProjectManager.Domain.Entities;


namespace ProjectManager.Application.UseCases.Teams.AddTeamMember
{
    public sealed class AddTeamMemberUseCase(ITeamMemberRepository teamMemberRepository, ITeamRepository teamRepository, IUserRepository userRepository) : IAddTeamMemberUseCase
    {
        private readonly ITeamMemberRepository _teamMemberRepository = teamMemberRepository;
        private readonly ITeamRepository _teamRepository = teamRepository;
        private readonly IUserRepository _userRepository = userRepository;

        public async Task Execute(AddTeamMemberRequest request, Guid teamId, Guid currentUserId, CancellationToken ct = default)
        {
            var team = await _teamRepository.GetByIdAsync(teamId, ct)
                ?? throw new InvalidOperationException("Team not found.");

            var targetUser = await _userRepository.UserBelongsToOrganizationAsync(request.UserId, team.OrganizationId, ct);

            if (!targetUser)
                throw new InvalidOperationException("Target user is not a member of this organization.");

            var alreadyMember = await _teamMemberRepository.IsUserInTeamAsync(request.UserId, teamId, ct);

            if (alreadyMember)
                throw new InvalidOperationException("User is already a member of this team.");

            var member = new TeamMember
            {
                Id = Guid.NewGuid(),
                TeamId = teamId,
                UserId = request.UserId,
                JoinedAt = DateTime.UtcNow
            };

            await _teamMemberRepository.AddMemberAsync(member, ct);
        }
    }
}
