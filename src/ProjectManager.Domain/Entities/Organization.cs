using ProjectManager.Domain.Enums;
using ProjectManager.Domain.ValueObjects;

namespace ProjectManager.Domain.Entities
{
    public class Organization : EntityBase
    {
        public string Name { get; set; } = null!;
        public OrganizationStatus Status { get; set; } = OrganizationStatus.Active;
        public Guid OwnerId { get; init; }
        public string? AddressStreet { get; set; }
        public string? AddressCity { get; set; }
        public string? AddressState { get; set; }
        public string? AddressZipCode { get; set; }
        public string? AddressCountry { get; set; }
        
        public Address? Address
        {
            get => HasAddress() ? Address.Create(AddressStreet!, AddressCity!, AddressState!, AddressZipCode!, AddressCountry!) : null;
            set
            {
                if (value == null)
                {
                    AddressStreet = null;
                    AddressCity = null;
                    AddressState = null;
                    AddressZipCode = null;
                    AddressCountry = null;
                }
                else
                {
                    AddressStreet = value.Street;
                    AddressCity = value.City;
                    AddressState = value.State;
                    AddressZipCode = value.ZipCode;
                    AddressCountry = value.Country;
                }
            }
        }
        
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
        
        public void SetAddress(string street, string city, string state, string zipCode, string country)
        {
            Address = Address.Create(street, city, state, zipCode, country);
            UpdatedAt = DateTime.UtcNow;
        }
        
        public void ClearAddress()
        {
            Address = null;
            UpdatedAt = DateTime.UtcNow;
        }

        private bool HasAddress() =>
            !string.IsNullOrEmpty(AddressStreet) &&
            !string.IsNullOrEmpty(AddressCity) &&
            !string.IsNullOrEmpty(AddressState) &&
            !string.IsNullOrEmpty(AddressZipCode) &&
            !string.IsNullOrEmpty(AddressCountry);
        
        public bool IsActive => Status == OrganizationStatus.Active;
        
        public bool IsOwnedBy(Guid userId) => OwnerId == userId;
    }
}
