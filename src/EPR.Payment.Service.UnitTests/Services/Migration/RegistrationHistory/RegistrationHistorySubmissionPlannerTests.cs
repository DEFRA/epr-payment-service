using EPR.Payment.Service.Common.Constants;
using EPR.Payment.Service.Common.Data.DataModels.Lookups;
using EPR.Payment.Service.Common.Data.DataModels.Migration;
using EPR.Payment.Service.Services.Migration.RegistrationHistory;
using FluentAssertions;
using FluentAssertions.Execution;

namespace EPR.Payment.Service.UnitTests.Services.Migration.RegistrationHistory
{
    [TestClass]
    public class RegistrationHistorySubmissionPlannerTests
    {
        private static readonly DateTime Cutover = new(2026, 7, 1, 0, 0, 0, DateTimeKind.Utc);
        private static readonly DateTime T0 = new(2026, 3, 1, 9, 0, 0, DateTimeKind.Unspecified);

        private static readonly IReadOnlyList<SubmissionPeriod> Periods = new List<SubmissionPeriod>
        {
            new() { Id = 1, WindowType = "Cso", RegistrationYear = 2025 },
            new() { Id = 2, WindowType = "Direct", RegistrationYear = 2025 },
            new() { Id = 3, WindowType = "CsoLargeProducer", RegistrationYear = 2026 },
            new() { Id = 5, WindowType = "DirectLargeProducer", RegistrationYear = 2026 },
        };

        private readonly Guid _submissionId = Guid.NewGuid();
        private long _nextId = 1;

        [TestMethod]
        public void Plan_DirectProducerLifecycle_BuildsOrderedHandlerRequests()
        {
            var rows = new List<RegistrationHistoryStaging>
            {
                Row(RegistrationHistoryEventTypes.RegulatorRegistrationDecision, "F1", T0.AddDays(5), decision: "Accepted"),
                Row(RegistrationHistoryEventTypes.Submitted, "F1", T0, arn: "PEPR001"),
                Row(RegistrationHistoryEventTypes.RegistrationApplicationSubmitted, "F1", T0.AddHours(1), arn: "PEPR001", eventDate: T0.AddHours(1).AddMinutes(-1)),
            };

            var result = Plan(rows);

            result.Outcome.Should().BeNull();
            var plan = result.Plan!;
            using (new AssertionScope())
            {
                plan.SubmissionId.Should().Be(_submissionId);
                plan.Steps.Select(s => s.GetType()).Should().ContainInOrder(typeof(SubmittedStep), typeof(ApplicationSubmittedStep), typeof(DecisionStep));

                var submitted = (SubmittedStep)plan.Steps[0];
                submitted.Request.SubmissionId.Should().Be(_submissionId);
                submitted.Request.RegistrationBlobName.Should().Be("blob-F1");
                submitted.Request.ComplianceSchemeId.Should().BeNull();
                submitted.Request.SubmissionPeriodId.Should().Be(5);
                submitted.Request.RegulatorNation.Should().Be("GB-ENG");
                submitted.Request.ApplicationReferenceNumber.Should().Be("PEPR001");
                submitted.Request.SubmissionDate.Kind.Should().Be(DateTimeKind.Utc);
                submitted.ReplayTs.Kind.Should().Be(DateTimeKind.Utc);

                var approval = (ApplicationSubmittedStep)plan.Steps[1];
                approval.Request.SubmissionDate.Should().Be(DateTime.SpecifyKind(T0.AddHours(1).AddMinutes(-1), DateTimeKind.Utc));
                approval.ReplayTs.Should().Be(DateTime.SpecifyKind(T0.AddHours(1), DateTimeKind.Utc));

                ((DecisionStep)plan.Steps[2]).Request.EventName.Should().Be(RegistrationEventNames.AcceptedByRegulator);
                plan.EventsSkipped.Should().Be(0);
                plan.Warnings.Should().BeEmpty();
            }
        }

