using ProjectManager.Domain.Enums;
using ProjectManager.Domain.Exceptions;

namespace ProjectManager.Domain.Entities
{
    public class Project : EntityBase
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public ProjectStatus Status { get; set; } = ProjectStatus.Planning;
        public Guid OrganizationId { get; set; }
        public Guid OwnerId { get; init; }
        // Navigation properties
        public virtual Organization Organization { get; init; } = null!;
        public virtual User Owner { get; init; } = null!;
        public virtual ICollection<ProjectMembership> ProjectMemberships { get; init; } = [];
        public virtual ICollection<WorkItem> WorkItems { get; init; } = [];
        
        public void TransitionStatus(ProjectStatus newStatus)
        {
            if (Status == newStatus)
                return;
            
            var validTransitions = new Dictionary<ProjectStatus, ProjectStatus[]>
            {
                { ProjectStatus.Planning, [ProjectStatus.Active, ProjectStatus.Cancelled] },
                { ProjectStatus.Active, [ProjectStatus.OnHold, ProjectStatus.Completed, ProjectStatus.Cancelled] },
                { ProjectStatus.OnHold, [ProjectStatus.Active, ProjectStatus.Cancelled] },
                { ProjectStatus.Completed, [] },
                { ProjectStatus.Cancelled, [] }
            };

            if (!validTransitions.TryGetValue(Status, out var allowed) || !allowed.Contains(newStatus))
            {
                throw new InvalidProjectStatusTransitionException(Status.ToString(), newStatus.ToString());
            }

            Status = newStatus;
            UpdatedAt = DateTime.UtcNow;
        }
        
        public bool IsActive => Status == ProjectStatus.Active;
        
        public bool IsFinalized => Status == ProjectStatus.Completed || Status == ProjectStatus.Cancelled;
        
        public bool IsOwnedBy(Guid userId) => OwnerId == userId;
        
        public IEnumerable<WorkItem> GetActiveWorkItems() =>
            WorkItems.Where(wi => wi.Status != WorkItemStatus.Done && wi.DeletedAt == null);
    }
}
