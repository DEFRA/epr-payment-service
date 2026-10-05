using System.Diagnostics.CodeAnalysis;

namespace EPR.Payment.Service.Options
{
    [ExcludeFromCodeCoverage]
    public class RegistrationHistoryMigrationOptions
    {
        public const string SectionName = "Migration:RegistrationHistory";

        public bool Enabled { get; set; } = true;

        /// <summary>A run with this key executes at most once; change it to run the migration again after a clear-down.</summary>
        public string RunKey { get; set; } = "registration-history-v1";

        public bool ClearDownMigratedDataOnError { get; set; } = true;

        public int ProgressLogInterval { get; set; } = 100;

        public int StagingBatchSize { get; set; } = 1000;

        /// <summary>
        /// Subsidiary late-fee rates take effect from this date. Migrated subsidiary late fees are only zero
        /// while every migrated application-submitted date is earlier, so later dates fail the submission.
        /// </summary>
        public DateTime SubsidiaryLateFeeEffectiveDate { get; set; } = new(2026, 10, 1, 0, 0, 0, DateTimeKind.Utc);
    }
}
