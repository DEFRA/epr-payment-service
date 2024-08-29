namespace EPR.Payment.Service.Common.Constants.RegistrationFees
{
    public static class ValidationMessages
    {
        public const string ProducerTypeInvalid = "ProducerType must be one of the following: ";
        public const string NumberOfSubsidiariesRange = "Number of subsidiaries must be between 0 and 100.";
        public const string NumberOfSubsidiariesRequiredWhenProducerTypeEmpty = "Number of subsidiaries must be greater than 0 when ProducerType is empty.";
        public const string RegulatorRequired = "Regulator is required.";
        public const string RegulatorInvalid = "Invalid Regulator.";
    }
}
