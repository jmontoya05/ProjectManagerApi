namespace ProjectManager.Application.Exceptions
{
    public class ForbiddenException : ApplicationException
    {
        public ForbiddenException(string message = "Access forbidden")
            : base(
                message,
                "FORBIDDEN",
                403)
        {
        }
    }
}
