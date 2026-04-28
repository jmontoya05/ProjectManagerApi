using System;

namespace ProjectManager.Domain.Entities
{
    public class Invitation
    {
        public Guid Id { get; init; }
        public string Email { get; init; } = null!;
        public Guid OrganizationId { get; init; }
        public Guid RoleId { get; init; }
        public string Token { get; init; } = null!;
        public DateTime ExpiresAt { get; init; }
        public bool Accepted { get; set; }
        public DateTime CreatedAt { get; init; }
        public Guid? CreatedBy { get; init; }
    }
}
