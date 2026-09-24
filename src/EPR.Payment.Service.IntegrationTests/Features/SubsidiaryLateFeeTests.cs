using System.Net;
using AwesomeAssertions;
using EPR.Payment.Service.Common.Constants;
using EPR.Payment.Service.Common.Dtos.Response.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Common.Dtos.Response.RegistrationFees.Producer;
using EPR.Payment.Service.IntegrationTests.Infrastructure;
using EPR.Payment.Service.IntegrationTests.Infrastructure.Builders;
using Microsoft.EntityFrameworkCore;
using EPR.Payment.Service.Common.Data;
using Microsoft.Extensions.DependencyInjection;

namespace EPR.Payment.Service.IntegrationTests.Features;

/// <summary>
/// SUB-225 ("Apply late fees to subsidiaries registered in a resubmission after the registration
/// deadline") outside-in coverage: whether a subsidiary that appears in a post-deadline resubmission
/// attracts its own late fee, per <see cref="EPR.Payment.Service.Services.RegistrationSubmission.NewlyAddedSubsidiaryFinder"/>
/// and <see cref="EPR.Payment.Service.Services.RegistrationSubmission.RegistrationFeeRequestBuilder"/>.
/// Producer- and compliance-scheme-level late fees (whole submission late) are already covered by
/// <see cref="RegistrationFeeSnapshotTests"/> - this file is scoped to the subsidiary-level rules
/// the ticket actually adds.
///
/// Dates are deliberately kept within 2026: RegistrationFeesDataSeed only seeds
/// ProducerSubsidiaries/LateFee and ComplianceSchemeSubsidiaries/LateFee from 2026-10-01 onward (a
/// SUB-225 addition, not present in the 2024/2025 historical band) - the fee lookup keys off
/// lifecycle.CalcDate, which is pinned to the *first* submission's date, so a scenario whose first
/// cycle is dated before 2026-10-01 silently prices the subsidiary late fee at zero regardless of
/// how many subsidiaries are actually flagged as newly-added, masking the count this file is
/// testing for. SeededSubmissionPeriods.DirectLargeProducer2027 / CsoLargeProducer2027 (deadline
/// 2026-10-02) still give a real before/after split: BeforeDeadline sits on 2026-10-01 so it's
/// both in-window for the fee lookup and still before the 2026-10-02 deadline.
/// </summary>
public class SubsidiaryLateFeeTests(ServiceFixture fixture) : IntegrationTestBase(fixture)
{
    private static readonly DateTime BeforeDeadline = new(2026, 10, 1, 0, 0, 0, DateTimeKind.Utc); // DirectLargeProducer2027 / CsoLargeProducer2027 deadline is 2026-10-02
    private static readonly DateTime AfterDeadline = new(2026, 12, 1, 0, 0, 0, DateTimeKind.Utc);

    // AC1 - happy path: a subsidiary that first appears in a post-deadline resubmission is charged.

    [Fact]
    public async Task GIVEN_new_subsidiary_added_in_resubmission_after_deadline_THEN_subsidiary_late_fee_is_charged()
    {
        var submissionId = Guid.NewGuid();
        var applicationReferenceNumber = NewApplicationReferenceNumber();
        var organisationId = NewOrganisationId();

        // Prior cycle: granted, on time, one subsidiary (S1) already known to the regulator.
        await Builder.RegistrationSubmissionData()
            .ForSubmissionId(submissionId)
            .WithApplicationReferenceNumber(applicationReferenceNumber)
            .InSubmissionPeriod(SeededSubmissionPeriods.DirectLargeProducer2027)
            .SubmittedOn(BeforeDeadline)
            .WithProducer(p => p.WithOrganisationId(organisationId).AsLarge().WithSubsidiary(s => s.WithSubsidiaryId("S1")))
            .WithEvent(RegistrationEventNames.SubmittedForRegulatorApproval, BeforeDeadline)
            .Accepted(BeforeDeadline.AddDays(2))
            .Build();

        // Resubmission after the deadline: same organisation, S1 again, plus brand-new S2.
        var resubmission = await Builder.RegistrationSubmissionData()
            .ForSubmissionId(submissionId)
            .WithApplicationReferenceNumber(applicationReferenceNumber)
            .InSubmissionPeriod(SeededSubmissionPeriods.DirectLargeProducer2027)
            .SubmittedOn(AfterDeadline)
            .WithProducer(p => p.WithOrganisationId(organisationId).AsLarge()
                .WithSubsidiary(s => s.WithSubsidiaryId("S1"))
                .WithSubsidiary(s => s.WithSubsidiaryId("S2")))
            .Build();

        await Events.PublishSubmittedForRegulatorApprovalAsync(submissionId, applicationReferenceNumber, AfterDeadline);
        await WaitForSnapshotAsync(resubmission.Id);

        var response = await GetProducerFeesBySubmission(submissionId);

        response.Should().NotBeNull();
        using (new AwesomeAssertions.Execution.AssertionScope())
        {
            response!.SubsidiariesFeeBreakdown.CountOfLateSubsidiaries.Should().Be(
                1, "only S2 is newly appearing after the deadline - S1 was already known from the granted on-time prior cycle");
            response.SubsidiariesFeeBreakdown.TotalSubsidiariesLateFees.Should().BeGreaterThan(
                0m, "a positive late-subsidiary count must produce a positive fee");
            response.ProducerLateRegistrationFee.Should().Be(
                0m, "the submission-level late fee is separate from the subsidiary late fee - the first cycle was on time");
        }
    }

