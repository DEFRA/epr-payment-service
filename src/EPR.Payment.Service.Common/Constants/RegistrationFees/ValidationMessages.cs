namespace EPR.Payment.Service.Common.Constants.RegistrationFees
{
    public static class ValidationMessages
    {
        public const string ProducerTypeInvalid = "ProducerType must be one of the following: ";
        public const string NumberOfSubsidiariesRange = "Number of subsidiaries must be greater than or equal to 0";
        public const string NumberOfSubsidiariesRequiredWhenProducerTypeEmpty = "Number of subsidiaries must be greater than 0 when ProducerType is empty.";
        public const string RegulatorRequired = "Regulator is required.";
        public const string RegulatorInvalid = "Invalid Regulator.";
        public const string ApplicationReferenceNumberRequired = "Application Reference Number is required.";
        public const string NoOfSubsidiariesOnlineMarketplaceRange = "No Of Subsidiaries Online Marketplace must be greater than or equal to 0";
    }
}
