namespace ProjectManager.Domain.Exceptions
{
    public sealed class InvalidEmailException(string email)
        : DomainException($"Email '{email}' is not in a valid format.", "INVALID_EMAIL");
}

