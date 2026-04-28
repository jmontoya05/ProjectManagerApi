namespace ProjectManager.Domain.Entities
{
    public class User
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = null!;
        public string Email { get; init; } = null!;
        public string PasswordHash { get; init; } = null!;
        public string DisplayName { get; init; } = null!;
        public string Status { get; init; } = null!;
        public DateTime CreatedAt { get; init; }
        public Guid? CreatedBy { get; init; }
        public DateTime UpdatedAt { get; init; }
        public Guid? UpdatedBy { get; init; }
        //Navigation Properties
        public virtual ICollection<RefreshToken> RefreshTokens { get; init; } = [];
        public virtual ICollection<OrganizationMembership> OrganizationMemberships { get; init; } = [];
        public virtual ICollection<TeamMember> TeamMemberships { get; init; } = [];
        public virtual ICollection<ProjectMembership> ProjectMemberships { get; init; } = [];
    }
}
