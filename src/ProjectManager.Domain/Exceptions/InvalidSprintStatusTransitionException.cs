namespace ProjectManager.Domain.Exceptions;

public sealed class InvalidSprintStatusTransitionException(string from, string to) : DomainException(
    $"Cannot transition sprint from '{from}' to '{to}'.", "INVALID_SPRINT_STATUS_TRANSITION");