namespace EPR.Payment.Service.Common.Constants.RegistrationFees.Exceptions
{
    public static class ComplianceSchemeFeeCalculationExceptions
    {
        public const string InvalidComplianceSchemeOrRegulatorError = "Registration fee for compliance scheme with regulator '{0}' not found.";
        public const string InvalidRegulatorError = "Fee for compliance scheme with regulator '{0}' not found.";
        public const string CalculationError = "An error occurred while calculating the compliance scheme fees.";
        public const string InvalidOnlineMarketPlaceError = "Compliance scheme Online Marketplace Fee record not found for regulator: '{0}'";
        public const string InvalidSubsidiariesFeeOrRegulatorError = "Subsidiaries fee for '{0}' and regulator '{1}' not found.";
        public const string InvalidMemberTypeOrRegulatorError = "Fee for Member type '{0}' and regulator '{1}' not found.";
    }
}