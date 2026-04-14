namespace ProjectManager.Application.Exceptions
{
    public class ValidationException : ApplicationException
    {
        public ValidationException(IDictionary<string, string[]> errors)
            : base(
                "One or more validation errors occurred.",
                "VALIDATION_ERROR",
                400,
                details: new Dictionary<string, object?> { ["errors"] = errors })
        {
        }
    }
}
