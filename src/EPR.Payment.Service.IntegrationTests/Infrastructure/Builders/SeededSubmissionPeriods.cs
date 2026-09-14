namespace EPR.Payment.Service.IntegrationTests.Infrastructure.Builders;

/// <summary>
/// Named IDs for the <c>Lookup.SubmissionPeriod</c> rows seeded by migration
/// <c>20260709105007_AddSubmissionPeriodAndLink</c> (see migrations.sql). Fixed calendar dates,
/// not relative to "today" - safe to reference regardless of when the suite runs, as long as
/// it runs before the seeded years roll off. Deadline column drives late-fee determination.
/// </summary>
public static class SeededSubmissionPeriods
{
    /// <summary>Deadline 2025-04-02 (Cso, 2025) - always in the past relative to this suite's target years.</summary>
    public const int Cso2025 = 1;

    /// <summary>Deadline 2025-04-02 (Direct, 2025) - always in the past.</summary>
    public const int Direct2025 = 2;

    /// <summary>Deadline 2025-10-02 (CsoLargeProducer, 2026) - in the past relative to 2026 H2 onward.</summary>
    public const int CsoLargeProducer2026 = 3;

    /// <summary>Deadline 2026-04-02 (CsoSmallProducer, 2026). WindowType drives the "exclude registration fee" rule.</summary>
    public const int CsoSmallProducer2026 = 4;

    /// <summary>Deadline 2025-10-02 (DirectLargeProducer, 2026) - in the past relative to 2026 H2 onward.</summary>
    public const int DirectLargeProducer2026 = 5;

    /// <summary>Deadline 2026-04-02 (DirectSmallProducer, 2026).</summary>
    public const int DirectSmallProducer2026 = 6;

    /// <summary>Deadline 2026-10-02 (CsoLargeProducer, 2027) - in the future relative to 2026 H2.</summary>
    public const int CsoLargeProducer2027 = 7;

    /// <summary>Deadline 2027-04-02 (CsoSmallProducer, 2027) - in the future.</summary>
    public const int CsoSmallProducer2027 = 8;

    /// <summary>Deadline 2026-10-02 (DirectLargeProducer, 2027) - in the future relative to 2026 H2.</summary>
    public const int DirectLargeProducer2027 = 9;

    /// <summary>Deadline 2027-04-02 (DirectSmallProducer, 2027) - in the future.</summary>
    public const int DirectSmallProducer2027 = 10;
}
