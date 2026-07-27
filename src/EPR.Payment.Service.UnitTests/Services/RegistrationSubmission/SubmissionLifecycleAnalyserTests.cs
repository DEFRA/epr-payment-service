using EPR.Payment.Service.Common.Constants;
using EPR.Payment.Service.Common.Data.DataModels;
using EPR.Payment.Service.Services.RegistrationSubmission;
using FluentAssertions;
using FluentAssertions.Execution;

namespace EPR.Payment.Service.UnitTests.Services.RegistrationSubmission
{
    [TestClass]
    public class SubmissionLifecycleAnalyserTests
    {
        private static readonly DateTime Today = new(2026, 7, 24, 0, 0, 0, DateTimeKind.Utc);
        private static readonly Guid SubmissionId = Guid.NewGuid();

        [TestMethod]
        public void Analyse_NullRecords_Throws()
        {
            Action act = () => SubmissionLifecycleAnalyser.Analyse(null!, Today);
            act.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Analyse_EmptyList_ReturnsNullFieldsAndCalcDateToday()
        {
            var result = SubmissionLifecycleAnalyser.Analyse(Array.Empty<RegistrationSubmissionData>(), Today);

            using (new AssertionScope())
            {
                result.FirstNonRejected.Should().BeNull();
                result.LatestNonRejected.Should().BeNull();
                result.FirstSubmittedForApprovalDate.Should().BeNull();
                result.LatestSubmittedForApprovalDate.Should().BeNull();
                result.CalcDate.Should().Be(Today);
            }
        }

        [TestMethod]
        public void Analyse_SingleNonRejectedWithoutSubmittedForApproval_SetsFirstEqualsLatestAndCalcDateToday()
        {
            var only = Record(created: new DateTime(2026, 6, 1));
            var result = SubmissionLifecycleAnalyser.Analyse(new[] { only }, Today);

            using (new AssertionScope())
            {
                result.FirstNonRejected.Should().BeSameAs(only);
                result.LatestNonRejected.Should().BeSameAs(only);
                result.FirstSubmittedForApprovalDate.Should().BeNull();
                result.LatestSubmittedForApprovalDate.Should().BeNull();
                result.CalcDate.Should().Be(Today);
            }
        }

        [TestMethod]
        public void Analyse_SingleNonRejectedWithSubmittedForApproval_UsesEventDateForCalc()
        {
            var eventDate = new DateTime(2026, 6, 10);
            var only = Record(
                created: new DateTime(2026, 6, 1),
                events: new[] { (RegistrationEventNames.SubmittedForRegulatorApproval, eventDate) });

            var result = SubmissionLifecycleAnalyser.Analyse(new[] { only }, Today);

            using (new AssertionScope())
            {
                result.FirstNonRejected.Should().BeSameAs(only);
                result.LatestNonRejected.Should().BeSameAs(only);
                result.FirstSubmittedForApprovalDate.Should().Be(eventDate);
                result.LatestSubmittedForApprovalDate.Should().Be(eventDate);
                result.CalcDate.Should().Be(eventDate);
            }
        }

        [TestMethod]
        public void Analyse_TwoNonRejectedBothSubmitted_FirstAndLatestDatesDiffer()
        {
            var firstEventDate = new DateTime(2026, 6, 10);
            var latestEventDate = new DateTime(2026, 7, 10);
            var first = Record(
                created: new DateTime(2026, 6, 1),
                events: new[] { (RegistrationEventNames.SubmittedForRegulatorApproval, firstEventDate) });
            var latest = Record(
                created: new DateTime(2026, 7, 1),
                events: new[] { (RegistrationEventNames.SubmittedForRegulatorApproval, latestEventDate) });

            var result = SubmissionLifecycleAnalyser.Analyse(new[] { first, latest }, Today);

            using (new AssertionScope())
            {
                result.FirstNonRejected.Should().BeSameAs(first);
                result.LatestNonRejected.Should().BeSameAs(latest);
                result.FirstSubmittedForApprovalDate.Should().Be(firstEventDate);
                result.LatestSubmittedForApprovalDate.Should().Be(latestEventDate);
                result.CalcDate.Should().Be(firstEventDate);
            }
        }

        [TestMethod]
        public void Analyse_FirstRejectedSecondNotRejected_PicksSecondForBoth()
        {
            var eventDate = new DateTime(2026, 7, 10);
            var rejectedFirst = Record(
                created: new DateTime(2026, 6, 1),
                events: new[]
                {
                    (RegistrationEventNames.SubmittedForRegulatorApproval, new DateTime(2026, 6, 10)),
                    (RegistrationEventNames.RejectedByRegulator, new DateTime(2026, 6, 15)),
                });
            var second = Record(
                created: new DateTime(2026, 7, 1),
                events: new[] { (RegistrationEventNames.SubmittedForRegulatorApproval, eventDate) });

            var result = SubmissionLifecycleAnalyser.Analyse(new[] { rejectedFirst, second }, Today);

            using (new AssertionScope())
            {
                result.FirstNonRejected.Should().BeSameAs(second);
                result.LatestNonRejected.Should().BeSameAs(second);
                result.FirstSubmittedForApprovalDate.Should().Be(eventDate);
                result.LatestSubmittedForApprovalDate.Should().Be(eventDate);
                result.CalcDate.Should().Be(eventDate);
            }
        }

        [TestMethod]
        public void Analyse_LatestRejectedOlderNotRejected_PicksOlderForBoth()
        {
            var eventDate = new DateTime(2026, 6, 10);
            var older = Record(
                created: new DateTime(2026, 6, 1),
                events: new[] { (RegistrationEventNames.SubmittedForRegulatorApproval, eventDate) });
            var rejectedLatest = Record(
                created: new DateTime(2026, 7, 1),
                events: new[]
                {
                    (RegistrationEventNames.SubmittedForRegulatorApproval, new DateTime(2026, 7, 10)),
                    (RegistrationEventNames.RejectedByRegulator, new DateTime(2026, 7, 15)),
                });

            var result = SubmissionLifecycleAnalyser.Analyse(new[] { older, rejectedLatest }, Today);

            using (new AssertionScope())
            {
                result.FirstNonRejected.Should().BeSameAs(older);
                result.LatestNonRejected.Should().BeSameAs(older);
                result.FirstSubmittedForApprovalDate.Should().Be(eventDate);
                result.LatestSubmittedForApprovalDate.Should().Be(eventDate);
                result.CalcDate.Should().Be(eventDate);
            }
        }

        [TestMethod]
        public void Analyse_AllRejected_ReturnsNulls()
        {
            var a = Record(
                created: new DateTime(2026, 6, 1),
                events: new[] { (RegistrationEventNames.RejectedByRegulator, new DateTime(2026, 6, 15)) });
            var b = Record(
                created: new DateTime(2026, 7, 1),
                events: new[] { (RegistrationEventNames.RejectedByRegulator, new DateTime(2026, 7, 15)) });

            var result = SubmissionLifecycleAnalyser.Analyse(new[] { a, b }, Today);

            using (new AssertionScope())
            {
                result.FirstNonRejected.Should().BeNull();
                result.LatestNonRejected.Should().BeNull();
                result.FirstSubmittedForApprovalDate.Should().BeNull();
                result.LatestSubmittedForApprovalDate.Should().BeNull();
                result.CalcDate.Should().Be(Today);
            }
        }

        [TestMethod]
        public void Analyse_LatestNotSubmittedYet_LatestApprovalDateNullFirstDateStillPopulated()
        {
            var firstEventDate = new DateTime(2026, 6, 10);
            var first = Record(
                created: new DateTime(2026, 6, 1),
                events: new[] { (RegistrationEventNames.SubmittedForRegulatorApproval, firstEventDate) });
            var latest = Record(created: new DateTime(2026, 7, 1));

            var result = SubmissionLifecycleAnalyser.Analyse(new[] { first, latest }, Today);

            using (new AssertionScope())
            {
                result.FirstNonRejected.Should().BeSameAs(first);
                result.LatestNonRejected.Should().BeSameAs(latest);
                result.FirstSubmittedForApprovalDate.Should().Be(firstEventDate);
                result.LatestSubmittedForApprovalDate.Should().BeNull();
                result.CalcDate.Should().Be(firstEventDate);
            }
        }

        [TestMethod]
        public void Analyse_MultipleSubmittedForApprovalOnOneRow_PicksLatestEventDate()
        {
            var earlier = new DateTime(2026, 6, 10);
            var later = new DateTime(2026, 6, 20);
            var only = Record(
                created: new DateTime(2026, 6, 1),
                events: new[]
                {
                    (RegistrationEventNames.SubmittedForRegulatorApproval, earlier),
                    (RegistrationEventNames.SubmittedForRegulatorApproval, later),
                });

            var result = SubmissionLifecycleAnalyser.Analyse(new[] { only }, Today);

            using (new AssertionScope())
            {
                result.FirstSubmittedForApprovalDate.Should().Be(later);
                result.LatestSubmittedForApprovalDate.Should().Be(later);
                result.CalcDate.Should().Be(later);
            }
        }

        private static RegistrationSubmissionData Record(DateTime created, (string EventName, DateTime EventDate)[]? events = null)
        {
            return new RegistrationSubmissionData
            {
                Id = Guid.NewGuid(),
                SubmissionId = SubmissionId,
                RegistrationBlobName = $"blob-{Guid.NewGuid()}",
                SubmissionPeriodId = 1,
                RegulatorNation = "GB-ENG",
                ApplicationReferenceNumber = "PEPR2601234",
                SubmissionDate = created,
                CreatedDate = created,
                Events = (events ?? Array.Empty<(string, DateTime)>())
                    .Select(e => new RegistrationSubmissionDataEvent
                    {
                        Id = Guid.NewGuid(),
                        EventName = e.EventName,
                        EventDate = e.EventDate,
                        CreatedDate = e.EventDate,
                    })
                    .ToList(),
            };
        }
    }
}
