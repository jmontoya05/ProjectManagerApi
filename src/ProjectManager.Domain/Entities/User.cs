using ProjectManager.Domain.Enums;

namespace ProjectManager.Domain.Entities
{
    public class User : EntityBase
    {
        public string Name { get; init; } = null!;
        public string Email { get; init; } = null!;
        public string PasswordHash { get; init; } = null!;
        public string DisplayName { get; init; } = null!;
        public UserStatus Status { get; set; } = UserStatus.Active;
        //Navigation Properties
        public virtual ICollection<RefreshToken> RefreshTokens { get; init; } = [];
        public virtual ICollection<OrganizationMembership> OrganizationMemberships { get; init; } = [];
        public virtual ICollection<TeamMember> TeamMemberships { get; init; } = [];
        public virtual ICollection<ProjectMembership> ProjectMemberships { get; init; } = [];
        
        public void Activate()
        {
            if (Status == UserStatus.Active)
                return;

            Status = UserStatus.Active;
            UpdatedAt = DateTime.UtcNow;
        }
        
        public void Deactivate()
        {
            if (Status == UserStatus.Inactive)
                return;

            Status = UserStatus.Inactive;
            UpdatedAt = DateTime.UtcNow;
        }
        
        public static void ValidateEmail(string email)
        {
            _ = ValueObjects.Email.Create(email);
        }
        
        public bool IsActive => Status == UserStatus.Active;
    }
}
