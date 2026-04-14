namespace ProjectManager.Api.Exceptions.ApplicationExceptions
{
    /// <summary>
    /// Exception thrown when a business rule is violated.
    /// Maps to HTTP 422 Unprocessable Entity.
    /// </summary>
    public sealed class BusinessRuleException : ApplicationException
    {
        /// <summary>
        /// Initializes a new instance of the BusinessRuleException class.
        /// </summary>
        /// <param name="message">The error message describing the business rule violation.</param>
        /// <param name="ruleCode">Optional code identifying the specific business rule that was violated.</param>
        public BusinessRuleException(string message, string? ruleCode = null)
            : base(
                message,
                ruleCode ?? "BUSINESS_RULE_VIOLATION",
                StatusCodes.Status422UnprocessableEntity,
                details: ruleCode != null
                    ? new Dictionary<string, object?> 
                    { 
                        ["ruleCode"] = ruleCode
                    }
                    : null)
        {
        }
    }
}
