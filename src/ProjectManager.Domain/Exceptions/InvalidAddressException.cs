namespace ProjectManager.Domain.Exceptions
{
    public sealed class InvalidAddressException(string reason)
        : DomainException($"Address is invalid: {reason}", "INVALID_ADDRESS");
}
