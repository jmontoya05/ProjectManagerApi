using ProjectManager.Domain.Enums;
using ProjectManager.Domain.Exceptions;
using ProjectManager.Domain.ValueObjects;

namespace ProjectManager.Domain.Entities
{
    public class Sprint : EntityBase
    {
        public string Name { get; set; } = null!;
        public string? Goal { get; set; }
        public Guid ProjectId { get; init; }
        public SprintStatus Status { get; set; } = SprintStatus.Planning;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? Capacity { get; set; }
        // Navigation properties
        public virtual Project Project { get; init; } = null!;
        public virtual ICollection<SprintWorkItem> WorkItems { get; init; } = [];
        
        public void Start()
        {
            if (Status != SprintStatus.Planning)
                throw new InvalidSprintStatusTransitionException(Status.ToString(), nameof(SprintStatus.Active));

            if (!StartDate.HasValue || !EndDate.HasValue)
                throw new InvalidOperationException("Sprint must have start and end dates to begin.");

            if (StartDate.Value > EndDate.Value)
                throw new InvalidOperationException("Sprint start date cannot be after end date.");

            Status = SprintStatus.Active;
            UpdatedAt = DateTime.UtcNow;
        }
        
        public void Complete()
        {
            if (Status != SprintStatus.Active)
                throw new InvalidSprintStatusTransitionException(Status.ToString(), nameof(SprintStatus.Completed));

            Status = SprintStatus.Completed;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Cancel()
        {
            if (Status == SprintStatus.Completed)
                throw new InvalidSprintStatusTransitionException(Status.ToString(), nameof(SprintStatus.Cancelled));

            Status = SprintStatus.Cancelled;
            UpdatedAt = DateTime.UtcNow;
        }
        
        public void AddWorkItem(WorkItem workItem)
        {
            if (Status is SprintStatus.Completed or SprintStatus.Cancelled)
                throw new CannotAddWorkItemToCompletedSprintException(Id, workItem.Id);

            if (Capacity.HasValue)
            {
                var currentCapacity = CalculateCurrentCapacity();
                if (currentCapacity + (workItem.StoryPoints ?? 0) > Capacity.Value)
                    throw new SprintCapacityExceededException(Id, currentCapacity + (workItem.StoryPoints ?? 0), Capacity.Value);
            }
            
            if (WorkItems.Any(swi => swi.WorkItemId == workItem.Id && swi.DeletedAt == null))
                return;

            var sprintWorkItem = new SprintWorkItem
            {
                SprintId = Id,
                WorkItemId = workItem.Id,
                OrderIndex = WorkItems.Count + 1,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            WorkItems.Add(sprintWorkItem);
            UpdatedAt = DateTime.UtcNow;
        }
        
        public void RemoveWorkItem(Guid workItemId)
        {
            var sprintWorkItem = WorkItems.FirstOrDefault(swi => swi.WorkItemId == workItemId && swi.DeletedAt == null);
            if (sprintWorkItem == null) return;
            sprintWorkItem.DeletedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }
        
        public int CalculateVelocity() =>
            WorkItems
                .Where(swi => swi.DeletedAt == null && swi.WorkItem.Status == WorkItemStatus.Done)
                .Sum(swi => swi.WorkItem.StoryPoints ?? 0);
        
        public int CalculateCurrentCapacity() =>
            WorkItems
                .Where(swi => swi.DeletedAt == null)
                .Sum(swi => swi.WorkItem.StoryPoints ?? 0);
        
        public int GetActiveWorkItemCount() =>
            WorkItems.Count(swi => swi.DeletedAt == null);
        
        public bool IsActive => Status == SprintStatus.Active;
        
        public bool IsFinalized => Status is SprintStatus.Completed or SprintStatus.Cancelled;
    }
}
