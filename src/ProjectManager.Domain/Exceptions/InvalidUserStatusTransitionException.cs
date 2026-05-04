namespace ProjectManager.Domain.Exceptions
{
    public sealed class InvalidUserStatusTransitionException(string currentStatus, string targetStatus)
        : DomainException($"Cannot transition user from '{currentStatus}' to '{targetStatus}'.",
            "INVALID_USER_STATUS_TRANSITION");
}

