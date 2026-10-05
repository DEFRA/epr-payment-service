using System.Net;
using AwesomeAssertions;
using EPR.Payment.Service.Common.Constants;
using EPR.Payment.Service.Common.Data;
using EPR.Payment.Service.Common.Data.DataModels.Migration;
using EPR.Payment.Service.Common.Dtos.Response.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Common.Dtos.Response.RegistrationFees.Producer;
using EPR.Payment.Service.IntegrationTests.Infrastructure;
using EPR.Payment.Service.IntegrationTests.Infrastructure.Builders;
using EPR.Payment.Service.IntegrationTests.Infrastructure.Fakes;
using EPR.Payment.Service.Options;
using EPR.Payment.Service.Services.Migration.RegistrationHistory;
using EPR.Payment.Service.Services.RegistrationSubmission.Storage;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EPR.Payment.Service.IntegrationTests.Features;

/// <summary>
/// Outside-in coverage of the Synapse registration history migration: the dataset is replayed
/// through the real handlers, fee calculators and SQL Server schema. The common-data API and
/// blob storage are faked. Dates sit in 2025 (DirectLargeProducer2026 / CsoLargeProducer2026,
/// deadline 2025-10-02), before the 2026-10-01 subsidiary late-fee effective date, like real history.
/// </summary>
public class RegistrationHistoryMigrationTests(ServiceFixture fixture) : IntegrationTestBase(fixture), IAsyncLifetime
{
    private static readonly DateTime OnTime = new(2025, 9, 1, 9, 0, 0, DateTimeKind.Utc);
    private static readonly DateTime Late = new(2025, 11, 1, 9, 0, 0, DateTimeKind.Utc);

    private readonly FakeRegistrationBlobReader _blobs = new();
    private WebApplicationFactory<Program> _host = null!;

