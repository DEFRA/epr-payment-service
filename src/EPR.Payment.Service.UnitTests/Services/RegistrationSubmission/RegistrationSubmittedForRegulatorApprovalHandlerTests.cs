using EPR.Payment.Service.Common.Data.DataModels;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.RegistrationSubmission;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationSubmission;
using EPR.Payment.Service.Services.Interfaces.RegistrationSubmission;
using EPR.Payment.Service.Services.RegistrationSubmission;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.Extensions.Logging;
using Moq;

namespace EPR.Payment.Service.UnitTests.Services.RegistrationSubmission
{
    [TestClass]
    public class RegistrationSubmittedForRegulatorApprovalHandlerTests
    {
        private static readonly DateTime Today = new(2026, 7, 24, 0, 0, 0, DateTimeKind.Utc);

        private Mock<IRegistrationSubmissionDataEventRepository> _repositoryMock = null!;
        private Mock<IRegistrationSubmissionDataRepository> _rsdRepositoryMock = null!;
        private Mock<IRegistrationFeeSnapshotHandler> _snapshotHandlerMock = null!;
        private Mock<TimeProvider> _timeProviderMock = null!;
        private Mock<ILogger<RegistrationSubmittedForRegulatorApprovalHandler>> _loggerMock = null!;
        private RegistrationSubmittedForRegulatorApprovalHandler _sut = null!;
        private CancellationToken _ct;

        [TestInitialize]
        public void Init()
        {
            _repositoryMock = new Mock<IRegistrationSubmissionDataEventRepository>();
            _rsdRepositoryMock = new Mock<IRegistrationSubmissionDataRepository>();
            _snapshotHandlerMock = new Mock<IRegistrationFeeSnapshotHandler>();
            _timeProviderMock = new Mock<TimeProvider>();
            _timeProviderMock.Setup(t => t.GetUtcNow()).Returns(new DateTimeOffset(Today, TimeSpan.Zero));
            _loggerMock = new Mock<ILogger<RegistrationSubmittedForRegulatorApprovalHandler>>();
            _rsdRepositoryMock
                .Setup(r => r.GetAllForSubmissionAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Array.Empty<RegistrationSubmissionData>());
            _sut = new RegistrationSubmittedForRegulatorApprovalHandler(
                _repositoryMock.Object,
                _rsdRepositoryMock.Object,
                _snapshotHandlerMock.Object,
                _timeProviderMock.Object,
                _loggerMock.Object);
            _ct = CancellationToken.None;
        }

        [TestMethod]
        public void Constructor_NullDependency_Throws()
        {
            using (new AssertionScope())
            {
                ((Action)(() => new RegistrationSubmittedForRegulatorApprovalHandler(null!, _rsdRepositoryMock.Object, _snapshotHandlerMock.Object, _timeProviderMock.Object, _loggerMock.Object)))
                    .Should().Throw<ArgumentNullException>();
                ((Action)(() => new RegistrationSubmittedForRegulatorApprovalHandler(_repositoryMock.Object, null!, _snapshotHandlerMock.Object, _timeProviderMock.Object, _loggerMock.Object)))
                    .Should().Throw<ArgumentNullException>();
                ((Action)(() => new RegistrationSubmittedForRegulatorApprovalHandler(_repositoryMock.Object, _rsdRepositoryMock.Object, null!, _timeProviderMock.Object, _loggerMock.Object)))
                    .Should().Throw<ArgumentNullException>();
                ((Action)(() => new RegistrationSubmittedForRegulatorApprovalHandler(_repositoryMock.Object, _rsdRepositoryMock.Object, _snapshotHandlerMock.Object, null!, _loggerMock.Object)))
                    .Should().Throw<ArgumentNullException>();
                ((Action)(() => new RegistrationSubmittedForRegulatorApprovalHandler(_repositoryMock.Object, _rsdRepositoryMock.Object, _snapshotHandlerMock.Object, _timeProviderMock.Object, null!)))
                    .Should().Throw<ArgumentNullException>();
            }
        }

