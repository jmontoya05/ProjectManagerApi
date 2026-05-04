namespace ProjectManager.Domain.Exceptions
{
    public sealed class InvalidProjectStatusTransitionException(string currentStatus, string targetStatus)
        : DomainException($"Cannot transition project from '{currentStatus}' to '{targetStatus}'.",
            "INVALID_PROJECT_STATUS_TRANSITION");
}

