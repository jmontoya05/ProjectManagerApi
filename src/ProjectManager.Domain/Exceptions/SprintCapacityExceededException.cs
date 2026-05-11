namespace ProjectManager.Domain.Exceptions;

public sealed class SprintCapacityExceededException(Guid sprintId, int targetCapacity, int currentSprintCapacity)
    : DomainException(
        $"Sprint '{sprintId}' capacity exceeded. Target capacity: {targetCapacity}, Current capacity: {currentSprintCapacity}", "SPRINT_CAPACITY_EXCEEDED");