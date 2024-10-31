﻿namespace EPR.Payment.Service.Common.Constants.RegistrationFees
{
    public static class ValidationMessages
    {
        // PaymentRequestDto Validation Messages
        public const string UserIdRequired = "User id is required.";
        public const string OrganisationIdRequired = "Organisation ID is required.";
        public const string ReferenceRequired = "Reference is required.";
        public const string AmountRequiredAndGreaterThanZero = "Amount is required and must be greater than zero.";
        public const string RegulatorInvalid = "Invalid Regulator.";
        public const string RegulatorNotENG = "Online payment is not supported for this regulator.";
        public const string InvalidReasonForPayment = "Reason For Payment cannot be null or empty.";
        public const string InvalidGovPayPaymentId = "Gov Pay Payment ID cannot be null or empty.";
        public const string InvalidUpdatedByUserId = "Updated By User ID cannot be null or empty.";
        public const string InvalidOrganisationId = "Updated By Organisation ID cannot be null or empty.";
        public const string InvalidStatus = "Status cannot be null or empty.";
        public const string InvalidStatusType = "Status For Payment must be a valid status type.";
        public const string DescriptionRequired = "Description is required.";
        public const string InvalidDescription = "Description is invalid; acceptable values are 'Registration fee' or 'Packaging data resubmission fee'";
        public const string InvalidRegulatorOffline = "Regulator is invalid; acceptable values are 'GB-ENG', 'GB-SCT', 'GB-WLS' and 'GB-NIR'.";


        public const string ProducerTypeInvalid = "Producer Type must be one of the following: ";
        public const string NumberOfSubsidiariesRange = "Number of subsidiaries must be greater than or equal to 0";
        public const string NumberOfOMPSubsidiariesLessThanOrEqualToNumberOfSubsidiaries = "Number of online marketplace subsidiaries must be less than or equal to number of subsidiaries";
        public const string RegulatorRequired = "Regulator is required.";
        public const string ApplicationReferenceNumberRequired = "Application Reference Number is required.";
        public const string NoOfSubsidiariesOnlineMarketplaceRange = "Number of Subsidiaries with Online Marketplace must be greater than or equal to 0";
        public const string InvalidComplianceSchemeMember = "Invalid ComplianceSchemeMember entry.";
        public const string InvalidMemberId = "MemberId is required.";
        public const string MemberTypeRequired = "MemberType is required.";
        public const string InvalidMemberType = "Member Type must be one of the following: ";
        public const string InvalidSubmissionDate = "Submission Date is required. It must be a valid UTC date.";
        public const string FutureSubmissionDate = "Submission Date can not be future dated.";
        public const string ResubmissionDateRequired = "Resubmission Date is required.";
        public const string ResubmissionDateInvalid = "Resubmission Date cannot be in the future.";
        public const string ReferenceNumberRequired = "Reference Number is required.";
        public const string MemberCountGreaterThanZero = "Member Count must be greater than zero.";
        public const string ResubmissionDateMustBeUtc = "Resubmission date must be in UTC.";
    }
}
