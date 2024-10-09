namespace EPR.Payment.Service.Common.Constants.RegistrationFees
{
    public static class ValidationMessages
    {
        public const string ProducerTypeInvalid = "Producer Type must be one of the following: ";
        public const string NumberOfSubsidiariesRange = "Number of subsidiaries must be greater than or equal to 0";
        public const string NumberOfOMPSubsidiariesLessThanOrEqualToNumberOfSubsidiaries = "Number of online marketplace subsidiaries must be less than or equal to number of subsidiaries";
        public const string RegulatorRequired = "Regulator is required.";
        public const string RegulatorInvalid = "Invalid Regulator.";
        public const string ApplicationReferenceNumberRequired = "Application Reference Number is required.";
        public const string NoOfSubsidiariesOnlineMarketplaceRange = "Number of Subsidiaries with Online Marketplace must be greater than or equal to 0";
        public const string InvalidComplianceSchemeMember = "Invalid ComplianceSchemeMember entry.";
        public const string InvalidMemberId = "MemberId must be greater than 0.";
        public const string MemberTypeRequired = "MemberType is required.";
        public const string InvalidMemberType = "MemberType must be either 'Large' or 'Small'.";
    }
}