    // AC2 - happy path: a subsidiary added in a resubmission still before the deadline is not charged.

    [Fact]
    public async Task GIVEN_new_subsidiary_added_in_resubmission_before_deadline_THEN_no_subsidiary_late_fee()
    {
        var submissionId = Guid.NewGuid();
        var applicationReferenceNumber = NewApplicationReferenceNumber();
        var organisationId = NewOrganisationId();

        await Builder.RegistrationSubmissionData()
            .ForSubmissionId(submissionId)
            .WithApplicationReferenceNumber(applicationReferenceNumber)
            .InSubmissionPeriod(SeededSubmissionPeriods.DirectLargeProducer2027)
            .SubmittedOn(BeforeDeadline)
            .WithProducer(p => p.WithOrganisationId(organisationId).AsLarge().WithSubsidiary(s => s.WithSubsidiaryId("S1")))
            .WithEvent(RegistrationEventNames.SubmittedForRegulatorApproval, BeforeDeadline)
            .Accepted(BeforeDeadline.AddDays(2))
            .Build();

        var stillBeforeDeadline = BeforeDeadline.AddHours(12); // must stay before the 2026-10-02 deadline
        var resubmission = await Builder.RegistrationSubmissionData()
            .ForSubmissionId(submissionId)
            .WithApplicationReferenceNumber(applicationReferenceNumber)
            .InSubmissionPeriod(SeededSubmissionPeriods.DirectLargeProducer2027)
            .SubmittedOn(stillBeforeDeadline)
            .WithProducer(p => p.WithOrganisationId(organisationId).AsLarge()
                .WithSubsidiary(s => s.WithSubsidiaryId("S1"))
                .WithSubsidiary(s => s.WithSubsidiaryId("S2")))
            .Build();

        await Events.PublishSubmittedForRegulatorApprovalAsync(submissionId, applicationReferenceNumber, stillBeforeDeadline);
        await WaitForSnapshotAsync(resubmission.Id);

        var response = await GetProducerFeesBySubmission(submissionId);

        response.Should().NotBeNull();
        using (new AwesomeAssertions.Execution.AssertionScope())
        {
            response!.SubsidiariesFeeBreakdown.CountOfLateSubsidiaries.Should().Be(
                0, "the resubmission that introduces S2 is itself still before the deadline");
            response.SubsidiariesFeeBreakdown.TotalSubsidiariesLateFees.Should().Be(0m);
        }
    }

    [Fact]
    public async Task GIVEN_subsidiary_submitted_on_time_but_only_queried_WHEN_resubmitted_after_deadline_THEN_no_subsidiary_late_fee()
    {
        var submissionId = Guid.NewGuid();
        var applicationReferenceNumber = NewApplicationReferenceNumber();
        var organisationId = NewOrganisationId();

        await Builder.RegistrationSubmissionData()
            .ForSubmissionId(submissionId)
            .WithApplicationReferenceNumber(applicationReferenceNumber)
            .InSubmissionPeriod(SeededSubmissionPeriods.DirectLargeProducer2027)
            .SubmittedOn(BeforeDeadline)
            .WithProducer(p => p.WithOrganisationId(organisationId).AsLarge().WithSubsidiary(s => s.WithSubsidiaryId("S1")))
            .WithEvent(RegistrationEventNames.SubmittedForRegulatorApproval, BeforeDeadline)
            .Queried(BeforeDeadline.AddDays(30)) // regulator queries it, after the deadline - never accepted
            .Build();

        var resubmission = await Builder.RegistrationSubmissionData()
            .ForSubmissionId(submissionId)
            .WithApplicationReferenceNumber(applicationReferenceNumber)
            .InSubmissionPeriod(SeededSubmissionPeriods.DirectLargeProducer2027)
            .SubmittedOn(AfterDeadline)
            .WithProducer(p => p.WithOrganisationId(organisationId).AsLarge().WithSubsidiary(s => s.WithSubsidiaryId("S1"))) // same S1, answering the query
            .Build();

        await Events.PublishSubmittedForRegulatorApprovalAsync(submissionId, applicationReferenceNumber, AfterDeadline);
        await WaitForSnapshotAsync(resubmission.Id);

        var response = await GetProducerFeesBySubmission(submissionId);

        response.Should().NotBeNull();
        response!.SubsidiariesFeeBreakdown.CountOfLateSubsidiaries.Should().Be(
            0, "S1 was already submitted on time before the deadline, no fee applies");
    }

