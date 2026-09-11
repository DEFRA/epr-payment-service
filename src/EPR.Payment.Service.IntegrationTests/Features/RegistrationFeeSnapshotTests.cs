using System.Net;
using AwesomeAssertions;
using EPR.Payment.Service.Common.Constants;
using EPR.Payment.Service.Common.Data;
using EPR.Payment.Service.Common.Dtos.Response.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Common.Dtos.Response.RegistrationFees.Producer;
using EPR.Payment.Service.IntegrationTests.Infrastructure;
using EPR.Payment.Service.IntegrationTests.Infrastructure.Builders;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EPR.Payment.Service.IntegrationTests.Features;

/// <summary>
/// covers the fee snapshot created on <c>SubmittedForRegulatorApproval</c> consumption,
/// and the resulting fork in the by-submission read path (snapshot projection vs. live-calc
/// fallback).
/// </summary>
public class RegistrationFeeSnapshotTests(ServiceFixture fixture) : IntegrationTestBase(fixture)
{
    // 01 - migration idempotency ------------------------------------------------------------

    [Fact]
    public async Task GIVEN_already_migrated_database_WHEN_migrator_runs_again_THEN_no_exception()
    {
        using var scope = fixture.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var act = async () => await context.Database.MigrateAsync();

        await act.Should().NotThrowAsync(
            "migrations.sql's IF NOT EXISTS guards (and EF's own __EFMigrationsHistory check) " +
            "must make re-applying migrations against an up-to-date database a no-op");
    }

    // 02 - snapshot / live-calculation round-trip regression --------------------------------

    public static IEnumerable<object[]> ProducerCases() =>
    [
        [new ProducerCase("Large, no subsidiaries, on time", "Large", 0, false, false, false, false, false)],
        [new ProducerCase("Large, 3 subsidiaries (OMP + CLR), on time", "Large", 3, true, true, true, true, false)],
        [new ProducerCase("Small, no subsidiaries, on time", "Small", 0, false, false, false, false, false)],
        [new ProducerCase("Small, 2 subsidiaries (OMP), on time", "Small", 2, true, false, true, false, false)],
        [new ProducerCase("Large, 1 subsidiary, late", "Large", 1, false, false, false, false, true)],
        [new ProducerCase("Small, 1 subsidiary, late", "Small", 1, false, false, false, false, true)],
    ];

    [Theory]
    [MemberData(nameof(ProducerCases))]
    public async Task GIVEN_producer_submission_WHEN_snapshot_created_THEN_matches_precreation_live_calculation(ProducerCase testCase)
    {
        var submissionPeriodId = testCase.Late
            ? SeededSubmissionPeriods.DirectSmallProducer2026
            : SeededSubmissionPeriods.DirectLargeProducer2027;

        var submissionId = Guid.NewGuid();
        var applicationReferenceNumber = NewApplicationReferenceNumber();

        var built = await Builder.RegistrationSubmissionData()
            .ForSubmissionId(submissionId)
            .WithApplicationReferenceNumber(applicationReferenceNumber)
            .InSubmissionPeriod(submissionPeriodId)
            .WithProducer(p =>
            {
                _ = testCase.ProducerSize == "Large" ? p.AsLarge() : p.AsSmall();
                p.AsOnlineMarketplace(testCase.ProducerOnlineMarketplace);
                p.AsClosedLoopRecycling(testCase.ProducerClosedLoopRecycling);
                p.WithSubsidiaries(testCase.SubsidiaryCount, s => s
                    .AsOnlineMarketplace(testCase.SubsidiariesOnlineMarketplace)
                    .AsClosedLoopRecycling(testCase.SubsidiariesClosedLoopRecycling));
            })
            .Build();

        // Before the event: no snapshot exists yet, so this drives the live-calculation fallback.
        var beforeResponse = await Client.GetAsync($"/api/v1/producer/registration-fee/{submissionId}");
        beforeResponse.StatusCode.Should().Be(HttpStatusCode.OK, testCase.Label);
        var before = await beforeResponse.ReadJson<RegistrationFeesResponseDto>();

        await Events.PublishSubmittedForRegulatorApprovalAsync(submissionId, applicationReferenceNumber, built.SubmissionDate);
        await WaitForSnapshotAsync(built.Id);

        // After the event: a snapshot now exists, so this drives the projection path instead.
        var after = await GetProducerFeesBySubmission(submissionId);

        after.Should().BeEquivalentTo(before, options => options
            // Known, accepted, low-impact inconsistency: the live-calc path leaves MemberId at
            // its default (null) for a producer response, while the snapshot projector always
            // sets it to string.Empty. Not meaningful for a producer response either way (MemberId
            // only carries information for compliance-scheme member breakdowns) - excluded here
            // rather than left as permanent test noise. Does not cover the FeeBreakdowns
            // divergence this test also surfaces, which remains open pending investigation.
            .Excluding(dto => dto.MemberId), testCase.Label);
    }

