namespace ProjectManager.Application.Exceptions
{
    public class UnauthorizedException : ApplicationException
    {
        public UnauthorizedException(string message = "Authentication required")
            : base(
                message,
                "UNAUTHORIZED",
                401)
        {
        }
    }
}
