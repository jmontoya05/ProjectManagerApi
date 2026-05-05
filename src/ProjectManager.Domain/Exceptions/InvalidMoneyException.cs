namespace ProjectManager.Domain.Exceptions
{
    public sealed class InvalidMoneyException(string reason)
        : DomainException($"Money value is invalid: {reason}", "INVALID_MONEY");
}
