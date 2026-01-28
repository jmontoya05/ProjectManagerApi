namespace ProjectManager.Application.UseCases.Teams.AddTeamMember
{
    public interface IAddTeamMemberUseCase
    {
        Task Execute(AddTeamMemberRequest request, Guid teamId, Guid currentUserId, CancellationToken ct = default);
    }
}
