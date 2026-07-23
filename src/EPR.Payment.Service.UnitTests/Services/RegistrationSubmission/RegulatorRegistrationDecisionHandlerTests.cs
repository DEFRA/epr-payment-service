using EPR.Payment.Service.Common.Data.Interfaces.Repositories.RegistrationSubmission;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationSubmission;
using EPR.Payment.Service.Services.RegistrationSubmission;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.Extensions.Logging;
using Moq;

namespace EPR.Payment.Service.UnitTests.Services.RegistrationSubmission
{
    [TestClass]
    public class RegulatorRegistrationDecisionHandlerTests
    {
        private Mock<IRegistrationSubmissionDataEventRepository> _repositoryMock = null!;
        private Mock<ILogger<RegulatorRegistrationDecisionHandler>> _loggerMock = null!;
        private RegulatorRegistrationDecisionHandler _sut = null!;
        private CancellationToken _ct;

        [TestInitialize]
        public void Init()
        {
            _repositoryMock = new Mock<IRegistrationSubmissionDataEventRepository>();
            _loggerMock = new Mock<ILogger<RegulatorRegistrationDecisionHandler>>();
            _sut = new RegulatorRegistrationDecisionHandler(_repositoryMock.Object, _loggerMock.Object);
            _ct = CancellationToken.None;
        }

        [TestMethod]
        public void Constructor_NullDependency_Throws()
        {
            using (new AssertionScope())
            {
                Action a1 = () => new RegulatorRegistrationDecisionHandler(null!, _loggerMock.Object);
                Action a2 = () => new RegulatorRegistrationDecisionHandler(_repositoryMock.Object, null!);
                a1.Should().Throw<ArgumentNullException>();
                a2.Should().Throw<ArgumentNullException>();
            }
        }

        [TestMethod]
        public async Task HandleAsync_NullRequest_Throws()
        {
            Func<Task> act = () => _sut.HandleAsync(null!, _ct);
            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [DataTestMethod]
        [DataRow("AcceptedByRegulator")]
        [DataRow("RejectedByRegulator")]
        [DataRow("QueriedByRegulator")]
        [DataRow("CancelledByRegulator")]
        public async Task HandleAsync_MatchingSubmissionData_InvokesRepoWithExpectedArgs(string eventName)
        {
            var request = NewRequest(eventName);
            _repositoryMock
                .Setup(r => r.AddEventForLatestSubmissionAsync(
                    request.SubmissionId,
                    eventName,
                    request.DecisionDate,
                    _ct))
                .ReturnsAsync(Guid.NewGuid());

            await _sut.HandleAsync(request, _ct);

            _repositoryMock.Verify(
                r => r.AddEventForLatestSubmissionAsync(
                    request.SubmissionId,
                    eventName,
                    request.DecisionDate,
                    _ct),
                Times.Once);
        }

        [TestMethod]
        public async Task HandleAsync_NoMatchingSubmissionData_ThrowsSubmissionDataNotFoundForEventException()
        {
            var request = NewRequest();
            _repositoryMock
                .Setup(r => r.AddEventForLatestSubmissionAsync(
                    It.IsAny<Guid>(),
                    It.IsAny<string>(),
                    It.IsAny<DateTime>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Guid?)null);

            Func<Task> act = () => _sut.HandleAsync(request, _ct);

            var thrown = (await act.Should().ThrowAsync<SubmissionDataNotFoundForEventException>()).Which;

            using (new AssertionScope())
            {
                thrown.SubmissionId.Should().Be(request.SubmissionId);
                thrown.EventName.Should().Be(request.EventName);
            }
        }

        [TestMethod]
        public async Task HandleAsync_NoMatchingSubmissionData_LogsWarning()
        {
            var request = NewRequest();
            _repositoryMock
                .Setup(r => r.AddEventForLatestSubmissionAsync(
                    It.IsAny<Guid>(),
                    It.IsAny<string>(),
                    It.IsAny<DateTime>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Guid?)null);

            Func<Task> act = () => _sut.HandleAsync(request, _ct);
            await act.Should().ThrowAsync<SubmissionDataNotFoundForEventException>();

            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("No RegistrationSubmissionData row found")),
                    It.IsAny<Exception?>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [TestMethod]
        public async Task HandleAsync_Success_LogsRecordedEvent()
        {
            var request = NewRequest();
            var newId = Guid.NewGuid();
            _repositoryMock
                .Setup(r => r.AddEventForLatestSubmissionAsync(
                    It.IsAny<Guid>(),
                    It.IsAny<string>(),
                    It.IsAny<DateTime>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(newId);

            await _sut.HandleAsync(request, _ct);

            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Recorded")),
                    It.IsAny<Exception?>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        private static RegulatorRegistrationDecisionRequest NewRequest(string eventName = "AcceptedByRegulator") => new()
        {
            SubmissionId = Guid.NewGuid(),
            EventName = eventName,
            DecisionDate = new DateTime(2026, 7, 22, 10, 0, 0, DateTimeKind.Utc),
        };
    }
}
