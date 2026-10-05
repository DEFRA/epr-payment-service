using System.Diagnostics.CodeAnalysis;

namespace EPR.Payment.Service.Common.Data.DataModels.Migration
{
    // No FK to RegistrationSubmissionData: these audit rows must survive clear-down.
    [ExcludeFromCodeCoverage]
    public class MigratedEntity
    {
        public Guid Id { get; set; }

        public Guid RunId { get; set; }

        public Guid SubmissionId { get; set; }

        public string FileId { get; set; } = null!;

        public Guid RegistrationSubmissionDataId { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
    }
}
