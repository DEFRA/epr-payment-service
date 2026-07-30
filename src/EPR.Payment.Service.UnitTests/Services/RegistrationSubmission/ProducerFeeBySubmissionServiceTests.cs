using EPR.Payment.Service.Common.Constants;
using EPR.Payment.Service.Common.Data.DataModels;
using EPR.Payment.Service.Common.Data.DataModels.Lookups;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.RegistrationSubmission;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.Producer;
using EPR.Payment.Service.Common.Dtos.Response.RegistrationFees;
using EPR.Payment.Service.Common.Dtos.Response.RegistrationFees.Producer;
using EPR.Payment.Service.Services.Interfaces.RegistrationFees.Producer;
using EPR.Payment.Service.Services.RegistrationSubmission;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.Extensions.Logging;
using Moq;

namespace EPR.Payment.Service.UnitTests.Services.RegistrationSubmission
{
    [TestClass]
    public class ProducerFeeBySubmissionServiceTests
    {
        private static readonly DateTime Today = new(2026, 7, 24, 0, 0, 0, DateTimeKind.Utc);
        private static readonly DateTime Deadline = new(2026, 4, 1, 0, 0, 0, DateTimeKind.Utc);

        private Mock<IRegistrationSubmissionDataRepository> _repositoryMock = null!;
        private Mock<IProducerFeesCalculatorService> _calculatorMock = null!;
        private Mock<TimeProvider> _timeProviderMock = null!;
        private Mock<ILogger<ProducerFeeBySubmissionService>> _loggerMock = null!;
        private ProducerFeeBySubmissionService _sut = null!;
        private RegistrationFeesResponseDto _calculatorResponse = null!;
        private ProducerRegistrationFeesRequestDto? _capturedRequest;

        [TestInitialize]
        public void Init()
        {
            _repositoryMock = new Mock<IRegistrationSubmissionDataRepository>();
            _calculatorMock = new Mock<IProducerFeesCalculatorService>();
            _timeProviderMock = new Mock<TimeProvider>();
            _timeProviderMock.Setup(t => t.GetUtcNow()).Returns(new DateTimeOffset(Today, TimeSpan.Zero));
            _loggerMock = new Mock<ILogger<ProducerFeeBySubmissionService>>();
            _calculatorResponse = new RegistrationFeesResponseDto
            {
                SubsidiariesFeeBreakdown = new SubsidiariesFeeBreakdown(),
            };
            _capturedRequest = null;
            _calculatorMock
                .Setup(c => c.CalculateFeesAsync(It.IsAny<ProducerRegistrationFeesRequestDto>(), It.IsAny<CancellationToken>()))
                .Callback<ProducerRegistrationFeesRequestDto, CancellationToken>((r, _) => _capturedRequest = r)
                .ReturnsAsync(_calculatorResponse);

            _sut = new ProducerFeeBySubmissionService(
                _repositoryMock.Object,
                _calculatorMock.Object,
                _timeProviderMock.Object,
                _loggerMock.Object);
        }

        [TestMethod]
        public void Constructor_NullDependency_Throws()
        {
            using (new AssertionScope())
            {
                ((Action)(() => new ProducerFeeBySubmissionService(null!, _calculatorMock.Object, _timeProviderMock.Object, _loggerMock.Object)))
                    .Should().Throw<ArgumentNullException>();
                ((Action)(() => new ProducerFeeBySubmissionService(_repositoryMock.Object, null!, _timeProviderMock.Object, _loggerMock.Object)))
                    .Should().Throw<ArgumentNullException>();
                ((Action)(() => new ProducerFeeBySubmissionService(_repositoryMock.Object, _calculatorMock.Object, null!, _loggerMock.Object)))
                    .Should().Throw<ArgumentNullException>();
                ((Action)(() => new ProducerFeeBySubmissionService(_repositoryMock.Object, _calculatorMock.Object, _timeProviderMock.Object, null!)))
                    .Should().Throw<ArgumentNullException>();
            }
        }

