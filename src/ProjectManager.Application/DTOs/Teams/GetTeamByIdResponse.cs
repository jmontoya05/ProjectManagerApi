namespace ProjectManager.Application.DTOs.Teams
{
    public sealed class GetTeamByIdResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public Guid OrganizationId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
