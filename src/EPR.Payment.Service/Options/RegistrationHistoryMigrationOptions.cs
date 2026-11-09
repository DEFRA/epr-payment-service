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
        /// The live service started charging subsidiary late fees on this date. Applications submitted earlier
        /// are replayed without them; later ones are charged, which needs the submission's full file history.
        /// </summary>
        public DateTime SubsidiaryLateFeeChargedFrom { get; set; } = new(2026, 10, 5, 0, 0, 0, DateTimeKind.Utc);
    }
}
