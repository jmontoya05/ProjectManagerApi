namespace ProjectManager.Application.Exceptions
{
    public class ApplicationException : Exception
    {
        public string ErrorCode { get; }
        public int? StatusCode { get; }
        public IDictionary<string, object?>? Details { get; }

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
