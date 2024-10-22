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
        public const string InvalidMemberId = "MemberId is required.";
        public const string MemberTypeRequired = "MemberType is required.";
        public const string InvalidMemberType = "Member Type must be one of the following: ";
        public const string ResubmissionDateRequired = "Resubmission Date is required.";
        public const string ResubmissionDateInvalid = "Resubmission Date cannot be in the future.";
        public const string ReferenceNumberRequired = "Reference Number is required.";
        public const string MemberCountGreaterThanZero = "Member Count must be greater than zero.";
        public const string ResubmissionDateMustBeUtc = "Resubmission date must be in UTC.";
    }
}