        [TestMethod]
        public void Plan_ComplianceScheme_SetsSchemeIdAndCsoPeriod()
        {
            var csId = Guid.NewGuid();
            var rows = new List<RegistrationHistoryStaging>
            {
                Row(RegistrationHistoryEventTypes.Submitted, "F1", T0, arn: "PEPR001", complianceSchemeId: csId.ToString(), journey: "CsoLargeProducer"),
            };

            var submitted = (SubmittedStep)Plan(rows).Plan!.Steps.Single();

            submitted.Request.ComplianceSchemeId.Should().Be(csId);
            submitted.Request.SubmissionPeriodId.Should().Be(3);
        }

        [TestMethod]
        public void Plan_SubmittedRowWithoutArn_FallsBackToItsApplicationSubmittedArn()
        {
            var rows = new List<RegistrationHistoryStaging>
            {
                Row(RegistrationHistoryEventTypes.Submitted, "F1", T0, arn: null),
                Row(RegistrationHistoryEventTypes.RegistrationApplicationSubmitted, "F1", T0.AddHours(1), arn: "PEPR777"),
            };

            var plan = Plan(rows).Plan!;

            ((SubmittedStep)plan.Steps[0]).Request.ApplicationReferenceNumber.Should().Be("PEPR777");
        }

        [TestMethod]
        public void Plan_ApplicationSubmittedWithoutArn_UsesFileArn()
        {
            var rows = new List<RegistrationHistoryStaging>
            {
                Row(RegistrationHistoryEventTypes.Submitted, "F1", T0, arn: "PEPR001"),
                Row(RegistrationHistoryEventTypes.RegistrationApplicationSubmitted, "F1", T0.AddHours(1), arn: null),
            };

            ((ApplicationSubmittedStep)Plan(rows).Plan!.Steps[1]).Request.ApplicationReferenceNumber.Should().Be("PEPR001");
        }

        [TestMethod]
        public void Plan_UnmappedAndDuplicateDecisions_AreSkippedWithCount()
        {
            var rows = new List<RegistrationHistoryStaging>
            {
                Row(RegistrationHistoryEventTypes.Submitted, "F1", T0, arn: "PEPR001"),
                Row(RegistrationHistoryEventTypes.RegulatorRegistrationDecision, "F1", T0.AddDays(1), decision: "Granted"),
                Row(RegistrationHistoryEventTypes.RegulatorRegistrationDecision, "F1", T0.AddDays(2), decision: "Accepted", eventDate: T0.AddDays(2)),
                Row(RegistrationHistoryEventTypes.RegulatorRegistrationDecision, "F1", T0.AddDays(2).AddSeconds(1), decision: "Accepted", eventDate: T0.AddDays(2)),
            };

            var plan = Plan(rows).Plan!;

            using (new AssertionScope())
            {
                plan.Steps.OfType<DecisionStep>().Should().ContainSingle();
                plan.EventsSkipped.Should().Be(2);
                plan.Warnings.Should().ContainSingle().Which.Should().Contain("Granted");
            }
        }

        [TestMethod]
        public void Plan_EventForUndeclaredFile_IsSkippedWithWarning()
        {
            var rows = new List<RegistrationHistoryStaging>
            {
                Row(RegistrationHistoryEventTypes.Submitted, "F1", T0, arn: "PEPR001"),
                Row(RegistrationHistoryEventTypes.RegulatorRegistrationDecision, "F2", T0.AddDays(1), decision: "Accepted"),
            };

            var plan = Plan(rows).Plan!;

            plan.Steps.Should().ContainSingle();
            plan.EventsSkipped.Should().Be(1);
            plan.Warnings.Should().ContainSingle().Which.Should().Contain("F2");
        }

