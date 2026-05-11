namespace ProjectManager.Domain.Exceptions
{
    public sealed class CannotAddWorkItemToCompletedSprintException(Guid sprintId, Guid workItemId) : DomainException(
        $"Cannot add work item '{workItemId}' to completed sprint '{sprintId}'.", "CANNOT_ADD_TO_COMPLETED_SPRINT");
}
