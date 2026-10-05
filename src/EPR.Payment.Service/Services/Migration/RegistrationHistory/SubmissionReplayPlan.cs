using System.Diagnostics.CodeAnalysis;
using EPR.Payment.Service.Common.Data.DataModels.Migration;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationSubmission;

namespace EPR.Payment.Service.Services.Migration.RegistrationHistory
{
    public abstract record ReplayStep(string FileId, DateTime ReplayTs);

    public sealed record SubmittedStep(string FileId, DateTime ReplayTs, CreateRegistrationSubmissionDataRequest Request)
        : ReplayStep(FileId, ReplayTs);

    public sealed record ApplicationSubmittedStep(string FileId, DateTime ReplayTs, RegistrationSubmittedForRegulatorApprovalRequest Request)
        : ReplayStep(FileId, ReplayTs);

    public sealed record DecisionStep(string FileId, DateTime ReplayTs, RegulatorRegistrationDecisionRequest Request)
        : ReplayStep(FileId, ReplayTs);

    [ExcludeFromCodeCoverage]
    public sealed record SubmissionReplayPlan(
        Guid SubmissionId,
        IReadOnlyList<ReplayStep> Steps,
        IReadOnlyList<string> Warnings,
        int EventsSkipped);

    [ExcludeFromCodeCoverage]
    public sealed record SubmissionOutcome(
        SubmissionMigrationStatus Status,
        string? Reason = null,
        string? Error = null,
        IReadOnlyList<string>? Warnings = null,
        int FilesMigrated = 0,
        int EventsReplayed = 0,
        int EventsSkipped = 0);

    /// <summary>Either a plan to replay, or a terminal outcome decided without touching the registration tables.</summary>
    [ExcludeFromCodeCoverage]
    public sealed record SubmissionPlanResult(SubmissionReplayPlan? Plan, SubmissionOutcome? Outcome)
    {
        public static SubmissionPlanResult Replay(SubmissionReplayPlan plan) => new(plan, null);

        public static SubmissionPlanResult Failed(string reason, string error) =>
            new(null, new SubmissionOutcome(SubmissionMigrationStatus.Failed, reason, error));

        public static SubmissionPlanResult NotRequired(string reason, string? detail = null) =>
            new(null, new SubmissionOutcome(SubmissionMigrationStatus.NotRequired, reason, detail));
    }
}
