using EPR.Payment.Service.Common.Constants.Fees;
using EPR.Payment.Service.Common.Enums;

namespace EPR.Payment.Service.Common.Constants.RegistrationFees
{
    public static class ValidationMessages
    {
        // PaymentRequestDto Validation Messages
        public const string UserIdRequired = "User id is required.";
        public const string OrganisationIdRequired = "Organisation ID is required.";
        public const string OrganisationIdInvalid = "Organisation ID is invalid, it should be the valid Guid.";
        public const string ReferenceRequired = "Reference is required.";
        public const string AmountRequiredAndGreaterThanZero = "Amount is required and must be greater than zero.";
        public const string RegulatorInvalid = "Invalid Regulator.";
        public const string RegulatorNotENG = "Online payment is not supported for this regulator.";
        public const string ReasonForPaymentRequired = "Reason For Payment cannot be null or empty.";
        public const string InvalidGovPayPaymentId = "Gov Pay Payment ID cannot be null or empty.";
        public const string InvalidUpdatedByUserId = "Updated By User ID cannot be null or empty.";
        public const string InvalidOrganisationId = "Updated By Organisation ID cannot be null or empty.";
        public const string InvalidStatus = "Status cannot be null or empty.";
        public const string InvalidStatusType = "Status For Payment must be a valid status type.";
        public const string DescriptionRequired = "The Description field is required.";
        public const string InvalidDescription = "Description is invalid; acceptable values are 'Registration fee' or 'Packaging data resubmission fee'.";
        public const string InvalidDescriptionV2 = "Description is invalid; acceptable values are 'Registration fee' or 'Accreditation fee' or 'Packaging data resubmission fee'.";
        public const string InvalidReasonForPayment = "ReasonForPayment is invalid; acceptable values are 'Registration fee' or 'Packaging data resubmission fee'.";
        public const string InvalidReasonForPaymentV2 = "ReasonForPayment is invalid; acceptable values are 'Registration fee' or 'Accreditation fee' or 'Packaging data resubmission fee'.";
        public const string InvalidRegulatorOffline = "Regulator is invalid; acceptable values are 'GB-ENG', 'GB-SCT', 'GB-WLS' and 'GB-NIR'.";
        public const string OfflineReferenceRequired = "The Reference field is required.";
        public const string OfflineRegulatorRequired = "The Regulator field is required.";
        public const string OfflinePaymentMethodRequired = "The PaymentMethod field is required.";
        public const string OnlineRequestorTypeRequired = "The RequestorType field is required.";

        public const string ProducerTypeInvalid = "Producer Type must be one of the following: ";
        public const string NumberOfSubsidiariesRange = "Number of subsidiaries must be greater than or equal to 0";
        public const string NumberOfOMPSubsidiariesLessThanOrEqualToNumberOfSubsidiaries = "Number of online marketplace subsidiaries must be less than or equal to number of subsidiaries";
        public const string RegulatorRequired = "Regulator is required.";
        public const string ApplicationReferenceNumberRequired = "Application Reference Number is required.";
        public const string NoOfSubsidiariesOnlineMarketplaceRange = "Number of Subsidiaries with Online Marketplace must be greater than or equal to 0";
        public const string ProducerMemberCountGreaterThanOrEqualToZero = "Member Count must be greater than or equal to zero.";
        public const string InvalidComplianceSchemeMember = "Invalid ComplianceSchemeMember entry.";
        public const string InvalidMemberId = "MemberId is required.";
        public const string MemberTypeRequired = "MemberType is required.";
        public const string InvalidMemberType = "Member Type must be one of the following: ";

        public const string InvalidSubmissionDate = "Submission date is mandatory and must be a valid date.";
        public const string FutureSubmissionDate = "Submission date cannot be a date in the future.";
        public const string SubmissionDateMustBeUtc = "Submission date must be in the UTC format which is YYYY-MM-DDTHH:MM:SSZ.";
        public const string SubmissionDateIsNotInRange = "Fee data is not available for given submission date.";
        public const string ResubmissionDateRequired = "Resubmission date is mandatory and must be a valid date.";
        public const string FutureResubmissionDate = "Resubmission date cannot be a date in the future.";
        public const string ResubmissionDateMustBeUtc = "Resubmission date must be in the UTC format which is YYYY-MM-DDTHH:MM:SSZ.";
        public const string ResubmissionDateIsNotInRange = "Fee data is not available for given resubmission date.";
        public const string ReferenceNumberRequired = "Reference Number is required.";        
        public const string MemberCountGreaterThanZero = "Member Count must be greater than zero.";

        //  AccreditationFeesRequestDto Validation Messages
        public const string EmptyRequestorType = "Requestor type is required";
        public const string InvalidRequestorType = "Requestor type must be one of the following: ";
        public const string EmptyTonnageBand = "Tonnage band is required";
        public const string InvalidTonnageBand = "Tonnage band must be one of the following: ";
        public const string EmptyMaterialType = "Material type is required";
        public const string InvalidMaterialType = "Material type must be one of the following: ";
        public static readonly string InvalidNumberOfOverseasSiteForExporter = $"Number of Overseas site must be greater than 0 and less than equal to {ReprocessorExporterConstants.MaxNumberOfOverseasSitesAllowed} for requestor type of exporter.";
        public const string InvalidNumberOfOverseasSiteForReprocessor = "Number of Overseas site must be 0 for requestor type of reprocessor.";

    }
}
