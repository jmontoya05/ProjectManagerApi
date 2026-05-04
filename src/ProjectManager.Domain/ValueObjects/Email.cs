using System.Text.RegularExpressions;
using ProjectManager.Domain.Exceptions;

namespace ProjectManager.Domain.ValueObjects
{
    public sealed partial class Email
    {
        private string Value { get; }

        private Email(string value)
        {
            Value = value;
        }

        public static Email Create(string email)
        {
            if (string.IsNullOrWhiteSpace(email) || !IsValidEmail(email))
                throw new InvalidEmailException(email);

            return new Email(email);
        }

        private static bool IsValidEmail(string email)
        {
            return MyRegex().IsMatch(email);
        }

        public override string ToString() => Value;

        public override bool Equals(object? obj) =>
            obj is Email email && Value == email.Value;

        public override int GetHashCode() => Value.GetHashCode();
        
        [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")]
        private static partial Regex MyRegex();
    }
}
