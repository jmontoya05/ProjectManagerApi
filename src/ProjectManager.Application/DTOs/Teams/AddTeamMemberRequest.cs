namespace ProjectManager.Application.DTOs.Teams
{
    public sealed class AddTeamMemberRequest
    {
        public required Guid UserId { get; set; }
    }
}
