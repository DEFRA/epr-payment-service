using EPR.Payment.Service.Common.Data.DataModels;

namespace EPR.Payment.Service.Services.RegistrationSubmission
{
    public record SubmissionLifecycle(
        RegistrationSubmissionData? FirstNonRejected,
        RegistrationSubmissionData? LatestNonRejected,
        DateTime? FirstSubmittedForApprovalDate,
        DateTime? LatestSubmittedForApprovalDate,
        DateTime CalcDate);
}
