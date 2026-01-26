namespace ProjectManager.Application.UseCases.Teams.List
{
    public sealed class ListTeamsResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public Guid OrganizationId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
