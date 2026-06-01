using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace EPR.Payment.Service.Common.Dtos.Request.RegistrationSubmission
{
    [ExcludeFromCodeCoverage]
    public class CreateRegistrationSubmissionDataRequest
    {
        public Guid SubmissionId { get; set; }

        public Guid FileId { get; set; }

        [JsonPropertyName("blobName")]
        public string RegistrationBlobName { get; set; } = string.Empty;

        public Guid? ComplianceSchemeId { get; set; }

        public string SubmissionPeriod { get; set; } = string.Empty;

        public DateTime SubmissionDate { get; set; }
    }
}
