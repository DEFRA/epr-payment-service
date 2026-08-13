using System.Diagnostics.CodeAnalysis;

namespace EPR.Payment.Service.Common.Dtos.Request.RegistrationSubmission
{
    [ExcludeFromCodeCoverage]
    public class RegistrationSubmittedForRegulatorApprovalRequest
    {
        public Guid SubmissionId { get; set; }

        public string ApplicationReferenceNumber { get; set; } = string.Empty;

        public DateTime SubmissionDate { get; set; }
    }
}
