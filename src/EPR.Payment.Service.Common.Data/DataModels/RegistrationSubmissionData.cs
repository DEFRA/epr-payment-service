using System.Diagnostics.CodeAnalysis;

namespace EPR.Payment.Service.Common.Data.DataModels
{
    [ExcludeFromCodeCoverage]
    public class RegistrationSubmissionData
    {
        public Guid Id { get; set; }

        public Guid SubmissionId { get; set; }

        public Guid FileId { get; set; }

        public string RegistrationBlobName { get; set; } = null!;

        public Guid? ComplianceSchemeId { get; set; }

        public string SubmissionPeriod { get; set; } = null!;

        public DateTime SubmissionDate { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        public DateTimeOffset? UpdatedDate { get; set; }

        public ICollection<RegistrationSubmissionProducer> Producers { get; set; } = new List<RegistrationSubmissionProducer>();
    }
}
