namespace ProjectManager.Application.Exceptions
{
    public class ConflictException : ApplicationException
    {
        public ConflictException(string message, string? entityType = null)
            : base(
                message,
                "CONFLICT",
                409,
                details: entityType != null ? new Dictionary<string, object?> 
                { 
                    ["entityType"] = entityType
                } : null)
        {
        }
    }
}
