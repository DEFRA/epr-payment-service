namespace EPR.Payment.Service.Common.Constants.RegistrationFees.Exceptions
{
    public static class ProducerFeesRepositoryConstants
    {
        public const string Status500InternalServerError = "Internal server error";
        public const string InvalidProducerTypeOrRegulatorError = "Base fee for producer type '{0}' and regulator '{1}' not found.";
        public const string InvalidSubsidiariesFeeOrRegulatorError = "Subsidiaries fee for '{0}' and regulator '{1}' not found.";
        public const string InvalidLateFeeError = "Producer Late Fee record not found for regulator: '{0}'";
        public const string InvalidOnlineMarketRegulatorError = "Producer Online Market Fee record not found for regulator: '{0}'";
    }
}
