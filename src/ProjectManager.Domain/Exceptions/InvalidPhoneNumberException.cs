namespace ProjectManager.Domain.Exceptions
{
    public sealed class InvalidPhoneNumberException(string phoneNumber)
        : DomainException($"Phone number '{phoneNumber}' is not valid.", "INVALID_PHONE_NUMBER");
}
