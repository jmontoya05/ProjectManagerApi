namespace ProjectManager.Api.Exceptions.ApplicationExceptions
{
    /// <summary>
    /// Exception thrown when there is a conflict, typically due to a duplicate entity.
    /// Maps to HTTP 409 Conflict.
    /// </summary>
    public class ConflictException : ApplicationException
    {
        /// <summary>
        /// Initializes a new instance of the ConflictException class.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="entityType">The type of entity that caused the conflict (e.g., "User", "Organization").</param>
        public ConflictException(string message, string? entityType = null)
            : base(
                message,
                "CONFLICT",
                StatusCodes.Status409Conflict,
                details: entityType != null
                    ? new Dictionary<string, object?> 
                    { 
                        ["entityType"] = entityType
                    }
                    : null)
        {
        }
    }
}
