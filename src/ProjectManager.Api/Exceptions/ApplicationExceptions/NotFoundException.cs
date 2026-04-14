namespace ProjectManager.Api.Exceptions.ApplicationExceptions
{
    /// <summary>
    /// Exception thrown when a requested entity is not found.
    /// Maps to HTTP 404 Not Found.
    /// </summary>
    public sealed class NotFoundException : ApplicationException
    {
        /// <summary>
        /// Initializes a new instance of the NotFoundException class.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="entityType">The type of entity that was not found (e.g., "User", "Project").</param>
        /// <param name="id">The identifier of the entity that was not found.</param>
        public NotFoundException(string message, string? entityType = null, object? id = null)
            : base(
                message,
                "ENTITY_NOT_FOUND",
                StatusCodes.Status404NotFound,
                details: entityType != null
                    ? new Dictionary<string, object?> 
                    { 
                        ["entityType"] = entityType,
                        ["id"] = id
                    }
                    : null)
        {
        }
    }
}
