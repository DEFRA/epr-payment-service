using System.Diagnostics.CodeAnalysis;

namespace EPR.Payment.Service.Common.Data.DataModels
{
    [ExcludeFromCodeCoverage]
    public class RegistrationSubmissionSubsidiary
    {
        public Guid Id { get; set; }

        public Guid RegistrationSubmissionProducerId { get; set; }

        public string SubsidiaryId { get; set; } = null!;

        public bool IsOnlineMarketplace { get; set; }

        public bool IsClosedLoopRecycling { get; set; }

        public bool IsNewJoiner { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        public RegistrationSubmissionProducer RegistrationSubmissionProducer { get; set; } = null!;
    }
}