    public Task InitializeAsync()
    {
        _host = fixture.CreateHostWithServices(services => services.AddSingleton<IBlobReader>(_blobs));

        // Guard: this host must only ever talk to the Testcontainers SQL Server, never a configured/hosted one.
        string hostConnectionString;
        using (var scope = _host.Services.CreateScope())
        {
            hostConnectionString = scope.ServiceProvider.GetRequiredService<AppDbContext>().Database.GetConnectionString() ?? string.Empty;
        }

        if (!string.Equals(ServerOf(hostConnectionString), ServerOf(fixture.TestConnectionString), StringComparison.OrdinalIgnoreCase)
            || !string.Equals(DatabaseOf(hostConnectionString), DatabaseOf(fixture.TestConnectionString), StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException(
                $"Derived test host is not pointing at the test container database (host server '{ServerOf(hostConnectionString)}', expected '{ServerOf(fixture.TestConnectionString)}').");
        }

        return Task.CompletedTask;
    }

    public async Task DisposeAsync() => await _host.DisposeAsync();

    [Fact]
    public async Task GIVEN_on_time_direct_producer_history_WHEN_migrated_THEN_lifecycle_and_fee_snapshot_match_live_processing()
    {
        var submission = NewSubmission();
        _blobs.Add(submission.Blob(1), $"{submission.OrganisationId},,EN,L,,,", $"{submission.OrganisationId},SUB1,,,,,");
        var records = new List<RegistrationHistoryRecord>
        {
            submission.Declared(1, OnTime),
            submission.ApplicationSubmitted(1, OnTime.AddHours(1)),
            submission.Decision(1, OnTime.AddDays(4), "Accepted"),
        };

        var run = await RunMigrationAsync(records);

        run.Status.Should().Be(MigrationRunStatus.Completed);
        (await SubmissionStatusAsync(run, submission.Id)).Status.Should().Be(SubmissionMigrationStatus.Succeeded);

        await using var scope = _host.Services.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var data = await context.RegistrationSubmissionData
            .Include(r => r.Producers).ThenInclude(p => p.Subsidiaries)
            .Include(r => r.Events)
            .SingleAsync(r => r.SubmissionId == submission.Id);

        using (new AwesomeAssertions.Execution.AssertionScope())
        {
            data.CreatedDate.Should().Be(new DateTimeOffset(OnTime), "the replay clock gives rows their historic creation time");
            data.SubmissionPeriodId.Should().Be(SeededSubmissionPeriods.DirectLargeProducer2026);
            data.RegulatorNation.Should().Be("GB-ENG");
            data.ApplicationReferenceNumber.Should().Be(submission.Arn);
            data.Producers.Should().ContainSingle().Which.Subsidiaries.Should().ContainSingle();
            data.Events.Select(e => e.EventName).Should().BeEquivalentTo(
                new[] { RegistrationEventNames.SubmittedForRegulatorApproval, RegistrationEventNames.AcceptedByRegulator });
            (await context.RegistrationFeeSnapshot.AnyAsync(s => s.RegistrationSubmissionDataId == data.Id)).Should().BeTrue();
            (await context.MigratedEntity.CountAsync(m => m.RunId == run.Id && m.RegistrationSubmissionDataId == data.Id)).Should().Be(1);
        }

        var fees = await GetProducerFees(submission.Id, requireSubmittedForApproval: true);
        fees.Should().NotBeNull();
        fees!.TotalFee.Should().BeGreaterThan(0m);
        fees.ProducerLateRegistrationFee.Should().Be(0m, "the application was submitted before the 2025-10-02 deadline");
    }

    [Fact]
    public async Task GIVEN_first_file_never_submitted_for_approval_WHEN_migrated_THEN_historic_replay_clock_drives_calc_date()
    {
        // The first non-rejected cycle has no approval event, so CalcDate falls back to the handler's "now".
        // With the real clock that would be after 2026-10-01 and charge a subsidiary late fee; the replay
        // clock makes it the historic approval time, so the late subsidiary is counted but priced at zero.
        var submission = NewSubmission();
        _blobs.Add(submission.Blob(1), $"{submission.OrganisationId},,EN,L,,,", $"{submission.OrganisationId},SUB1,,,,,");
        _blobs.Add(submission.Blob(2), $"{submission.OrganisationId},,EN,L,,,", $"{submission.OrganisationId},SUB1,,,,,");
        var records = new List<RegistrationHistoryRecord>
        {
            submission.Declared(1, OnTime),
            submission.Declared(2, Late),
            submission.ApplicationSubmitted(2, Late.AddHours(1)),
        };

        var run = await RunMigrationAsync(records);

        run.Status.Should().Be(MigrationRunStatus.Completed);
        var fees = await GetProducerFees(submission.Id, requireSubmittedForApproval: true);
        fees.Should().NotBeNull();
        using (new AwesomeAssertions.Execution.AssertionScope())
        {
            fees!.RegistrationBlobName.Should().Be(submission.Blob(2));
            fees.ProducerLateRegistrationFee.Should().BeGreaterThan(0m, "the only approval was after the deadline");
            fees.SubsidiariesFeeBreakdown.TotalSubsidiariesLateFees.Should().Be(0m, "subsidiary late-fee rates start 2026-10-01");
        }
    }

    [Fact]
    public async Task GIVEN_latest_two_files_rejected_WHEN_migrated_THEN_producer_view_serves_older_non_rejected_snapshot()
    {
        var submission = NewSubmission();
        foreach (var file in new[] { 1, 2, 3 })
        {
            _blobs.Add(submission.Blob(file), $"{submission.OrganisationId},,EN,L,,,");
        }

        var records = new List<RegistrationHistoryRecord>
        {
            submission.Declared(1, OnTime),
            submission.ApplicationSubmitted(1, OnTime.AddHours(1)),
            submission.Decision(1, OnTime.AddDays(2), "Accepted"),
            submission.Declared(2, Late),
            submission.ApplicationSubmitted(2, Late.AddHours(1)),
            submission.Decision(2, Late.AddDays(2), "Rejected"),
            submission.Declared(3, Late.AddDays(10)),
            submission.ApplicationSubmitted(3, Late.AddDays(10).AddHours(1)),
            submission.Decision(3, Late.AddDays(12), "Rejected"),
        };

        var run = await RunMigrationAsync(records);

        run.Status.Should().Be(MigrationRunStatus.Completed);
        var fees = await GetProducerFees(submission.Id, requireSubmittedForApproval: false);
        fees.Should().NotBeNull();
        fees!.RegistrationBlobName.Should().Be(submission.Blob(1));
        fees.ProducerLateRegistrationFee.Should().Be(0m);
    }

    [Fact]
    public async Task GIVEN_compliance_scheme_history_WHEN_migrated_THEN_member_fees_are_snapshotted()
    {
        var submission = NewSubmission(complianceSchemeId: Guid.NewGuid(), journey: "CsoLargeProducer");
        _blobs.Add(submission.Blob(1), "300001,,EN,L,,,", "300002,,EN,S,,,");
        var records = new List<RegistrationHistoryRecord>
        {
            submission.Declared(1, OnTime),
            submission.ApplicationSubmitted(1, OnTime.AddHours(1)),
        };

        var run = await RunMigrationAsync(records);

        run.Status.Should().Be(MigrationRunStatus.Completed);
        var response = await Client.GetAsync($"/api/v1/compliance-scheme/registration-fee/{submission.Id}?requireSubmittedForApproval=true");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var fees = await response.ReadJson<ComplianceSchemeFeesResponseDto>();
        fees!.ComplianceSchemeMembersWithFees.Select(m => m.MemberId).Should().BeEquivalentTo(new[] { "300001", "300002" });
        fees.ComplianceSchemeRegistrationFee.Should().BeGreaterThan(0m);
    }

    [Fact]
    public async Task GIVEN_submission_already_captured_live_WHEN_migrated_THEN_it_is_not_required_and_untouched()
    {
        var submission = NewSubmission();
        await Builder.RegistrationSubmissionData()
            .ForSubmissionId(submission.Id)
            .WithApplicationReferenceNumber(submission.Arn)
            .InSubmissionPeriod(SeededSubmissionPeriods.DirectLargeProducer2026)
            .SubmittedOn(OnTime)
            .WithProducer(p => p.WithOrganisationId(submission.OrganisationId).AsLarge())
            .Build();
        _blobs.Add(submission.Blob(1), $"{submission.OrganisationId},,EN,L,,,");

        var run = await RunMigrationAsync(new List<RegistrationHistoryRecord> { submission.Declared(1, OnTime) });

        run.Status.Should().Be(MigrationRunStatus.Completed);
        var status = await SubmissionStatusAsync(run, submission.Id);
        status.Status.Should().Be(SubmissionMigrationStatus.NotRequired);
        status.Reason.Should().Be(MigrationReasons.AlreadyCaptured);
        (await CountDataRowsAsync(submission.Id)).Should().Be(1);
    }

    [Fact]
    public async Task GIVEN_a_failed_submission_and_clear_down_enabled_WHEN_migrated_THEN_all_migrated_data_is_removed_but_logs_kept()
    {
        var good = NewSubmission();
        var missingBlob = NewSubmission();
        _blobs.Add(good.Blob(1), $"{good.OrganisationId},,EN,L,,,");
        var records = new List<RegistrationHistoryRecord>
        {
            good.Declared(1, OnTime),
            good.ApplicationSubmitted(1, OnTime.AddHours(1)),
            missingBlob.Declared(1, OnTime),
        };

        var run = await RunMigrationAsync(records, clearDown: true);

        using (new AwesomeAssertions.Execution.AssertionScope())
        {
            run.Status.Should().Be(MigrationRunStatus.ClearedDown);
            run.ClearDownApplied.Should().BeTrue();
            run.FailedCount.Should().Be(1);
            (await CountDataRowsAsync(good.Id)).Should().Be(0);
            (await CountDataRowsAsync(missingBlob.Id)).Should().Be(0);
            (await SubmissionStatusAsync(run, good.Id)).Status.Should().Be(SubmissionMigrationStatus.ClearedDown);
            var failed = await SubmissionStatusAsync(run, missingBlob.Id);
            failed.Status.Should().Be(SubmissionMigrationStatus.Failed);
            failed.Reason.Should().Be(MigrationReasons.BlobNotFound);
            (await CountStagingRowsAsync(run)).Should().Be(3, "staging is retained for diagnosis");
            (await CountMigratedEntitiesAsync(run)).Should().Be(1, "the audit of what was created is retained");
        }
    }

    [Fact]
    public async Task GIVEN_a_failed_submission_and_clear_down_disabled_WHEN_migrated_THEN_successful_submissions_are_kept()
    {
        var good = NewSubmission();
        var missingBlob = NewSubmission();
        _blobs.Add(good.Blob(1), $"{good.OrganisationId},,EN,L,,,");
        var records = new List<RegistrationHistoryRecord> { good.Declared(1, OnTime), missingBlob.Declared(1, OnTime) };

        var run = await RunMigrationAsync(records, clearDown: false);

        run.Status.Should().Be(MigrationRunStatus.CompletedWithErrors);
        run.ClearDownApplied.Should().BeFalse();
        (await CountDataRowsAsync(good.Id)).Should().Be(1);
        (await SubmissionStatusAsync(run, missingBlob.Id)).Status.Should().Be(SubmissionMigrationStatus.Failed);
    }

    [Fact]
    public async Task GIVEN_a_completed_run_WHEN_started_again_with_the_same_run_key_THEN_it_never_runs_a_second_time()
    {
        var submission = NewSubmission();
        _blobs.Add(submission.Blob(1), $"{submission.OrganisationId},,EN,L,,,");
        var runKey = NewRunKey();
        var client = new FakeCommonDataApiClient(new List<RegistrationHistoryRecord> { submission.Declared(1, OnTime) });

        await RunMigrationAsync(client, runKey, clearDown: true);
        await RunMigrationAsync(client, runKey, clearDown: true);

        client.CallCount.Should().Be(1);
        (await CountDataRowsAsync(submission.Id)).Should().Be(1);
    }

    private Task<MigrationRun> RunMigrationAsync(IReadOnlyList<RegistrationHistoryRecord> records, bool clearDown = false) =>
        RunMigrationAsync(new FakeCommonDataApiClient(records), NewRunKey(), clearDown);

    private async Task<MigrationRun> RunMigrationAsync(ICommonDataApiClient client, string runKey, bool clearDown)
    {
        await using (var scope = _host.Services.CreateAsyncScope())
        {
            var runner = ActivatorUtilities.CreateInstance<RegistrationHistoryMigrationRunner>(
                scope.ServiceProvider,
                Microsoft.Extensions.Options.Options.Create(new RegistrationHistoryMigrationOptions
                {
                    Enabled = true,
                    RunKey = runKey,
                    ClearDownMigratedDataOnError = clearDown,
                    ProgressLogInterval = 1,
                }),
                Microsoft.Extensions.Options.Options.Create(new CommonDataApiOptions { BaseUrl = "http://common-data.invalid" }),
                client);

            await runner.RunAsync(CancellationToken.None);
        }

        await using var readScope = _host.Services.CreateAsyncScope();
        return await readScope.ServiceProvider.GetRequiredService<AppDbContext>().MigrationRun.AsNoTracking().SingleAsync(r => r.RunKey == runKey);
    }

    private async Task<RegistrationSubmissionMigration> SubmissionStatusAsync(MigrationRun run, Guid submissionId)
    {
        await using var scope = _host.Services.CreateAsyncScope();
        return await scope.ServiceProvider.GetRequiredService<AppDbContext>().RegistrationSubmissionMigration.AsNoTracking()
            .SingleAsync(s => s.RunId == run.Id && s.SubmissionId == submissionId.ToString());
    }

    private async Task<int> CountDataRowsAsync(Guid submissionId)
    {
        await using var scope = _host.Services.CreateAsyncScope();
        return await scope.ServiceProvider.GetRequiredService<AppDbContext>().RegistrationSubmissionData.CountAsync(r => r.SubmissionId == submissionId);
    }

    private async Task<int> CountStagingRowsAsync(MigrationRun run)
    {
        await using var scope = _host.Services.CreateAsyncScope();
        return await scope.ServiceProvider.GetRequiredService<AppDbContext>().RegistrationHistoryStaging.CountAsync(s => s.RunId == run.Id);
    }

    private async Task<int> CountMigratedEntitiesAsync(MigrationRun run)
    {
        await using var scope = _host.Services.CreateAsyncScope();
        return await scope.ServiceProvider.GetRequiredService<AppDbContext>().MigratedEntity.CountAsync(m => m.RunId == run.Id);
    }

    private async Task<RegistrationFeesResponseDto?> GetProducerFees(Guid submissionId, bool requireSubmittedForApproval)
    {
        var response = await Client.GetAsync($"/api/v1/producer/registration-fee/{submissionId}?requireSubmittedForApproval={requireSubmittedForApproval}");
        return response.StatusCode == HttpStatusCode.OK ? await response.ReadJson<RegistrationFeesResponseDto>() : null;
    }

    private static string ServerOf(string connectionString)
    {
        try
        {
            return new Microsoft.Data.SqlClient.SqlConnectionStringBuilder(connectionString).DataSource;
        }
        catch (ArgumentException)
        {
            return "<unparseable>";
        }
    }

    private static string DatabaseOf(string connectionString)
    {
        try
        {
            return new Microsoft.Data.SqlClient.SqlConnectionStringBuilder(connectionString).InitialCatalog;
        }
        catch (ArgumentException)
        {
            return "<unparseable>";
        }
    }

    private static string NewRunKey() => $"it-{Guid.NewGuid():N}";

    private static HistorySubmission NewSubmission(Guid? complianceSchemeId = null, string journey = "DirectLargeProducer") =>
        new(Guid.NewGuid(), $"PEPR{Guid.NewGuid():N}"[..15].ToUpperInvariant(), Guid.NewGuid().ToString("N")[..20], complianceSchemeId, journey);

    private sealed record HistorySubmission(Guid Id, string Arn, string OrganisationId, Guid? ComplianceSchemeId, string Journey)
    {
        public string Blob(int file) => $"{Id:N}-file{file}";

        public RegistrationHistoryRecord Declared(int file, DateTime at) => Record(file, RegistrationHistoryEventTypes.Submitted, 1, at) with
        {
            BlobName = Blob(file),
            ApplicationReferenceNumber = Arn,
        };

        public RegistrationHistoryRecord ApplicationSubmitted(int file, DateTime at) =>
            Record(file, RegistrationHistoryEventTypes.RegistrationApplicationSubmitted, 2, at) with { ApplicationReferenceNumber = Arn };

        public RegistrationHistoryRecord Decision(int file, DateTime at, string decision) =>
            Record(file, RegistrationHistoryEventTypes.RegulatorRegistrationDecision, 3, at) with { Decision = decision };

        private RegistrationHistoryRecord Record(int file, string eventType, int order, DateTime at) => new()
        {
            SubmissionId = Id.ToString(),
            FileId = $"{Id:N}-file{file}",
            ComplianceSchemeId = ComplianceSchemeId?.ToString(),
            SubmissionPeriod = "January to December 2026",
            RegistrationJourney = Journey,
            RegulatorNation = "GB-ENG",
            EventType = eventType,
            EventTypeOrder = order,
            ReplayTs = at,
            EventDate = at,
        };
    }
}
