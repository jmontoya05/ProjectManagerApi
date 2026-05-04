namespace ProjectManager.Domain.Exceptions
{
    public abstract class DomainException(string message, string errorCode) : Exception(message)
    {
        public string ErrorCode { get; } = errorCode;
    }
}

