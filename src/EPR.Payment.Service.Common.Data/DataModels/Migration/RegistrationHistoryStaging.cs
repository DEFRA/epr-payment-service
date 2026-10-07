using System.Diagnostics.CodeAnalysis;

namespace EPR.Payment.Service.Common.Data.DataModels.Migration
{
    [ExcludeFromCodeCoverage]
    public class RegistrationHistoryStaging
    {
        public long Id { get; set; }

        public Guid RunId { get; set; }

        public string? SubmissionId { get; set; }

        public string? FileId { get; set; }

        public string? BlobName { get; set; }

        public string? OrganisationId { get; set; }

        public string? ComplianceSchemeId { get; set; }

        public string? SubmissionPeriod { get; set; }

        public string? RegistrationJourney { get; set; }

        public string? RegulatorNation { get; set; }

        public string? Roles { get; set; }

        public int? SubmissionFileCount { get; set; }

        public string? EventType { get; set; }

        public int EventTypeOrder { get; set; }

        public DateTime? ReplayTs { get; set; }

        public DateTime? EventDate { get; set; }

        public string? ApplicationReferenceNumber { get; set; }

        public string? Decision { get; set; }
    }
}
