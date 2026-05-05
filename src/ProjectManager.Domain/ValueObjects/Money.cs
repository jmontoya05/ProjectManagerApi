using ProjectManager.Domain.Exceptions;

namespace ProjectManager.Domain.ValueObjects
{
    public sealed class Money
    {
        public decimal Amount { get; }
        public string Currency { get; }

        private Money(decimal amount, string currency)
        {
            Amount = amount;
            Currency = currency;
        }

        public static Money Create(decimal amount, string currency)
        {
            if (amount < 0)
                throw new InvalidMoneyException("Amount cannot be negative");

            if (string.IsNullOrWhiteSpace(currency) || currency.Length != 3)
                throw new InvalidMoneyException("Currency must be a valid 3-letter ISO code");

            return new Money(amount, currency.ToUpper());
        }

        public Money Add(Money other)
        {
            if (Currency != other.Currency)
                throw new InvalidMoneyException($"Cannot add different currencies: {Currency} and {other.Currency}");

            return new Money(Amount + other.Amount, Currency);
        }

        public Money Subtract(Money other)
        {
            if (Currency != other.Currency)
                throw new InvalidMoneyException($"Cannot subtract different currencies: {Currency} and {other.Currency}");

            var result = Amount - other.Amount;
            if (result < 0)
                throw new InvalidMoneyException("Result cannot be negative");

            return new Money(result, Currency);
        }

        public Money Multiply(decimal factor)
        {
            if (factor < 0)
                throw new InvalidMoneyException("Multiplication factor cannot be negative");

            return new Money(Amount * factor, Currency);
        }

        public override string ToString() => $"{Amount:F2} {Currency}";

        public override bool Equals(object? obj) =>
            obj is Money money && Amount == money.Amount && Currency == money.Currency;

        public override int GetHashCode() =>
            HashCode.Combine(Amount, Currency);
    }
}
