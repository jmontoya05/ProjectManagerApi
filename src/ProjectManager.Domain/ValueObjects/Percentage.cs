using ProjectManager.Domain.Exceptions;

namespace ProjectManager.Domain.ValueObjects
{
    public sealed class Percentage
    {
        public decimal Value { get; }

        private Percentage(decimal value)
        {
            Value = value;
        }

        public static Percentage Create(decimal value)
        {
            if (value < 0 || value > 100)
                throw new InvalidPercentageException(value);

            return new Percentage(decimal.Round(value, 2));
        }

        public static Percentage Zero => new(0);
        public static Percentage HalfComplete => new(50);
        public static Percentage Complete => new(100);

        public bool IsZero => Value == 0;
        public bool IsComplete => Value == 100;
        public bool IsPartial => Value > 0 && Value < 100;

        public override string ToString() => $"{Value}%";

        public override bool Equals(object? obj) =>
            obj is Percentage pct && Value == pct.Value;

        public override int GetHashCode() =>
            Value.GetHashCode();
    }
}
