namespace ProjectManager.Api.Exceptions.ApplicationExceptions
{
    /// <summary>
    /// Exception thrown when validation of request data fails.
    /// Maps to HTTP 400 Bad Request.
    /// </summary>
    public sealed class ValidationException : ApplicationException
    {
        /// <summary>
        /// Initializes a new instance of the ValidationException class.
        /// </summary>
        /// <param name="errors">A dictionary mapping property names to arrays of validation error messages.</param>
        public ValidationException(IDictionary<string, string[]> errors)
            : base(
                "One or more validation errors occurred.",
                "VALIDATION_ERROR",
                StatusCodes.Status400BadRequest,
                details: new Dictionary<string, object?> { ["errors"] = errors })
        {
        }
    }
}