        [TestMethod]
        public void Plan_UnknownPeriod_IsNotRequired()
        {
            var rows = new List<RegistrationHistoryStaging>
            {
                Row(RegistrationHistoryEventTypes.Submitted, "F1", T0, arn: "PEPR001", period: "January to December 2024"),
            };

            var outcome = Plan(rows).Outcome!;

            outcome.Status.Should().Be(SubmissionMigrationStatus.NotRequired);
            outcome.Reason.Should().Be(MigrationReasons.PeriodNotMapped);
        }

        [TestMethod]
        [DataRow(null, DisplayName = "file count unknown")]
        [DataRow(3, DisplayName = "fewer files migrated than declared")]
        public void Plan_ApprovalOnOrAfterSubsidiaryLateFeeEffectiveDate_WithIncompleteHistory_Fails(int? submissionFileCount)
        {
            var rows = new List<RegistrationHistoryStaging>
            {
                Row(RegistrationHistoryEventTypes.Submitted, "F1", T0, arn: "PEPR001", submissionFileCount: submissionFileCount),
                Row(RegistrationHistoryEventTypes.Submitted, "F3", T0.AddDays(1), arn: "PEPR001", submissionFileCount: submissionFileCount),
                Row(RegistrationHistoryEventTypes.RegistrationApplicationSubmitted, "F3", Cutover, arn: "PEPR001", eventDate: Cutover, submissionFileCount: submissionFileCount),
            };

            var result = Plan(rows);

            AssertFailed(result, MigrationReasons.PostCutoverApproval);
            result.Outcome!.Error.Should().Contain($"only 2 of {submissionFileCount?.ToString() ?? "unknown"} declared files");
        }

        [TestMethod]
        public void Plan_ApprovalOnOrAfterSubsidiaryLateFeeEffectiveDate_WithEveryDeclaredFile_Replays()
        {
            var rows = new List<RegistrationHistoryStaging>
            {
                Row(RegistrationHistoryEventTypes.Submitted, "F1", T0, arn: "PEPR001", submissionFileCount: 2),
                Row(RegistrationHistoryEventTypes.Submitted, "F2", T0.AddDays(1), arn: "PEPR001", submissionFileCount: 2),
                Row(RegistrationHistoryEventTypes.RegistrationApplicationSubmitted, "F2", Cutover, arn: "PEPR001", eventDate: Cutover, submissionFileCount: 2),
            };

            var result = Plan(rows);

            result.Outcome.Should().BeNull();
            result.Plan!.Steps.Should().HaveCount(3);
        }

        [TestMethod]
        public void Plan_NoRows_Fails() =>
            AssertFailed(RegistrationHistorySubmissionPlanner.Plan(new List<RegistrationHistoryStaging>(), Periods, Cutover), MigrationReasons.NoSubmittedFile);

        [TestMethod]
        public void Plan_NoSubmittedRow_Fails() =>
            AssertFailed(Plan(new List<RegistrationHistoryStaging> { Row(RegistrationHistoryEventTypes.RegulatorRegistrationDecision, "F1", T0, decision: "Accepted") }), MigrationReasons.NoSubmittedFile);

        [TestMethod]
        public void Plan_InvalidSubmissionId_Fails()
        {
            var row = Row(RegistrationHistoryEventTypes.Submitted, "F1", T0, arn: "PEPR001");
            row.SubmissionId = "not-a-guid";

            AssertFailed(Plan(new List<RegistrationHistoryStaging> { row }), MigrationReasons.InvalidData);
        }

        [TestMethod]
        public void Plan_InvalidComplianceSchemeId_Fails() =>
            AssertFailed(Plan(new List<RegistrationHistoryStaging> { Row(RegistrationHistoryEventTypes.Submitted, "F1", T0, arn: "PEPR001", complianceSchemeId: "bad") }), MigrationReasons.InvalidData);