    public static IEnumerable<object[]> ComplianceSchemeCases() =>
    [
        [new ComplianceSchemeCase("Standard window, 2 large members, on time", SeededSubmissionPeriods.CsoLargeProducer2027, 2, "Large", false)],
        [new ComplianceSchemeCase("Small-producer window (registration fee excluded), 3 small members, on time", SeededSubmissionPeriods.CsoSmallProducer2027, 3, "Small", false)],
        [new ComplianceSchemeCase("Standard window, 1 large member, late", SeededSubmissionPeriods.CsoLargeProducer2026, 1, "Large", true)],
    ];

    [Theory]
    [MemberData(nameof(ComplianceSchemeCases))]
    public async Task GIVEN_compliance_scheme_submission_WHEN_snapshot_created_THEN_matches_precreation_live_calculation(ComplianceSchemeCase testCase)
    {
        var submissionId = Guid.NewGuid();
        var applicationReferenceNumber = NewApplicationReferenceNumber();

        var dataBuilder = Builder.RegistrationSubmissionData()
            .ForSubmissionId(submissionId)
            .WithApplicationReferenceNumber(applicationReferenceNumber)
            .InSubmissionPeriod(testCase.SubmissionPeriodId)
            .ForComplianceScheme(Guid.NewGuid());

        for (var i = 0; i < testCase.MemberCount; i++)
        {
            dataBuilder = dataBuilder.WithProducer(p =>
            {
                _ = testCase.MemberSize == "Large" ? p.AsLarge() : p.AsSmall();
            });
        }

        var built = await dataBuilder.Build();

        var beforeResponse = await Client.GetAsync($"/api/v1/compliance-scheme/registration-fee/{submissionId}");
        beforeResponse.StatusCode.Should().Be(HttpStatusCode.OK, testCase.Label);
        var before = await beforeResponse.ReadJson<ComplianceSchemeFeesResponseDto>();

        await Events.PublishSubmittedForRegulatorApprovalAsync(submissionId, applicationReferenceNumber, built.SubmissionDate);
        await WaitForSnapshotAsync(built.Id);

        var after = await GetComplianceSchemeFeesBySubmission(submissionId);

        after.Should().BeEquivalentTo(before, testCase.Label);
    }

    // 03 - duplicate event delivery is idempotent --------------------------------------------

    [Fact]
    public async Task GIVEN_snapshot_already_exists_WHEN_same_event_delivered_again_THEN_exactly_one_snapshot_persists()
    {
        var submissionId = Guid.NewGuid();
        var applicationReferenceNumber = NewApplicationReferenceNumber();

        var built = await Builder.RegistrationSubmissionData()
            .ForSubmissionId(submissionId)
            .WithApplicationReferenceNumber(applicationReferenceNumber)
            .InSubmissionPeriod(SeededSubmissionPeriods.DirectLargeProducer2027)
            .WithProducer(p => p.AsLarge())
            .Build();

        await Events.PublishSubmittedForRegulatorApprovalAsync(submissionId, applicationReferenceNumber, built.SubmissionDate);
        await WaitForSnapshotAsync(built.Id);
        var first = await GetProducerFeesBySubmission(submissionId);

        // Simulates at-least-once redelivery of the same message (or two concurrent consumers
        // racing after both passed the handler's "does a snapshot already exist" check) - the
        // second delivery is expected to be a clean no-op. This exercises the sequential
        // redelivery case; a true concurrent race is not constructed here.
        await Events.PublishSubmittedForRegulatorApprovalAsync(submissionId, applicationReferenceNumber, built.SubmissionDate);

        // Give the (no-op) redelivery a moment to be processed, then assert the DB still has one row.
        await Task.Delay(TimeSpan.FromSeconds(2));

        using var scope = fixture.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var snapshotCount = await context.RegistrationFeeSnapshot
            .CountAsync(s => s.RegistrationSubmissionDataId == built.Id);

        snapshotCount.Should().Be(1);

        var second = await GetProducerFeesBySubmission(submissionId);
        second.Should().BeEquivalentTo(first, "redelivery must not change the frozen snapshot values");
    }

