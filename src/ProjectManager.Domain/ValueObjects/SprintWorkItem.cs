using ProjectManager.Domain.Entities;

namespace ProjectManager.Domain.ValueObjects
{
    public class SprintWorkItem : EntityBase
    {
        public Guid SprintId { get; init; }
        public Guid WorkItemId { get; init; }
        public int OrderIndex { get; set; }
        // Navigation properties
        public virtual Sprint Sprint { get; init; } = null!;
        public virtual WorkItem WorkItem { get; init; } = null!;
    }
}