        [TestMethod]
        public void Plan_MissingReplayTs_Fails()
        {
            var row = Row(RegistrationHistoryEventTypes.Submitted, "F1", T0, arn: "PEPR001");
            row.ReplayTs = null;

            AssertFailed(Plan(new List<RegistrationHistoryStaging> { row }), MigrationReasons.InvalidData);
        }

        [TestMethod]
        public void Plan_MissingBlobName_Fails() =>
            AssertFailed(Plan(new List<RegistrationHistoryStaging> { Row(RegistrationHistoryEventTypes.Submitted, "F1", T0, arn: "PEPR001", blobName: " ") }), MigrationReasons.BlobNotFound);

        [TestMethod]
        public void Plan_MissingNation_Fails() =>
            AssertFailed(Plan(new List<RegistrationHistoryStaging> { Row(RegistrationHistoryEventTypes.Submitted, "F1", T0, arn: "PEPR001", nation: null) }), MigrationReasons.NationMissing);

        [TestMethod]
        public void Plan_NoArnAnywhere_Fails() =>
            AssertFailed(Plan(new List<RegistrationHistoryStaging> { Row(RegistrationHistoryEventTypes.Submitted, "F1", T0, arn: null) }), MigrationReasons.ArnMissing);

        [TestMethod]
        public void Plan_UnknownEventType_IsSkipped()
        {
            var rows = new List<RegistrationHistoryStaging>
            {
                Row(RegistrationHistoryEventTypes.Submitted, "F1", T0, arn: "PEPR001"),
                Row("SomethingElse", "F1", T0.AddDays(1)),
            };

            var plan = Plan(rows).Plan!;

            plan.EventsSkipped.Should().Be(1);
            plan.Warnings.Should().ContainSingle().Which.Should().Contain("SomethingElse");
        }

        [TestMethod]
        public void Plan_NullArguments_Throw()
        {
            using (new AssertionScope())
            {
                ((Action)(() => RegistrationHistorySubmissionPlanner.Plan(null!, Periods, Cutover))).Should().Throw<ArgumentNullException>();
                ((Action)(() => RegistrationHistorySubmissionPlanner.Plan(new List<RegistrationHistoryStaging>(), null!, Cutover))).Should().Throw<ArgumentNullException>();
            }
        }

        private SubmissionPlanResult Plan(IReadOnlyList<RegistrationHistoryStaging> rows) =>
            RegistrationHistorySubmissionPlanner.Plan(rows, Periods, Cutover);

        private static void AssertFailed(SubmissionPlanResult result, string reason)
        {
            result.Plan.Should().BeNull();
            result.Outcome!.Status.Should().Be(SubmissionMigrationStatus.Failed);
            result.Outcome.Reason.Should().Be(reason);
            result.Outcome.Error.Should().NotBeNullOrWhiteSpace();
        }

        private RegistrationHistoryStaging Row(
            string eventType,
            string fileId,
            DateTime replayTs,
            string? arn = null,
            string? decision = null,
            DateTime? eventDate = null,
            string? complianceSchemeId = null,
            string? journey = "DirectLargeProducer",
            string period = "January to December 2026",
            string? nation = "GB-ENG",
            string? blobName = null,
            int? submissionFileCount = null) => new()
            {
                Id = _nextId++,
                SubmissionId = _submissionId.ToString(),
                FileId = fileId,
                BlobName = blobName ?? $"blob-{fileId}",
                ComplianceSchemeId = complianceSchemeId,
                SubmissionPeriod = period,
                RegistrationJourney = journey,
                RegulatorNation = nation,
                SubmissionFileCount = submissionFileCount,
                EventType = eventType,
                EventTypeOrder = eventType switch
                {
                    RegistrationHistoryEventTypes.Submitted => 1,
                    RegistrationHistoryEventTypes.RegistrationApplicationSubmitted => 2,
                    _ => 3,
                },
                ReplayTs = replayTs,
                EventDate = eventDate ?? replayTs,
                ApplicationReferenceNumber = arn,
                Decision = decision,
            };
    }
}
