using ProjectManager.Domain.Enums;
using ProjectManager.Domain.Exceptions;

namespace ProjectManager.Domain.Entities
{
    public class WorkItem : EntityBase
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public WorkItemType Type { get; init; } = WorkItemType.Task;
        public WorkItemPriority Priority { get; set; } = WorkItemPriority.Medium;
        public WorkItemStatus Status { get; set; } = WorkItemStatus.Backlog;
        public int? StoryPoints { get; set; }
        public int? TimeEstimateMinutes { get; set; }
        public Guid ProjectId { get; init; }
        public Guid? ParentWorkItemId { get; init; }
        public Guid? AssigneeId { get; set; }
        public Guid? TeamId { get; set; }
        // Navigation
        public virtual Project Project { get; init; } = null!;
        public virtual WorkItem? ParentWorkItem { get; init; }
        public virtual ICollection<WorkItem> Subtasks { get; init; } = [];
        public virtual User? Assignee { get; set; }
        public virtual Team? Team { get; set; }
        
        public void TransitionStatus(WorkItemStatus newStatus)
        {
            if (Status == newStatus)
                return;
            
            var validTransitions = new Dictionary<WorkItemStatus, WorkItemStatus[]>
            {
                { WorkItemStatus.Backlog, [WorkItemStatus.InProgress] },
                { WorkItemStatus.InProgress, [WorkItemStatus.Backlog, WorkItemStatus.Done] },
                { WorkItemStatus.Done, [WorkItemStatus.InProgress] }
            };

            if (!validTransitions.TryGetValue(Status, out var allowed) || !allowed.Contains(newStatus))
            {
                throw new InvalidWorkItemStatusTransitionException(Status.ToString(), newStatus.ToString());
            }
            
            if (newStatus == WorkItemStatus.Done)
            {
                var activeSubtasks = Subtasks.Where(s => s.Status != WorkItemStatus.Done && s.DeletedAt == null).ToList();
                if (activeSubtasks.Count > 0)
                {
                    throw new WorkItemCannotBeCompletedWithActiveSubtasksException(Id, activeSubtasks.Count);
                }
            }

            Status = newStatus;
            UpdatedAt = DateTime.UtcNow;
        }
        
        public void AssignTo(Guid userId)
        {
            AssigneeId = userId;
            UpdatedAt = DateTime.UtcNow;
        }
        
        public void Unassign()
        {
            AssigneeId = null;
            UpdatedAt = DateTime.UtcNow;
        }
        
        public void AssignToTeam(Guid teamId)
        {
            TeamId = teamId;
            UpdatedAt = DateTime.UtcNow;
        }
        
        public void AddSubtask(WorkItem subtask)
        {
            if (subtask.ParentWorkItemId != Id)
                throw new InvalidOperationException("Subtask does not belong to this work item.");

            Subtasks.Add(subtask);
            UpdatedAt = DateTime.UtcNow;
        }
        
        public int GetActiveSubtaskCount() =>
            Subtasks.Count(s => s.Status != WorkItemStatus.Done && s.DeletedAt == null);
        
        public bool IsCompleted => Status == WorkItemStatus.Done;
        
        public bool IsInProgress => Status == WorkItemStatus.InProgress;

        public bool IsAssigned => AssigneeId.HasValue;
        
        public bool IsTeamAssigned => TeamId.HasValue;

        public bool IsParent => Subtasks.Count > 0;
    }
}
