using System.Diagnostics.CodeAnalysis;

namespace EPR.Payment.Service.Common.Data.DataModels.Migration
{
    /// <summary>
    /// A submission known not to migrate (e.g. bad source data). Not run-scoped: every run records it as Excluded
    /// instead of replaying it. Add rows with Scripts/Migration/exclude-submissions.sql.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ExcludedSubmission
    {
        public string SubmissionId { get; set; } = null!;

        public string Reason { get; set; } = null!;

        public string? Note { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
    }
}
