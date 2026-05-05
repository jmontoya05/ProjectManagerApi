using ProjectManager.Domain.Exceptions;

namespace ProjectManager.Domain.ValueObjects
{
    public sealed class Address
    {
        public string Street { get; }
        public string City { get; }
        public string State { get; }
        public string ZipCode { get; }
        public string Country { get; }

        private Address(string street, string city, string state, string zipCode, string country)
        {
            Street = street;
            City = city;
            State = state;
            ZipCode = zipCode;
            Country = country;
        }

        public static Address Create(string street, string city, string state, string zipCode, string country)
        {
            if (string.IsNullOrWhiteSpace(street))
                throw new InvalidAddressException("Street cannot be empty");
            if (string.IsNullOrWhiteSpace(city))
                throw new InvalidAddressException("City cannot be empty");
            if (string.IsNullOrWhiteSpace(state))
                throw new InvalidAddressException("State cannot be empty");
            if (string.IsNullOrWhiteSpace(zipCode))
                throw new InvalidAddressException("Zip code cannot be empty");
            if (string.IsNullOrWhiteSpace(country))
                throw new InvalidAddressException("Country cannot be empty");

            return new Address(street.Trim(), city.Trim(), state.Trim(), zipCode.Trim(), country.Trim());
        }

        public override string ToString() => $"{Street}, {City}, {State} {ZipCode}, {Country}";

        public override bool Equals(object? obj) =>
            obj is Address addr &&
            Street == addr.Street &&
            City == addr.City &&
            State == addr.State &&
            ZipCode == addr.ZipCode &&
            Country == addr.Country;

        public override int GetHashCode() =>
            HashCode.Combine(Street, City, State, ZipCode, Country);
    }
}
