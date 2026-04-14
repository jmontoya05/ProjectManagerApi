namespace ProjectManager.Application.Exceptions
{
    public class NotFoundException : ApplicationException
    {
        public NotFoundException(string message, string? entityType = null, object? id = null)
            : base(
                message,
                "ENTITY_NOT_FOUND",
                404,
                details: entityType != null ? new Dictionary<string, object?> 
                { 
                    ["entityType"] = entityType,
                    ["id"] = id
                } : null)
        {
        }
    }
}
