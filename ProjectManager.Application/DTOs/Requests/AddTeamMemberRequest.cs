namespace ProjectManager.Application.DTOs.Requests
{
    public sealed class AddTeamMemberRequest
    {
        public required Guid UserId { get; set; }
    }
}
