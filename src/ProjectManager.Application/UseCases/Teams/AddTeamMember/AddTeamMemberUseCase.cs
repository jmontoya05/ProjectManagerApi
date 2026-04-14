using ProjectManager.Application.DTOs.Teams;
using ProjectManager.Application.Ports;
using ProjectManager.Application.Exceptions;
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
            _ = await _teamRepository.GetByIdAsync(teamId, ct)
                ?? throw new NotFoundException("Team not found", "Team", teamId);

            _ = await _userRepository.GetByIdAsync(request.UserId, ct)
                ?? throw new NotFoundException("Target user not found", "User", request.UserId);

            var alreadyMember = await _teamMemberRepository.IsUserInTeamAsync(request.UserId, teamId, ct);

            if (alreadyMember)
                throw new ConflictException("User is already a member of this team.", "TeamMember");

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
