namespace ProjectManager.Domain.Exceptions
{
    public sealed class InvalidPercentageException(decimal value)
        : DomainException($"Percentage value '{value}' must be between 0 and 100.", "INVALID_PERCENTAGE");
}