    // 04 - a resubmission years after the original submission keeps the original submission's fee
    // rate - by design, not a bug. Confirmed with product: once a registration's fee is
    // calculated, it is meant to stay fixed forever, specifically so that everywhere this fee is
    // looked up it is guaranteed to show the same figure rather than risk drifting if it were
    // ever recalculated against a newer rate table. --------------------------------------------

    [Fact]
    public async Task GIVEN_producer_first_submitted_in_2025_WHEN_resubmitted_in_2026_THEN_snapshot_still_uses_the_2025_fee_rate()
    {
        // RegistrationFeesDataSeed seeds two non-overlapping, genuinely different rates for a
        // Large producer's base registration fee: 262000 (pence) effective 2024-01-01 to
        // 2025-12-31, and 284200 effective 2026-01-01 to 2026-12-31. Picking one submission date
        // from each band, for the *same* SubmissionId thread, is what makes this deterministic -
        // no need to fake the clock, since the rate lookup keys off the stored event date, not
        // wall-clock time (see SubmissionLifecycleAnalyser.CalcDate).
        const decimal fee2025Pence = 262000m;
        const decimal fee2026Pence = 284200m;
        var originalSubmissionDate = new DateTime(2025, 6, 1, 0, 0, 0, DateTimeKind.Utc);
        var resubmissionDate = new DateTime(2026, 6, 1, 0, 0, 0, DateTimeKind.Utc);

        var submissionId = Guid.NewGuid();
        var applicationReferenceNumber = NewApplicationReferenceNumber();

        // The original 2025 submission cycle: already has its own SubmittedForRegulatorApproval
        // event on record (as if this row's own approval event was processed years ago).
        await Builder.RegistrationSubmissionData()
            .ForSubmissionId(submissionId)
            .WithApplicationReferenceNumber(applicationReferenceNumber)
            .InSubmissionPeriod(SeededSubmissionPeriods.DirectLargeProducer2026)
            .SubmittedOn(originalSubmissionDate)
            .WithProducer(p => p.AsLarge())
            .WithEvent(RegistrationEventNames.SubmittedForRegulatorApproval, originalSubmissionDate)
            .Build();

        // The 2026 resubmission cycle: a new row for the same SubmissionId, no event yet - its
        // own SubmittedForRegulatorApproval event is about to be published for real, dated 2026.
        var resubmission = await Builder.RegistrationSubmissionData()
            .ForSubmissionId(submissionId)
            .WithApplicationReferenceNumber(applicationReferenceNumber)
            .InSubmissionPeriod(SeededSubmissionPeriods.DirectLargeProducer2027)
            .SubmittedOn(resubmissionDate)
            .WithProducer(p => p.AsLarge())
            .Build();

        await Events.PublishSubmittedForRegulatorApprovalAsync(submissionId, applicationReferenceNumber, resubmissionDate);
        await WaitForSnapshotAsync(resubmission.Id);

        var response = await GetProducerFeesBySubmission(submissionId);

        response.Should().NotBeNull();
        response!.ProducerRegistrationFee.Should().Be(
            fee2025Pence,
            "the snapshot handler builds its request from lifecycle.CalcDate, which is pinned to the " +
            "*first* non-rejected cycle's SubmittedForRegulatorApproval date (2025) - the 2026 " +
            "resubmission's own event date must not override it");
        response.ProducerRegistrationFee.Should().NotBe(fee2026Pence);
    }

    // Late-fee lifecycle scenarios - how SubmissionLifecycleAnalyser/RegistrationFeeRequestBuilder
    // decide IsLateFeeApplicable, and how that interacts with regulator decision events recorded
    // between submissions. All rows below share SeededSubmissionPeriods.DirectLargeProducer2026
    // (deadline 2025-10-02) and stay within the 2024-2025 fee-rate band, so the only thing varying
    // between cases is the late-fee boolean, not the underlying rate (late fee is a flat 33200
    // pence in that band, GroupId=1/SubGroup.LateFee, regardless of regulator or producer size). --

