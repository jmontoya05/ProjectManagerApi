namespace ProjectManager.Domain.Entities
{
    public class Invitation : EntityBase
    {
        public string Email { get; init; } = null!;
        public Guid OrganizationId { get; init; }
        public Guid RoleId { get; init; }
        public string Token { get; init; } = null!;
        public DateTime ExpiresAt { get; init; }
        public bool Accepted { get; set; }
    }
}
