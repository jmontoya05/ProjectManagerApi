namespace ProjectManager.Api.Exceptions.ApplicationExceptions
{
    /// <summary>
    /// Exception thrown when an authenticated request lacks sufficient permissions.
    /// Maps to HTTP 403 Forbidden.
    /// </summary>
    public sealed class ForbiddenException : ApplicationException
    {
        /// <summary>
        /// Initializes a new instance of the ForbiddenException class.
        /// </summary>
        /// <param name="message">The error message. Defaults to "Access forbidden".</param>
        public ForbiddenException(string message = "Access forbidden")
            : base(
                message,
                "FORBIDDEN",
                StatusCodes.Status403Forbidden)
        {
        }
    }
}
