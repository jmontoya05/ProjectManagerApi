namespace ProjectManager.Application.Exceptions
{
    public class EntityAlreadyExistsException : ConflictException
    {
        public EntityAlreadyExistsException(string message, string? entityType = null)
            : base(message, entityType)
        {
        }
    }
}