        [TestMethod]
        public async Task GetFeesAsync_NoRecords_ReturnsNullAndDoesNotCallCalculator()
        {
            _repositoryMock.Setup(r => r.GetAllForSubmissionAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Array.Empty<RegistrationSubmissionData>());

            var result = await _sut.GetFeesAsync(Guid.NewGuid(), CancellationToken.None);

            using (new AssertionScope())
            {
                result.Should().BeNull();
                _calculatorMock.Verify(c => c.CalculateFeesAsync(It.IsAny<ProducerRegistrationFeesRequestDto>(), It.IsAny<CancellationToken>()), Times.Never);
            }
        }

        [TestMethod]
        public async Task GetFeesAsync_AllRejected_ReturnsNull()
        {
            var rejected = BuildRecord(
                created: Today.AddMonths(-1),
                events: new[]
                {
                    (RegistrationEventNames.SubmittedForRegulatorApproval, Today.AddMonths(-1)),
                    (RegistrationEventNames.RejectedByRegulator, Today.AddDays(-15)),
                });
            SetupRepo(rejected);

            var result = await _sut.GetFeesAsync(Guid.NewGuid(), CancellationToken.None);

            result.Should().BeNull();
        }

        [TestMethod]
        public async Task GetFeesAsync_LatestNonRejectedHasNoProducers_ReturnsNull()
        {
            var record = BuildRecord(created: Today.AddDays(-1), producers: Array.Empty<RegistrationSubmissionProducer>());
            SetupRepo(record);

            var result = await _sut.GetFeesAsync(Guid.NewGuid(), CancellationToken.None);

            using (new AssertionScope())
            {
                result.Should().BeNull();
                _calculatorMock.Verify(c => c.CalculateFeesAsync(It.IsAny<ProducerRegistrationFeesRequestDto>(), It.IsAny<CancellationToken>()), Times.Never);
            }
        }