    private static readonly DateTime LateFeeBeforeDeadline = new(2025, 8, 1, 0, 0, 0, DateTimeKind.Utc);
    private const decimal LateFeePence = 33200m;

    [Fact]
    public async Task GIVEN_submission_made_after_the_deadline_THEN_late_fee_is_applied()
    {
        var afterDeadline = new DateTime(2025, 11, 1, 0, 0, 0, DateTimeKind.Utc);
        var submissionId = Guid.NewGuid();
        var applicationReferenceNumber = NewApplicationReferenceNumber();

        var built = await Builder.RegistrationSubmissionData()
            .ForSubmissionId(submissionId)
            .WithApplicationReferenceNumber(applicationReferenceNumber)
            .InSubmissionPeriod(SeededSubmissionPeriods.DirectLargeProducer2026)
            .SubmittedOn(afterDeadline)
            .WithProducer(p => p.AsLarge())
            .Build();

        await Events.PublishSubmittedForRegulatorApprovalAsync(submissionId, applicationReferenceNumber, afterDeadline);
        await WaitForSnapshotAsync(built.Id);

        var response = await GetProducerFeesBySubmission(submissionId);

        response.Should().NotBeNull();
        response!.ProducerLateRegistrationFee.Should().Be(
            LateFeePence,
            "the submission date (2025-11-01) is after the window's deadline (2025-10-02)");
    }

    [Fact]
    public async Task GIVEN_original_submission_was_late_WHEN_resubmitted_THEN_resubmission_snapshot_still_applies_late_fee()
    {
        var originalDate = new DateTime(2025, 11, 1, 0, 0, 0, DateTimeKind.Utc); // after deadline
        var resubmissionDate = new DateTime(2025, 12, 1, 0, 0, 0, DateTimeKind.Utc);

        var submissionId = Guid.NewGuid();
        var applicationReferenceNumber = NewApplicationReferenceNumber();

        await Builder.RegistrationSubmissionData()
            .ForSubmissionId(submissionId)
            .WithApplicationReferenceNumber(applicationReferenceNumber)
            .InSubmissionPeriod(SeededSubmissionPeriods.DirectLargeProducer2026)
            .SubmittedOn(originalDate)
            .WithProducer(p => p.AsLarge())
            .WithEvent(RegistrationEventNames.SubmittedForRegulatorApproval, originalDate)
            .Build();

        var resubmission = await Builder.RegistrationSubmissionData()
            .ForSubmissionId(submissionId)
            .WithApplicationReferenceNumber(applicationReferenceNumber)
            .InSubmissionPeriod(SeededSubmissionPeriods.DirectLargeProducer2026)
            .SubmittedOn(resubmissionDate)
            .WithProducer(p => p.AsLarge())
            .Build();

        await Events.PublishSubmittedForRegulatorApprovalAsync(submissionId, applicationReferenceNumber, resubmissionDate);
        await WaitForSnapshotAsync(resubmission.Id);

        var response = await GetProducerFeesBySubmission(submissionId);

        response.Should().NotBeNull();
        response!.ProducerLateRegistrationFee.Should().Be(
            LateFeePence,
            "FirstSubmittedForApprovalDate stays pinned to the original (late) cycle's own date, so " +
            "firstSubmissionWasLate stays true for every later resubmission of this same thread");
    }

