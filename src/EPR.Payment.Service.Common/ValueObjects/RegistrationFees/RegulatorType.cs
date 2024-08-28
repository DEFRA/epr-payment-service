namespace EPR.Payment.Service.Common.ValueObjects.RegistrationFees
{
    public sealed class RegulatorType
    {
        // Predefined valid regulator types
        private static readonly HashSet<string> ValidRegulators = new(StringComparer.OrdinalIgnoreCase)
        {
            "GB-ENG", // England
            "GB-SCT", // Scotland
            "GB-WLS", // Wales
            "GB-NIR"  // Northern Ireland
        };

        // Property to store the actual regulator type string
        public string Value { get; }

        // Private constructor to enforce factory method usage
        private RegulatorType(string value)
        {
            Value = value;
        }

        // Factory method for creating a valid RegulatorType instance
        public static RegulatorType Create(string regulator)
        {
            if (string.IsNullOrWhiteSpace(regulator))
            {
                throw new ArgumentException("Regulator cannot be null or empty.", nameof(regulator));
            }

            // If regulator is valid, create a new instance; otherwise, throw an exception
            if (!ValidRegulators.Contains(regulator))
            {
                throw new ArgumentException($"Invalid regulator type: {regulator}.", nameof(regulator));
            }

            return new RegulatorType(regulator);
        }

        // Override equality members to support value equality
        public override bool Equals(object? obj) =>
            obj is RegulatorType other && string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);

        public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Value);

        public override string ToString() => Value;

        // Public static properties for common regulator types
        public static RegulatorType GBEng => Create("GB-ENG");
        public static RegulatorType GBSct => Create("GB-SCT");
        public static RegulatorType GBWls => Create("GB-WLS");
        public static RegulatorType GBNir => Create("GB-NIR");
    }
}
