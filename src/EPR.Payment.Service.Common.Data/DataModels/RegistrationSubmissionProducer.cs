using System.Diagnostics.CodeAnalysis;

namespace EPR.Payment.Service.Common.Data.DataModels
{
    [ExcludeFromCodeCoverage]
    public class RegistrationSubmissionProducer
    {
        public Guid Id { get; set; }

        public Guid RegistrationSubmissionDataId { get; set; }

        public string OrganisationId { get; set; } = null!;

        public string OrganisationSize { get; set; } = null!;

        public int NationId { get; set; }

        public bool IsOnlineMarketplace { get; set; }

        public bool IsClosedLoopRecycling { get; set; }

        public bool IsNewJoiner { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        public RegistrationSubmissionData RegistrationSubmissionData { get; set; } = null!;

        public ICollection<RegistrationSubmissionSubsidiary> Subsidiaries { get; set; } = new List<RegistrationSubmissionSubsidiary>();
    }
}