    [Fact]
    public async Task GIVEN_original_submission_was_on_time_WHEN_queried_after_deadline_and_then_resubmitted_THEN_late_fee_is_not_applied()
    {
        var queriedDate = new DateTime(2025, 10, 15, 0, 0, 0, DateTimeKind.Utc); // after deadline
        var resubmissionDate = new DateTime(2025, 12, 1, 0, 0, 0, DateTimeKind.Utc); // also after deadline

        var submissionId = Guid.NewGuid();
        var applicationReferenceNumber = NewApplicationReferenceNumber();

        await Builder.RegistrationSubmissionData()
            .ForSubmissionId(submissionId)
            .WithApplicationReferenceNumber(applicationReferenceNumber)
            .InSubmissionPeriod(SeededSubmissionPeriods.DirectLargeProducer2026)
            .SubmittedOn(LateFeeBeforeDeadline)
            .WithProducer(p => p.AsLarge())
            .WithEvent(RegistrationEventNames.SubmittedForRegulatorApproval, LateFeeBeforeDeadline)
            .WithEvent(RegistrationEventNames.QueriedByRegulator, queriedDate)
            .Build();

        var resubmission = await Builder.RegistrationSubmissionData()
            .ForSubmissionId(submissionId)
            .WithApplicationReferenceNumber(applicationReferenceNumber)
            .InSubmissionPeriod(SeededSubmissionPeriods.DirectLargeProducer2026)
            .SubmittedOn(resubmissionDate)
            .WithProducer(p => p.AsLarge())
            .Build();

        await Events.PublishSubmittedForRegulatorApprovalAsync(submissionId, applicationReferenceNumber, resubmissionDate);
        await WaitForSnapshotAsync(resubmission.Id);

        var response = await GetProducerFeesBySubmission(submissionId);

        response.Should().NotBeNull();
        response!.ProducerLateRegistrationFee.Should().Be(
            0m,
            "QueriedByRegulator does not remove the original on-time row from the lifecycle, and is " +
            "not itself a SubmittedForRegulatorApproval event, so FirstSubmittedForApprovalDate stays " +
            "pinned to the original on-time date regardless of when the query or the resubmission happen");
    }

    [Fact]
    public async Task GIVEN_original_submission_was_on_time_WHEN_rejected_after_deadline_and_then_resubmitted_after_deadline_THEN_late_fee_is_applied()
    {
        var rejectedDate = new DateTime(2025, 10, 15, 0, 0, 0, DateTimeKind.Utc); // after deadline
        var resubmissionDate = new DateTime(2025, 12, 1, 0, 0, 0, DateTimeKind.Utc); // also after deadline

        var submissionId = Guid.NewGuid();
        var applicationReferenceNumber = NewApplicationReferenceNumber();

        await Builder.RegistrationSubmissionData()
            .ForSubmissionId(submissionId)
            .WithApplicationReferenceNumber(applicationReferenceNumber)
            .InSubmissionPeriod(SeededSubmissionPeriods.DirectLargeProducer2026)
            .SubmittedOn(LateFeeBeforeDeadline)
            .WithProducer(p => p.AsLarge())
            .WithEvent(RegistrationEventNames.SubmittedForRegulatorApproval, LateFeeBeforeDeadline)
            .WithEvent(RegistrationEventNames.RejectedByRegulator, rejectedDate)
            .Build();

        var resubmission = await Builder.RegistrationSubmissionData()
            .ForSubmissionId(submissionId)
            .WithApplicationReferenceNumber(applicationReferenceNumber)
            .InSubmissionPeriod(SeededSubmissionPeriods.DirectLargeProducer2026)
            .SubmittedOn(resubmissionDate)
            .WithProducer(p => p.AsLarge())
            .Build();

        await Events.PublishSubmittedForRegulatorApprovalAsync(submissionId, applicationReferenceNumber, resubmissionDate);
        await WaitForSnapshotAsync(resubmission.Id);

        var response = await GetProducerFeesBySubmission(submissionId);

        response.Should().NotBeNull();
        response!.ProducerLateRegistrationFee.Should().Be(
            LateFeePence,
            "RejectedByRegulator excludes the original row from the lifecycle entirely, so the " +
            "resubmission becomes the *first* non-rejected cycle in its own right, and its own " +
            "SubmittedForRegulatorApproval date is after the deadline");
    }

    // 05 - zero producers ----------------------------------------------------------------------

    [Fact]
    public async Task GIVEN_submission_with_no_producers_WHEN_event_consumed_THEN_snapshot_creation_is_skipped()
    {
        var submissionId = Guid.NewGuid();
        var applicationReferenceNumber = NewApplicationReferenceNumber();

        var built = await Builder.RegistrationSubmissionData()
            .ForSubmissionId(submissionId)
            .WithApplicationReferenceNumber(applicationReferenceNumber)
            .InSubmissionPeriod(SeededSubmissionPeriods.DirectLargeProducer2027)
            .Build();

        await Events.PublishSubmittedForRegulatorApprovalAsync(submissionId, applicationReferenceNumber, built.SubmissionDate);
        await WaitForEventRecordedAsync(built.Id, RegistrationEventNames.SubmittedForRegulatorApproval);

        using var scope = fixture.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var snapshotExists = await context.RegistrationFeeSnapshot
            .AnyAsync(s => s.RegistrationSubmissionDataId == built.Id);

        snapshotExists.Should().BeFalse("a submission with zero producers has nothing to calculate a fee for");
    }

