namespace ProjectManager.Domain.Exceptions
{
    public sealed class InvalidWorkItemStatusTransitionException(string currentStatus, string targetStatus)
        : DomainException($"Cannot transition work item from '{currentStatus}' to '{targetStatus}'.",
            "INVALID_WORK_ITEM_STATUS_TRANSITION");
}

