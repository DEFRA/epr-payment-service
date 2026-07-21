using System.Diagnostics.CodeAnalysis;
using EPR.Payment.Service.Common.Data.DataModels.Lookups;

namespace EPR.Payment.Service.Common.Data.DataModels
{
    [ExcludeFromCodeCoverage]
    public class RegistrationSubmissionData
    {
        public Guid Id { get; set; }

        public Guid SubmissionId { get; set; }

        public string RegistrationBlobName { get; set; } = null!;

        public Guid? ComplianceSchemeId { get; set; }

        public DateTime SubmissionDate { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        public int SubmissionPeriodId { get; set; }

        public ICollection<RegistrationSubmissionProducer> Producers { get; set; } = new List<RegistrationSubmissionProducer>();

        public virtual SubmissionPeriod SubmissionPeriodWindow { get; set; } = null!;

        public ICollection<RegistrationSubmissionDataEvent> Events { get; set; } = new List<RegistrationSubmissionDataEvent>();
    }
}