    [Fact]
    public async Task GIVEN_subsidiary_submitted_on_time_but_only_queried_WHEN_resubmitted_after_deadline_with_another_subsidiary_THEN_subsidiary_late_fee_is_charged()
    {
        var submissionId = Guid.NewGuid();
        var applicationReferenceNumber = NewApplicationReferenceNumber();
        var organisationId = NewOrganisationId();

        await Builder.RegistrationSubmissionData()
            .ForSubmissionId(submissionId)
            .WithApplicationReferenceNumber(applicationReferenceNumber)
            .InSubmissionPeriod(SeededSubmissionPeriods.DirectLargeProducer2027)
            .SubmittedOn(BeforeDeadline)
            .WithProducer(p => p.WithOrganisationId(organisationId).AsLarge().WithSubsidiary(s => s.WithSubsidiaryId("S1")))
            .WithEvent(RegistrationEventNames.SubmittedForRegulatorApproval, BeforeDeadline)
            .Queried(BeforeDeadline.AddDays(30)) // regulator queries it, after the deadline - never accepted
            .Build();

        var resubmission = await Builder.RegistrationSubmissionData()
            .ForSubmissionId(submissionId)
            .WithApplicationReferenceNumber(applicationReferenceNumber)
            .InSubmissionPeriod(SeededSubmissionPeriods.DirectLargeProducer2027)
            .SubmittedOn(AfterDeadline)
            .WithProducer(p => p.WithOrganisationId(organisationId).AsLarge()
                .WithSubsidiary(s => s.WithSubsidiaryId("S1")) // same S1, answering the query
                .WithSubsidiary(s => s.WithSubsidiaryId("S2"))) // and a brand-new S2
            .Build();

        await Events.PublishSubmittedForRegulatorApprovalAsync(submissionId, applicationReferenceNumber, AfterDeadline);
        await WaitForSnapshotAsync(resubmission.Id);

        var response = await GetProducerFeesBySubmission(submissionId);

        response.Should().NotBeNull();
        using (new AwesomeAssertions.Execution.AssertionScope())
        {
            response!.SubsidiariesFeeBreakdown.CountOfLateSubsidiaries.Should().Be(
                1, "only S2 is newly appearing after the deadline - S1 was already submitted on time and only queried, not rejected or cancelled, so it still establishes baseline");
            response.SubsidiariesFeeBreakdown.TotalSubsidiariesLateFees.Should().BeGreaterThan(
                0m, "a positive late-subsidiary count must produce a positive fee");
            response.ProducerLateRegistrationFee.Should().Be(
                0m, "the submission-level late fee is separate from the subsidiary late fee - the first cycle was on time");
        }
    }

