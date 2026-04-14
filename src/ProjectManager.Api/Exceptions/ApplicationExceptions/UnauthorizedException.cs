namespace ProjectManager.Api.Exceptions.ApplicationExceptions
{
    /// <summary>
    /// Exception thrown when a request is not authenticated.
    /// Maps to HTTP 401 Unauthorized.
    /// </summary>
    public sealed class UnauthorizedException : ApplicationException
    {
        /// <summary>
        /// Initializes a new instance of the UnauthorizedException class.
        /// </summary>
        /// <param name="message">The error message. Defaults to "Authentication required".</param>
        public UnauthorizedException(string message = "Authentication required")
            : base(
                message,
                "UNAUTHORIZED",
                StatusCodes.Status401Unauthorized)
        {
        }
    }
}
