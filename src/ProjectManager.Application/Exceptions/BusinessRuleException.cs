namespace ProjectManager.Application.Exceptions
{
    public class BusinessRuleException : ApplicationException
    {
        public BusinessRuleException(string message, string? ruleCode = null)
            : base(
                message,
                ruleCode ?? "BUSINESS_RULE_VIOLATION",
                422,
                details: ruleCode != null ? new Dictionary<string, object?>
                {
                    ["ruleCode"] = ruleCode
                } : null)
        {
        }
    }
}