    [Fact]
    public async Task GIVEN_CS_member_subsidiary_submitted_on_time_but_only_queried_WHEN_resubmitted_after_deadline_THEN_no_subsidiary_late_fee()
    {
        var submissionId = Guid.NewGuid();
        var applicationReferenceNumber = NewApplicationReferenceNumber();
        var memberId = NewOrganisationId();

        await Builder.RegistrationSubmissionData()
            .ForSubmissionId(submissionId)
            .WithApplicationReferenceNumber(applicationReferenceNumber)
            .InSubmissionPeriod(SeededSubmissionPeriods.CsoLargeProducer2027)
            .ForComplianceScheme(Guid.NewGuid())
            .SubmittedOn(BeforeDeadline)
            .WithProducer(p => p.WithOrganisationId(memberId).AsLarge().WithSubsidiary(s => s.WithSubsidiaryId("S1")))
            .WithEvent(RegistrationEventNames.SubmittedForRegulatorApproval, BeforeDeadline)
            .Queried(BeforeDeadline.AddDays(30))
            .Build();

        var resubmission = await Builder.RegistrationSubmissionData()
            .ForSubmissionId(submissionId)
            .WithApplicationReferenceNumber(applicationReferenceNumber)
            .InSubmissionPeriod(SeededSubmissionPeriods.CsoLargeProducer2027)
            .ForComplianceScheme(Guid.NewGuid())
            .SubmittedOn(AfterDeadline)
            .WithProducer(p => p.WithOrganisationId(memberId).AsLarge().WithSubsidiary(s => s.WithSubsidiaryId("S1")))
            .Build();

        await Events.PublishSubmittedForRegulatorApprovalAsync(submissionId, applicationReferenceNumber, AfterDeadline);
        await WaitForSnapshotAsync(resubmission.Id);

        var response = await GetComplianceSchemeFeesBySubmission(submissionId);

        response.Should().NotBeNull();
        var member = response!.ComplianceSchemeMembersWithFees.Should().ContainSingle(m => m.MemberId == memberId).Subject;
        member.SubsidiariesFeeBreakdown.CountOfLateSubsidiaries.Should().Be(
            0, "S1 was already submitted on time before the deadline, no fee applies");
    }

    [Fact]
    public async Task GIVEN_CS_member_subsidiary_submitted_on_time_but_only_queried_WHEN_resubmitted_after_deadline_with_another_subsidiary_THEN_subsidiary_late_fee_is_charged()
    {
        var submissionId = Guid.NewGuid();
        var applicationReferenceNumber = NewApplicationReferenceNumber();
        var memberId = NewOrganisationId();

        await Builder.RegistrationSubmissionData()
            .ForSubmissionId(submissionId)
            .WithApplicationReferenceNumber(applicationReferenceNumber)
            .InSubmissionPeriod(SeededSubmissionPeriods.CsoLargeProducer2027)
            .ForComplianceScheme(Guid.NewGuid())
            .SubmittedOn(BeforeDeadline)
            .WithProducer(p => p.WithOrganisationId(memberId).AsLarge().WithSubsidiary(s => s.WithSubsidiaryId("S1")))
            .WithEvent(RegistrationEventNames.SubmittedForRegulatorApproval, BeforeDeadline)
            .Queried(BeforeDeadline.AddDays(30))
            .Build();

        var resubmission = await Builder.RegistrationSubmissionData()
            .ForSubmissionId(submissionId)
            .WithApplicationReferenceNumber(applicationReferenceNumber)
            .InSubmissionPeriod(SeededSubmissionPeriods.CsoLargeProducer2027)
            .ForComplianceScheme(Guid.NewGuid())
            .SubmittedOn(AfterDeadline)
            .WithProducer(p => p.WithOrganisationId(memberId).AsLarge()
                .WithSubsidiary(s => s.WithSubsidiaryId("S1"))
                .WithSubsidiary(s => s.WithSubsidiaryId("S2")))
            .Build();

        await Events.PublishSubmittedForRegulatorApprovalAsync(submissionId, applicationReferenceNumber, AfterDeadline);
        await WaitForSnapshotAsync(resubmission.Id);

        var response = await GetComplianceSchemeFeesBySubmission(submissionId);

        response.Should().NotBeNull();
        var member = response!.ComplianceSchemeMembersWithFees.Should().ContainSingle(m => m.MemberId == memberId).Subject;

        using (new AwesomeAssertions.Execution.AssertionScope())
        {
            member.SubsidiariesFeeBreakdown.CountOfLateSubsidiaries.Should().Be(
                1, "only S2 is newly appearing after the deadline - S1 was already known from the granted on-time prior cycle");
            member.SubsidiariesFeeBreakdown.TotalSubsidiariesLateFees.Should().BeGreaterThan(
                0m, "a positive late-subsidiary count must produce a positive fee");
            member.MemberLateRegistrationFee.Should().Be(
                0m, "the submission-level late fee is separate from the subsidiary late fee - the first cycle was on time");
        }
    }

    /// <summary>See <see cref="RegistrationFeeSnapshotTests.WaitForSnapshotAsync"/> for why this
    /// polls the database rather than the by-submission GET endpoint.</summary>
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

    // RegistrationSubmissionProducer.OrganisationId is varchar(20) - shorter than a full Guid, and
    // must be pinned to the SAME value across two separately-built cycles for the same submission
    // thread, or NewlyAddedSubsidiaryFinder (which keys on (OrganisationId, SubsidiaryId)) will
    // never match a subsidiary against its own prior cycle regardless of SubsidiaryId.
    private static string NewOrganisationId() => Guid.NewGuid().ToString("N")[..20];
}
