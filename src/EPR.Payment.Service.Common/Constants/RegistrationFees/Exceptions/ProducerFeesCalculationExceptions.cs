namespace EPR.Payment.Service.Common.Constants.RegistrationFees.Exceptions
{
    public static class ProducerFeesCalculationExceptions
    {
        public const string RegulatorMissing = "Regulator must be provided.";
        public const string InvalidSubsidiariesNumber = "NumberOfSubsidiaries must be greater than 0 and Regulator must be provided.";
        public const string RegulatorTypeInvalid = "Invalid regulator type: {0}.";
        public const string RegulatorCannotBeNullOrEmpty = "Regulator cannot be null or empty.";
        public const string FeeCalculationError = "An error occurred while calculating fees.";
    }
}