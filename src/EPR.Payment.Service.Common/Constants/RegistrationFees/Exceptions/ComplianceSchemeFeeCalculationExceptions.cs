namespace EPR.Payment.Service.Common.Constants.RegistrationFees.Exceptions
{
    public static class ComplianceSchemeFeeCalculationExceptions
    {
        public const string RegulatorMissing = "The regulator identifier is missing.";
        public const string BaseFeeCalculationError = "An error occurred while calculating the compliance scheme base fee.";
        public const string ProducerAndSubsidiaryFeeCalculationError = "An error occurred while calculating the compliance scheme producer and subsidiary fees.";
        public const string RetrievalError = "An error occurred while retrieving the compliance scheme base fee.";
        public const string InvalidComplianceSchemeOrRegulatorError = "Base fee for compliance scheme with regulator '{0}' not found.";
        public const string InvalidRegulatorError = "Base fee for compliance scheme with regulator '{0}' not found.";
        public const string BaseFeeCalculationInvalidOperation = "Error calculating compliance scheme base fee.";
    }
}