    // 06 - every cycle rejected -----------------------------------------------------------------

    [Fact]
    public async Task GIVEN_only_rejected_submission_rows_WHEN_event_consumed_THEN_snapshot_creation_is_skipped()
    {
        var submissionId = Guid.NewGuid();
        var applicationReferenceNumber = NewApplicationReferenceNumber();

        var built = await Builder.RegistrationSubmissionData()
            .ForSubmissionId(submissionId)
            .WithApplicationReferenceNumber(applicationReferenceNumber)
            .InSubmissionPeriod(SeededSubmissionPeriods.DirectLargeProducer2027)
            .WithProducer(p => p.AsLarge())
            .Rejected()
            .Build();

        await Events.PublishSubmittedForRegulatorApprovalAsync(submissionId, applicationReferenceNumber, built.SubmissionDate);
        await WaitForEventRecordedAsync(built.Id, RegistrationEventNames.SubmittedForRegulatorApproval);

        using var scope = fixture.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var snapshotExists = await context.RegistrationFeeSnapshot
            .AnyAsync(s => s.RegistrationSubmissionDataId == built.Id);

        snapshotExists.Should().BeFalse();

        var response = await Client.GetAsync($"/api/v1/producer/registration-fee/{submissionId}");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    // 07 - zero-amount / zero-price line items round-trip correctly -----------------------------

    [Fact]
    public async Task GIVEN_subsidiary_with_zero_price_but_positive_units_WHEN_snapshot_projected_THEN_band_still_present()
    {
        // A subsidiary that earns a place in a fee band (UnitCount > 0) but happens to price at
        // zero must still be projected back - only bands with both UnitCount<=0 AND
        // TotalPrice<=0 are dropped by RegistrationFeeSnapshotHandler.AddSubsidiaryLineItems.
        var submissionId = Guid.NewGuid();
        var applicationReferenceNumber = NewApplicationReferenceNumber();

        var built = await Builder.RegistrationSubmissionData()
            .ForSubmissionId(submissionId)
            .WithApplicationReferenceNumber(applicationReferenceNumber)
            .InSubmissionPeriod(SeededSubmissionPeriods.DirectLargeProducer2027)
            .WithProducer(p => p.AsLarge().WithSubsidiary())
            .Build();

        // Deliberately the plain (non-requireSubmittedForApproval) route - the same live-calc
        // fallback path scenario 02 exercises before an event exists.
        var beforeResponse = await Client.GetAsync($"/api/v1/producer/registration-fee/{submissionId}");
        beforeResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var before = await beforeResponse.ReadJson<RegistrationFeesResponseDto>();
        var expectedBand = before.SubsidiariesFeeBreakdown.FeeBreakdowns.Should().ContainSingle(b => b.UnitCount > 0).Subject;

        await Events.PublishSubmittedForRegulatorApprovalAsync(submissionId, applicationReferenceNumber, built.SubmissionDate);
        await WaitForSnapshotAsync(built.Id);

        var after = await GetProducerFeesBySubmission(submissionId);

        // Scenario 02 covers full round-trip equivalence (and, as currently run, surfaces that the
        // snapshot path omits zero-count/zero-price bands the live path always includes - see that
        // test's findings). This scenario checks the narrower, unambiguous claim the AC actually
        // needs: a band the producer is genuinely being charged for is never lost.
        after!.SubsidiariesFeeBreakdown.FeeBreakdowns.Should().ContainEquivalentOf(expectedBand);
    }

    // 08 - structured log on snapshot creation ---------------------------------------------------

    [Fact(Skip =
        "Blocked on a platform decision, not a test gap: Program.cs calls builder.Host.UseSerilog(...) " +
        "without writeToProviders: true, so Serilog owns logging exclusively and other " +
        "ILoggerProvider instances (this test's TestLoggerProvider included) never receive events. " +
        "Unblock either by adding writeToProviders: true in Program.cs (the standard, low-risk way " +
        "to let other providers observe the same log stream) or by asserting via a Serilog-native " +
        "sink configured through the test's Serilog:WriteTo configuration instead.")]
    public async Task GIVEN_event_consumed_WHEN_snapshot_created_THEN_structured_log_entry_recorded()
    {
        var submissionId = Guid.NewGuid();
        var applicationReferenceNumber = NewApplicationReferenceNumber();

        var built = await Builder.RegistrationSubmissionData()
            .ForSubmissionId(submissionId)
            .WithApplicationReferenceNumber(applicationReferenceNumber)
            .InSubmissionPeriod(SeededSubmissionPeriods.DirectLargeProducer2027)
            .WithProducer(p => p.AsLarge())
            .Build();

        await Events.PublishSubmittedForRegulatorApprovalAsync(submissionId, applicationReferenceNumber, built.SubmissionDate);

        var entries = await ResponsePolling.WaitUntilAsync(
            () => Task.FromResult(Logs.EntriesContaining(built.Id.ToString())),
            found => found.Count > 0);

        entries.Should().Contain(e =>
            e.Message.Contains("Created RegistrationFeeSnapshot", StringComparison.Ordinal));
    }

    /// <summary>
    /// Polls the database directly for the snapshot row rather than the by-submission GET
    /// endpoint - that endpoint returns 200 whether or not a snapshot exists yet (falling back to
    /// live calculation when it doesn't), so a non-null response is not a reliable "snapshot is
    /// ready" signal on its own.
    /// </summary>
    private async Task WaitForSnapshotAsync(Guid registrationSubmissionDataId)
    {
        await ResponsePolling.WaitUntilAsync(
            async () =>
            {
                using var scope = fixture.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                return await context.RegistrationFeeSnapshot
                    .AnyAsync(s => s.RegistrationSubmissionDataId == registrationSubmissionDataId);
            },
            exists => exists);
    }

    /// <summary>
    /// Waits for the given event to be recorded against a specific RegistrationSubmissionData row
    /// - a DB-observable proxy for "the handler has processed this message at least as far as
    /// recording the event", used where scenarios need a readiness signal but can't rely on log
    /// capture (see the Skip reason on scenario 08).
    /// </summary>
    private async Task WaitForEventRecordedAsync(Guid registrationSubmissionDataId, string eventName)
    {
        await ResponsePolling.WaitUntilAsync(
            async () =>
            {
                using var scope = fixture.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                return await context.RegistrationSubmissionDataEvents
                    .AnyAsync(e => e.RegistrationSubmissionDataId == registrationSubmissionDataId && e.EventName == eventName);
            },
            recorded => recorded);
    }

    private async Task<RegistrationFeesResponseDto?> GetProducerFeesBySubmission(Guid submissionId)
    {
        var response = await Client.GetAsync($"/api/v1/producer/registration-fee/{submissionId}?requireSubmittedForApproval=true");
        return response.StatusCode == HttpStatusCode.OK ? await response.ReadJson<RegistrationFeesResponseDto>() : null;
    }

    private async Task<ComplianceSchemeFeesResponseDto?> GetComplianceSchemeFeesBySubmission(Guid submissionId)
    {
        var response = await Client.GetAsync($"/api/v1/compliance-scheme/registration-fee/{submissionId}?requireSubmittedForApproval=true");
        return response.StatusCode == HttpStatusCode.OK ? await response.ReadJson<ComplianceSchemeFeesResponseDto>() : null;
    }

    private static string NewApplicationReferenceNumber() => $"PEPR{Guid.NewGuid():N}"[..15].ToUpperInvariant();

    public sealed record ProducerCase(
        string Label,
        string ProducerSize,
        int SubsidiaryCount,
        bool SubsidiariesOnlineMarketplace,
        bool SubsidiariesClosedLoopRecycling,
        bool ProducerOnlineMarketplace,
        bool ProducerClosedLoopRecycling,
        bool Late)
    {
        public override string ToString() => Label;
    }

    public sealed record ComplianceSchemeCase(
        string Label,
        int SubmissionPeriodId,
        int MemberCount,
        string MemberSize,
        bool Late)
    {
        public override string ToString() => Label;
    }
}