        [TestMethod]
        public async Task GetFeesAsync_MultipleProducers_LogsWarningAndUsesFirst()
        {
            var producers = new[]
            {
                Producer("ORG-A", newJoiner: false),
                Producer("ORG-B", newJoiner: false),
            };
            var record = BuildRecord(created: Today.AddDays(-1), producers: producers);
            SetupRepo(record);

            await _sut.GetFeesAsync(Guid.NewGuid(), CancellationToken.None);
            var captured = CapturedRequest();

            captured.ProducerType.Should().Be("Large");
            _calculatorMock.Verify(c => c.CalculateFeesAsync(It.IsAny<ProducerRegistrationFeesRequestDto>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [TestMethod]
        public async Task GetFeesAsync_NotYetSubmittedAndTodayAfterDeadline_IsLateFeeApplicableTrue()
        {
            // Latest non-rejected has NO SubmittedForRegulatorApproval event → compare-date = today.
            // Today (2026-07-24) > Deadline (2026-04-01) → submissionLevelLate = true.
            // No first submission → noFirstSubmission = true → IsLateFeeApplicable = true.
            var record = BuildRecord(created: Today.AddDays(-1), producers: new[] { Producer(newJoiner: false) });
            SetupRepo(record);

            await _sut.GetFeesAsync(Guid.NewGuid(), CancellationToken.None);
            var captured = CapturedRequest();

            captured.IsLateFeeApplicable.Should().BeTrue();
        }

        [TestMethod]
        public async Task GetFeesAsync_OriginalLate_IsLateFeeApplicableTrue()
        {
            var lateFirstSubmit = Deadline.AddDays(5);
            var record = BuildRecord(
                created: Today.AddMonths(-2),
                events: new[] { (RegistrationEventNames.SubmittedForRegulatorApproval, lateFirstSubmit) },
                producers: new[] { Producer(newJoiner: false) });
            SetupRepo(record);

            await _sut.GetFeesAsync(Guid.NewGuid(), CancellationToken.None);
            var captured = CapturedRequest();

            captured.IsLateFeeApplicable.Should().BeTrue();
        }

        [TestMethod]
        public async Task GetFeesAsync_OriginalOnTimeAndLatestOnTime_NotLateFee()
        {
            var onTime = Deadline.AddDays(-10);
            var first = BuildRecord(
                created: onTime.AddDays(-1),
                events: new[] { (RegistrationEventNames.SubmittedForRegulatorApproval, onTime) });
            var latest = BuildRecord(
                created: onTime.AddDays(1),
                events: new[] { (RegistrationEventNames.SubmittedForRegulatorApproval, onTime.AddDays(2)) },
                producers: new[] { Producer(newJoiner: false) });
            SetupRepo(first, latest);

            await _sut.GetFeesAsync(Guid.NewGuid(), CancellationToken.None);
            var captured = CapturedRequest();

            captured.IsLateFeeApplicable.Should().BeFalse();
        }

        [TestMethod]
        public async Task GetFeesAsync_IsNewJoinerTrueButOriginalOnTime_DoesNotSetIsLateFeeApplicable()
        {
            // Regression guard: direct-producer must NOT branch on IsNewJoiner.
            // Setup: first submission on time (isOriginalLate=false, noFirstSubmission=false),
            // latest not yet submitted, today past deadline (submissionLevelLate=true),
            // producer.IsNewJoiner = true. CS would set late-fee here (via the dropped clause).
            // Producer path must return false.
            var onTimeFirstSubmit = Deadline.AddDays(-10);
            var first = BuildRecord(
                created: onTimeFirstSubmit.AddDays(-1),
                events: new[] { (RegistrationEventNames.SubmittedForRegulatorApproval, onTimeFirstSubmit) });
            var latest = BuildRecord(
                created: Today.AddDays(-1),
                producers: new[] { Producer("ORG-1", newJoiner: true) });
            SetupRepo(first, latest);

            await _sut.GetFeesAsync(Guid.NewGuid(), CancellationToken.None);
            var captured = CapturedRequest();

            captured.IsLateFeeApplicable.Should().BeFalse();
        }

        [TestMethod]
        public async Task GetFeesAsync_CalcDateIsFirstSubmittedForApproval_WhenPresent()
        {
            var firstApproval = new DateTime(2026, 6, 15, 0, 0, 0, DateTimeKind.Utc);
            var record = BuildRecord(
                created: firstApproval.AddDays(-1),
                events: new[] { (RegistrationEventNames.SubmittedForRegulatorApproval, firstApproval) },
                producers: new[] { Producer() });
            SetupRepo(record);

            await _sut.GetFeesAsync(Guid.NewGuid(), CancellationToken.None);
            var captured = CapturedRequest();

            captured.SubmissionDate.Should().Be(firstApproval);
        }

        [TestMethod]
        public async Task GetFeesAsync_CalcDateIsToday_WhenNoFirstSubmissionYet()
        {
            var record = BuildRecord(created: Today.AddDays(-1), producers: new[] { Producer() });
            SetupRepo(record);

            await _sut.GetFeesAsync(Guid.NewGuid(), CancellationToken.None);
            var captured = CapturedRequest();

            captured.SubmissionDate.Should().Be(Today);
        }

        [TestMethod]
        public async Task GetFeesAsync_RegulatorAndAppRefTakenFromLatestNonRejectedRecord()
        {
            var first = BuildRecord(
                created: Today.AddDays(-30),
                regulatorNation: "GB-SCT",
                applicationReferenceNumber: "OLD-REF");
            var latest = BuildRecord(
                created: Today.AddDays(-1),
                regulatorNation: "GB-ENG",
                applicationReferenceNumber: "NEW-REF",
                producers: new[] { Producer() });
            SetupRepo(first, latest);

            await _sut.GetFeesAsync(Guid.NewGuid(), CancellationToken.None);
            var captured = CapturedRequest();

            using (new AssertionScope())
            {
                captured.Regulator.Should().Be("GB-ENG");
                captured.ApplicationReferenceNumber.Should().Be("NEW-REF");
            }
        }

        [TestMethod]
        public async Task GetFeesAsync_ReturnsCalculatorResponse()
        {
            var record = BuildRecord(created: Today.AddDays(-1), producers: new[] { Producer() });
            SetupRepo(record);

            var result = await _sut.GetFeesAsync(Guid.NewGuid(), CancellationToken.None);

            result.Should().BeSameAs(_calculatorResponse);
        }

        [TestMethod]
        public async Task GetFeesAsync_MappedWithSubsidiaryCounts()
        {
            var producer = new RegistrationSubmissionProducer
            {
                OrganisationId = "ORG-1",
                OrganisationSize = "Large",
                IsOnlineMarketplace = true,
                IsClosedLoopRecycling = false,
                IsNewJoiner = false,
                Subsidiaries = new List<RegistrationSubmissionSubsidiary>
                {
                    new() { SubsidiaryId = "S1", IsOnlineMarketplace = true, IsClosedLoopRecycling = false },
                    new() { SubsidiaryId = "S2", IsOnlineMarketplace = false, IsClosedLoopRecycling = true },
                    new() { SubsidiaryId = "S3", IsOnlineMarketplace = false, IsClosedLoopRecycling = false },
                },
            };
            var record = BuildRecord(created: Today.AddDays(-1), producers: new[] { producer });
            SetupRepo(record);

            await _sut.GetFeesAsync(Guid.NewGuid(), CancellationToken.None);
            var captured = CapturedRequest();

            using (new AssertionScope())
            {
                captured.ProducerType.Should().Be("Large");
                captured.IsProducerOnlineMarketplace.Should().BeTrue();
                captured.IsClosedLoopRecycling.Should().BeFalse();
                captured.NumberOfSubsidiaries.Should().Be(3);
                captured.NoOfSubsidiariesOnlineMarketplace.Should().Be(1);
                captured.NoOfSubsidiariesClosedLoopRecycling.Should().Be(1);
            }
        }

        // -------- helpers --------

        private void SetupRepo(params RegistrationSubmissionData[] records)
        {
            _repositoryMock
                .Setup(r => r.GetAllForSubmissionAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(records);
        }

        private ProducerRegistrationFeesRequestDto CapturedRequest()
        {
            _capturedRequest.Should().NotBeNull("calculator should have been invoked");
            return _capturedRequest!;
        }

        private static RegistrationSubmissionData BuildRecord(
            DateTime created,
            string regulatorNation = "GB-ENG",
            string applicationReferenceNumber = "PEPR2601234",
            (string EventName, DateTime EventDate)[]? events = null,
            RegistrationSubmissionProducer[]? producers = null)
        {
            return new RegistrationSubmissionData
            {
                Id = Guid.NewGuid(),
                SubmissionId = Guid.NewGuid(),
                RegistrationBlobName = $"blob-{Guid.NewGuid()}",
                SubmissionPeriodId = 1,
                SubmissionDate = created,
                CreatedDate = new DateTimeOffset(created, TimeSpan.Zero),
                RegulatorNation = regulatorNation,
                ApplicationReferenceNumber = applicationReferenceNumber,
                Events = (events ?? Array.Empty<(string, DateTime)>())
                    .Select(e => new RegistrationSubmissionDataEvent
                    {
                        EventName = e.EventName,
                        EventDate = e.EventDate,
                    })
                    .ToList(),
                Producers = (producers ?? new[] { Producer() }).ToList(),
                SubmissionPeriodWindow = new SubmissionPeriod
                {
                    Id = 1,
                    WindowType = "LargeProducer",
                    DeadlineDate = Deadline,
                    OpeningDate = Deadline.AddMonths(-6),
                    ClosingDate = Deadline.AddMonths(6),
                    RegistrationYear = 2026,
                },
            };
        }

        private static RegistrationSubmissionProducer Producer(string organisationId = "ORG-1", bool newJoiner = false)
        {
            return new RegistrationSubmissionProducer
            {
                OrganisationId = organisationId,
                OrganisationSize = "Large",
                IsNewJoiner = newJoiner,
                Subsidiaries = new List<RegistrationSubmissionSubsidiary>(),
            };
        }
    }
}
