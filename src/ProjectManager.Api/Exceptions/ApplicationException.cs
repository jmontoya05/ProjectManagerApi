namespace ProjectManager.Api.Exceptions
{
    /// <summary>
    /// Base exception class for all application-specific exceptions.
    /// Provides structured error information including error codes, HTTP status codes, and additional details.
    /// </summary>
    public class ApplicationException : Exception
    {
        /// <summary>
        /// Gets the error code that identifies the type of error.
        /// </summary>
        public string ErrorCode { get; }

        /// <summary>
        /// Gets the HTTP status code associated with this exception.
        /// </summary>
        public int? StatusCode { get; }

        /// <summary>
        /// Gets additional details about the error.
        /// </summary>
        public IDictionary<string, object?>? Details { get; }

        /// <summary>
        /// Initializes a new instance of the ApplicationException class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="errorCode">The error code that identifies the type of error.</param>
        /// <param name="statusCode">The HTTP status code associated with this exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        /// <param name="details">Additional details about the error.</param>
        public ApplicationException(
            string message,
            string errorCode,
            int? statusCode = null,
            Exception? innerException = null,
            IDictionary<string, object?>? details = null)
            : base(message, innerException)
        {
            ErrorCode = errorCode;
            StatusCode = statusCode;
            Details = details;
        }
    }
}
