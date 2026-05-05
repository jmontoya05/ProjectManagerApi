using System.Text.RegularExpressions;
using ProjectManager.Domain.Exceptions;

namespace ProjectManager.Domain.ValueObjects
{
    public sealed partial class PhoneNumber(string value)
    {
        public string Value { get; } = value;

        public static PhoneNumber Create(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                throw new InvalidPhoneNumberException(phoneNumber);
            
            var cleaned = MyRegex().Replace(phoneNumber, "");
            
            if (!PhoneRegex().IsMatch(cleaned))
                throw new InvalidPhoneNumberException(phoneNumber);

            return new PhoneNumber(phoneNumber.Trim());
        }

        public override string ToString() => Value;

        public override bool Equals(object? obj) =>
            obj is PhoneNumber phone && Value == phone.Value;

        public override int GetHashCode() => Value.GetHashCode();
        
        [GeneratedRegex(@"[\s\-\(\)]")]
        private static partial Regex MyRegex();
        
        [GeneratedRegex(@"^\+?\d{7,15}$")]
        private static partial Regex PhoneRegex();
    }
}


