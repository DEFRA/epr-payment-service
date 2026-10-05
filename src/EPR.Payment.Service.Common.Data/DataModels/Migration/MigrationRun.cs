using System.Diagnostics.CodeAnalysis;

namespace EPR.Payment.Service.Common.Data.DataModels.Migration
{
    [ExcludeFromCodeCoverage]
    public class MigrationRun
    {
        public Guid Id { get; set; }

        public string RunKey { get; set; } = null!;

        public MigrationRunStatus Status { get; set; }

        public string InstanceId { get; set; } = null!;

        public DateTimeOffset StartedAt { get; set; }

        public DateTimeOffset? StagingCompletedAt { get; set; }

        public DateTimeOffset? CompletedAt { get; set; }

        public int StagedRowCount { get; set; }

        public int SubmissionCount { get; set; }

        public int SucceededCount { get; set; }

        public int FailedCount { get; set; }

        public int NotRequiredCount { get; set; }

        public bool ClearDownApplied { get; set; }

        public string? ErrorMessage { get; set; }
    }
}