        [TestMethod]
        public async Task HandleAsync_NullRequest_Throws()
        {
            Func<Task> act = () => _sut.HandleAsync(null!, _ct);
            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [TestMethod]
        public async Task HandleAsync_MatchingSubmissionData_InvokesRepoWithExpectedArgs()
        {
            var request = NewRequest();
            _repositoryMock
                .Setup(r => r.AddEventForLatestSubmissionAsync(request.SubmissionId, RegistrationSubmittedForRegulatorApprovalHandler.EventName, request.SubmissionDate, _ct))
                .ReturnsAsync(Guid.NewGuid());

            await _sut.HandleAsync(request, _ct);

            _repositoryMock.Verify(
                r => r.AddEventForLatestSubmissionAsync(request.SubmissionId, RegistrationSubmittedForRegulatorApprovalHandler.EventName, request.SubmissionDate, _ct),
                Times.Once);
        }

        [TestMethod]
        public async Task HandleAsync_NoMatchingSubmissionData_ThrowsSubmissionDataNotFoundForEventException()
        {
            var request = NewRequest();
            _repositoryMock
                .Setup(r => r.AddEventForLatestSubmissionAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Guid?)null);

            Func<Task> act = () => _sut.HandleAsync(request, _ct);

            var thrown = (await act.Should().ThrowAsync<SubmissionDataNotFoundForEventException>()).Which;

            using (new AssertionScope())
            {
                thrown.SubmissionId.Should().Be(request.SubmissionId);
                thrown.EventName.Should().Be(RegistrationSubmittedForRegulatorApprovalHandler.EventName);
                _snapshotHandlerMock.Verify(
                    h => h.HandleAsync(It.IsAny<RegistrationSubmissionData>(), It.IsAny<DateTime>(), It.IsAny<SubmissionLifecycle>(), It.IsAny<CancellationToken>()),
                    Times.Never);
            }
        }

        [TestMethod]
        public async Task HandleAsync_EventRecordedButNoRsdRowsFound_SkipsSnapshotWithoutError()
        {
            var request = NewRequest();
            _repositoryMock
                .Setup(r => r.AddEventForLatestSubmissionAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Guid.NewGuid());

            await _sut.HandleAsync(request, _ct);

            _snapshotHandlerMock.Verify(
                h => h.HandleAsync(It.IsAny<RegistrationSubmissionData>(), It.IsAny<DateTime>(), It.IsAny<SubmissionLifecycle>(), It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [TestMethod]
        public async Task HandleAsync_LatestNonRejected_InvokesSnapshotHandlerOnce()
        {
            var request = NewRequest();
            var rsd = new RegistrationSubmissionData
            {
                Id = Guid.NewGuid(),
                SubmissionId = request.SubmissionId,
                RegistrationBlobName = "blob",
                CreatedDate = new DateTimeOffset(Today.AddDays(-1), TimeSpan.Zero),
                RegulatorNation = "GB-ENG",
                ApplicationReferenceNumber = request.ApplicationReferenceNumber,
                Producers = new List<RegistrationSubmissionProducer>(),
                Events = new List<RegistrationSubmissionDataEvent>(),
            };
            _repositoryMock
                .Setup(r => r.AddEventForLatestSubmissionAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Guid.NewGuid());
            _rsdRepositoryMock
                .Setup(r => r.GetAllForSubmissionAsync(request.SubmissionId, _ct))
                .ReturnsAsync(new[] { rsd });

            await _sut.HandleAsync(request, _ct);

            _snapshotHandlerMock.Verify(
                h => h.HandleAsync(rsd, request.SubmissionDate, It.IsAny<SubmissionLifecycle>(), _ct),
                Times.Once);
        }

        [TestMethod]
        public async Task HandleAsync_SnapshotHandlerThrows_PropagatesToCaller()
        {
            var request = NewRequest();
            var rsd = new RegistrationSubmissionData
            {
                Id = Guid.NewGuid(),
                SubmissionId = request.SubmissionId,
                RegistrationBlobName = "blob",
                CreatedDate = new DateTimeOffset(Today.AddDays(-1), TimeSpan.Zero),
                RegulatorNation = "GB-ENG",
                ApplicationReferenceNumber = request.ApplicationReferenceNumber,
                Producers = new List<RegistrationSubmissionProducer>(),
                Events = new List<RegistrationSubmissionDataEvent>(),
            };
            _repositoryMock
                .Setup(r => r.AddEventForLatestSubmissionAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Guid.NewGuid());
            _rsdRepositoryMock
                .Setup(r => r.GetAllForSubmissionAsync(request.SubmissionId, _ct))
                .ReturnsAsync(new[] { rsd });
            _snapshotHandlerMock
                .Setup(h => h.HandleAsync(It.IsAny<RegistrationSubmissionData>(), It.IsAny<DateTime>(), It.IsAny<SubmissionLifecycle>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InvalidOperationException("boom"));

            Func<Task> act = () => _sut.HandleAsync(request, _ct);

            await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("boom");
        }

        private static RegistrationSubmittedForRegulatorApprovalRequest NewRequest() => new()
        {
            SubmissionId = Guid.NewGuid(),
            ApplicationReferenceNumber = "PEPR2601234",
            SubmissionDate = new DateTime(2026, 7, 20, 10, 0, 0, DateTimeKind.Utc),
        };
    }
}
