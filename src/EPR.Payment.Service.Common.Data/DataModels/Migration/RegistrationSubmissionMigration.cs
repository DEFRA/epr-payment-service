using System.Diagnostics.CodeAnalysis;

namespace EPR.Payment.Service.Common.Data.DataModels.Migration
{
    [ExcludeFromCodeCoverage]
    public class RegistrationSubmissionMigration
    {
        public Guid Id { get; set; }

        public Guid RunId { get; set; }

        public string SubmissionId { get; set; } = null!;

        public SubmissionMigrationStatus Status { get; set; }

        public string? Reason { get; set; }

        public string? Error { get; set; }

        public string? Warnings { get; set; }

        public int FilesMigrated { get; set; }

        public int EventsReplayed { get; set; }

        public int EventsSkipped { get; set; }

        public DateTimeOffset? ProcessedAt { get; set; }
    }
}
