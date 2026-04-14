namespace ProjectManager.Api.Exceptions.ApplicationExceptions
{
    /// <summary>
    /// Exception thrown when an entity already exists and cannot be created again.
    /// Inherits from ConflictException and maps to HTTP 409 Conflict.
    /// </summary>
    public sealed class EntityAlreadyExistsException : ConflictException
    {
        /// <summary>
        /// Initializes a new instance of the EntityAlreadyExistsException class.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="entityType">The type of entity that already exists (e.g., "User", "Organization").</param>
        public EntityAlreadyExistsException(string message, string? entityType = null)
            : base(message, entityType)
        {
        }
    }
}
