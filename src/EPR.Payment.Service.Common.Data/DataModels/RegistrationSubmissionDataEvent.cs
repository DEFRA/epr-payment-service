using System.Diagnostics.CodeAnalysis;

namespace EPR.Payment.Service.Common.Data.DataModels
{
    [ExcludeFromCodeCoverage]
    public class RegistrationSubmissionDataEvent
    {
        public Guid Id { get; set; }

        public Guid RegistrationSubmissionDataId { get; set; }

        public string EventName { get; set; } = null!;

        public DateTime EventDate { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        public virtual RegistrationSubmissionData RegistrationSubmissionData { get; set; } = null!;
    }
}
