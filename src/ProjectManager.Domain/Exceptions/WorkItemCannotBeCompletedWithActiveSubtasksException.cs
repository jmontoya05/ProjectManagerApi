namespace ProjectManager.Domain.Exceptions
{
    public sealed class WorkItemCannotBeCompletedWithActiveSubtasksException(Guid workItemId, int activeSubtaskCount)
        : DomainException(
            $"Cannot complete work item '{workItemId}' while {activeSubtaskCount} subtask(s) are still active.",
            "WORK_ITEM_HAS_ACTIVE_SUBTASKS");
}
