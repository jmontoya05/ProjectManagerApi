using ProjectManager.Domain.Enums;

namespace ProjectManager.Domain.Entities
{
    public class Organization : EntityBase
    {
        public string Name { get; set; } = null!;
        public OrganizationStatus Status { get; set; } = OrganizationStatus.Active;
        public Guid OwnerId { get; init; }
        
        //Navigation properties
        public virtual User Owner { get; init; } = null!;
        public virtual ICollection<OrganizationMembership> OrganizationMemberships { get; init; } = [];
        public virtual ICollection<Team> Teams { get; init; } = [];
        public virtual ICollection<Project> Projects { get; init; } = [];
        public virtual ICollection<Role> Roles { get; init; } = [];

        public void Activate()
        {
            if (Status == OrganizationStatus.Active)
                return;

            Status = OrganizationStatus.Active;
            UpdatedAt = DateTime.UtcNow;
        }
        
        public void Deactivate()
        {
            if (Status == OrganizationStatus.Inactive)
                return;

            Status = OrganizationStatus.Inactive;
            UpdatedAt = DateTime.UtcNow;
        }
        
        public bool IsActive => Status == OrganizationStatus.Active;
        
        public bool IsOwnedBy(Guid userId) => OwnerId == userId;
    }
}
