namespace ProjectManager.Domain.Entities
{
    public class Team : EntityBase
    {
        public string Name { get; set; } = null!;
        public Guid OrganizationId { get; set; }
        //Navigation Properties
        public virtual Organization Organization { get; init; } = null!;
        public virtual ICollection<TeamMember> Members { get; init; } = [];

        public void AddMember(TeamMember member)
        {
            if (member.TeamId != Id)
                throw new InvalidOperationException("Member does not belong to this team.");

            Members.Add(member);
            UpdatedAt = DateTime.UtcNow;
        }

        public void RemoveMember(Guid memberId)
        {
            var member = Members.FirstOrDefault(m => m.Id == memberId);
            if (member == null) return;
            Members.Remove(member);
            UpdatedAt = DateTime.UtcNow;
        }
        
        public int GetMemberCount() => Members.Count(m => m.DeletedAt == null);
    }
}
