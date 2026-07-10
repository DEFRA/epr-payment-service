using System.Diagnostics.CodeAnalysis;

namespace EPR.Payment.Service.Common.Dtos.Request.RegistrationSubmission
{
    [ExcludeFromCodeCoverage]
    public class CreateRegistrationSubmissionDataRequest
    {
        public Guid SubmissionId { get; set; }

        public string RegistrationBlobName { get; set; } = string.Empty;

        public Guid? ComplianceSchemeId { get; set; }

        public DateTime SubmissionDate { get; set; }

        public int? SubmissionPeriodId { get; set; }
    }
}
