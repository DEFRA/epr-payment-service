using Azure;
using EPR.Payment.Service.Common.Constants;
using EPR.Payment.Service.Common.Data.DataModels;
using EPR.Payment.Service.Common.Data.DataModels.Migration;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.Migration;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.RegistrationSubmission;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationSubmission;
using EPR.Payment.Service.Common.Services.Interfaces.RegistrationSubmission;
using EPR.Payment.Service.Services.Migration.RegistrationHistory;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;

namespace EPR.Payment.Service.UnitTests.Services.Migration.RegistrationHistory
{
    [TestClass]
    public class RegistrationHistorySubmissionReplayerTests
    {
        private static readonly DateTime DeclaredAt = new(2026, 3, 1, 9, 0, 0, DateTimeKind.Utc);
        private static readonly DateTime ApprovalAt = DeclaredAt.AddHours(1);
        private static readonly DateTime DecisionAt = DeclaredAt.AddDays(3);
        private static readonly DateTimeOffset Now = new(2026, 10, 5, 12, 0, 0, TimeSpan.Zero);

        private readonly Guid _runId = Guid.NewGuid();
        private readonly Guid _submissionId = Guid.NewGuid();
        private readonly Guid _dataId = Guid.NewGuid();

        private Mock<IRegistrationHistoryMigrationRepository> _repositoryMock = null!;
        private Mock<IRegistrationSubmissionDataRepository> _dataRepositoryMock = null!;
        private Mock<IDbContextTransaction> _transactionMock = null!;
        private Mock<IRegistrationSubmissionDataHandler> _dataHandlerMock = null!;
        private Mock<IRegistrationSubmittedForRegulatorApprovalHandler> _approvalHandlerMock = null!;
        private Mock<IRegulatorRegistrationDecisionHandler> _decisionHandlerMock = null!;
        private Mock<IRegistrationReplayHandlerFactory> _handlerFactoryMock = null!;
        private TimeProvider? _clockGivenToHandlers;
        private List<Dictionary<string, object>> _logScopes = null!;
        private RegistrationHistorySubmissionReplayer _sut = null!;

        [TestInitialize]
        public void Init()
        {
            _repositoryMock = new Mock<IRegistrationHistoryMigrationRepository>();
            _dataRepositoryMock = new Mock<IRegistrationSubmissionDataRepository>();
            _transactionMock = new Mock<IDbContextTransaction>();
            _dataHandlerMock = new Mock<IRegistrationSubmissionDataHandler>();
            _approvalHandlerMock = new Mock<IRegistrationSubmittedForRegulatorApprovalHandler>();
            _decisionHandlerMock = new Mock<IRegulatorRegistrationDecisionHandler>();
            _handlerFactoryMock = new Mock<IRegistrationReplayHandlerFactory>();

            _repositoryMock.Setup(r => r.BeginTransactionAsync(It.IsAny<CancellationToken>())).ReturnsAsync(_transactionMock.Object);
            _dataHandlerMock.Setup(h => h.HandleAsync(It.IsAny<CreateRegistrationSubmissionDataRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(_dataId);
            _handlerFactoryMock
                .Setup(f => f.Create(It.IsAny<IServiceProvider>(), It.IsAny<TimeProvider>()))
                .Callback<IServiceProvider, TimeProvider>((_, clock) => _clockGivenToHandlers = clock)
                .Returns(new ReplayHandlers(_dataHandlerMock.Object, _approvalHandlerMock.Object, _decisionHandlerMock.Object));
            SetupRecordedEvents(2);

            var providerMock = new Mock<IServiceProvider>();
            providerMock.Setup(p => p.GetService(typeof(IRegistrationHistoryMigrationRepository))).Returns(_repositoryMock.Object);
            providerMock.Setup(p => p.GetService(typeof(IRegistrationSubmissionDataRepository))).Returns(_dataRepositoryMock.Object);

            var scopeMock = new Mock<IServiceScope>();
            scopeMock.Setup(s => s.ServiceProvider).Returns(providerMock.Object);
            var scopeFactoryMock = new Mock<IServiceScopeFactory>();
            scopeFactoryMock.Setup(f => f.CreateScope()).Returns(scopeMock.Object);

            var timeProviderMock = new Mock<TimeProvider>();
            timeProviderMock.Setup(t => t.GetUtcNow()).Returns(Now);

            _logScopes = new List<Dictionary<string, object>>();
            var loggerMock = new Mock<ILogger<RegistrationHistorySubmissionReplayer>>();
            loggerMock.Setup(l => l.BeginScope(It.IsAny<Dictionary<string, object>>()))
                .Callback<Dictionary<string, object>>(_logScopes.Add);

            _sut = new RegistrationHistorySubmissionReplayer(
                scopeFactoryMock.Object,
                _handlerFactoryMock.Object,
                timeProviderMock.Object,
                loggerMock.Object);
        }

        [TestMethod]
        public void Constructor_NullDependency_Throws()
        {
            var factory = Mock.Of<IServiceScopeFactory>();
            var handlers = Mock.Of<IRegistrationReplayHandlerFactory>();
            var logger = Mock.Of<ILogger<RegistrationHistorySubmissionReplayer>>();

            using (new AssertionScope())
            {
                ((Action)(() => new RegistrationHistorySubmissionReplayer(null!, handlers, TimeProvider.System, logger))).Should().Throw<ArgumentNullException>();
                ((Action)(() => new RegistrationHistorySubmissionReplayer(factory, null!, TimeProvider.System, logger))).Should().Throw<ArgumentNullException>();
                ((Action)(() => new RegistrationHistorySubmissionReplayer(factory, handlers, null!, logger))).Should().Throw<ArgumentNullException>();
                ((Action)(() => new RegistrationHistorySubmissionReplayer(factory, handlers, TimeProvider.System, null!))).Should().Throw<ArgumentNullException>();
            }
        }

        [TestMethod]
        public async Task ReplayAsync_SubmissionAlreadyCaptured_IsNotRequiredAndWritesNothing()
        {
            _repositoryMock.Setup(r => r.SubmissionExistsAsync(_submissionId, It.IsAny<CancellationToken>())).ReturnsAsync(true);

            var outcome = await _sut.ReplayAsync(_runId, Plan(), CancellationToken.None);

            using (new AssertionScope())
            {
                outcome.Status.Should().Be(SubmissionMigrationStatus.NotRequired);
                outcome.Reason.Should().Be(MigrationReasons.AlreadyCaptured);
                _repositoryMock.Verify(r => r.BeginTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
                _dataHandlerMock.VerifyNoOtherCalls();
            }
        }

        [TestMethod]
        public async Task ReplayAsync_ReplaysStepsInOrderWithHistoricClockAndCommits()
        {
            var clockReadings = new List<DateTimeOffset>();
            _dataHandlerMock
                .Setup(h => h.HandleAsync(It.IsAny<CreateRegistrationSubmissionDataRequest>(), It.IsAny<CancellationToken>()))
                .Callback(() => clockReadings.Add(_clockGivenToHandlers!.GetUtcNow()))
                .ReturnsAsync(_dataId);
            _approvalHandlerMock
                .Setup(h => h.HandleAsync(It.IsAny<RegistrationSubmittedForRegulatorApprovalRequest>(), It.IsAny<CancellationToken>()))
                .Callback(() => clockReadings.Add(_clockGivenToHandlers!.GetUtcNow()))
                .Returns(Task.CompletedTask);
            _decisionHandlerMock
                .Setup(h => h.HandleAsync(It.IsAny<RegulatorRegistrationDecisionRequest>(), It.IsAny<CancellationToken>()))
                .Callback(() => clockReadings.Add(_clockGivenToHandlers!.GetUtcNow()))
                .Returns(Task.CompletedTask);

            var outcome = await _sut.ReplayAsync(_runId, Plan(), CancellationToken.None);

            using (new AssertionScope())
            {
                outcome.Status.Should().Be(SubmissionMigrationStatus.Succeeded);
                outcome.FilesMigrated.Should().Be(1);
                outcome.EventsReplayed.Should().Be(2);
                outcome.EventsSkipped.Should().Be(1);
                outcome.Warnings.Should().ContainSingle("plan warnings are carried through and pairing matched");
                clockReadings.Select(c => c.UtcDateTime).Should().Equal(DeclaredAt, ApprovalAt, DecisionAt);
                _repositoryMock.Verify(r => r.AddMigratedEntityAsync(
                    It.Is<MigratedEntity>(m => m.RunId == _runId && m.SubmissionId == _submissionId && m.FileId == "F1"
                                               && m.RegistrationSubmissionDataId == _dataId && m.CreatedAt == Now),
                    It.IsAny<CancellationToken>()), Times.Once);
                _transactionMock.Verify(t => t.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
            }
        }

        [TestMethod]
        public async Task ReplayAsync_ScopesLogsByReplayStep()
        {
            await _sut.ReplayAsync(_runId, Plan(), CancellationToken.None);

            using (new AssertionScope())
            {
                _logScopes.Select(s => s["MigrationReplayStep"]).Should().Equal(nameof(SubmittedStep), nameof(ApplicationSubmittedStep), nameof(DecisionStep));
                _logScopes.Select(s => s["MigrationReplayTs"]).Should().Equal(DeclaredAt, ApprovalAt, DecisionAt);
                _logScopes.Should().OnlyContain(s => Equals(s["MigrationFileId"], "F1"));
            }
        }

        [TestMethod]
        public async Task ReplayAsync_RecordedEventsDifferFromPlan_AddsPairingMismatchWarning()
        {
            SetupRecordedEvents(1);

            var outcome = await _sut.ReplayAsync(_runId, Plan(), CancellationToken.None);

            outcome.Status.Should().Be(SubmissionMigrationStatus.Succeeded);
            outcome.Warnings.Should().Contain(w => w.StartsWith("PairingMismatch: file F1 expected 2 events, recorded 1"));
        }

        [TestMethod]
        public async Task ReplayAsync_BlobMissing_FailsWithBlobNotFoundAndDoesNotCommit()
        {
            _dataHandlerMock
                .Setup(h => h.HandleAsync(It.IsAny<CreateRegistrationSubmissionDataRequest>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new RequestFailedException(404, "The specified blob does not exist."));

            var outcome = await _sut.ReplayAsync(_runId, Plan(), CancellationToken.None);

            AssertFailed(outcome, MigrationReasons.BlobNotFound);
        }

        [TestMethod]
        public async Task ReplayAsync_CsvWithoutProducerRow_FailsWithCsvInvalid()
        {
            _dataHandlerMock
                .Setup(h => h.HandleAsync(It.IsAny<CreateRegistrationSubmissionDataRequest>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InvalidOperationException("No producer row found for OrganisationId '100001'."));

            var outcome = await _sut.ReplayAsync(_runId, Plan(), CancellationToken.None);

            AssertFailed(outcome, MigrationReasons.CsvInvalid);
        }

        [TestMethod]
        public async Task ReplayAsync_HandlerError_FailsWithHandlerError()
        {
            _decisionHandlerMock
                .Setup(h => h.HandleAsync(It.IsAny<RegulatorRegistrationDecisionRequest>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InvalidOperationException("boom"));

            var outcome = await _sut.ReplayAsync(_runId, Plan(), CancellationToken.None);

            AssertFailed(outcome, MigrationReasons.HandlerError);
        }

        [TestMethod]
        public async Task ReplayAsync_Cancelled_Propagates()
        {
            _dataHandlerMock
                .Setup(h => h.HandleAsync(It.IsAny<CreateRegistrationSubmissionDataRequest>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new OperationCanceledException());

            Func<Task> act = () => _sut.ReplayAsync(_runId, Plan(), CancellationToken.None);

            await act.Should().ThrowAsync<OperationCanceledException>();
        }

        [TestMethod]
        public async Task ReplayAsync_NullPlan_Throws()
        {
            Func<Task> act = () => _sut.ReplayAsync(_runId, null!, CancellationToken.None);

            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        private void AssertFailed(SubmissionOutcome outcome, string reason)
        {
            using (new AssertionScope())
            {
                outcome.Status.Should().Be(SubmissionMigrationStatus.Failed);
                outcome.Reason.Should().Be(reason);
                outcome.Error.Should().NotBeNullOrWhiteSpace();
                _transactionMock.Verify(t => t.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
                _transactionMock.Verify(t => t.DisposeAsync(), Times.Once);
            }
        }

        private void SetupRecordedEvents(int eventCount)
        {
            var record = new RegistrationSubmissionData { Id = _dataId, SubmissionId = _submissionId };
            for (var i = 0; i < eventCount; i++)
            {
                record.Events.Add(new RegistrationSubmissionDataEvent { EventName = $"E{i}" });
            }

            _dataRepositoryMock
                .Setup(r => r.GetAllForSubmissionAsync(_submissionId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<RegistrationSubmissionData> { record });
        }

        private SubmissionReplayPlan Plan() => new(
            _submissionId,
            new List<ReplayStep>
            {
                new SubmittedStep("F1", DeclaredAt, new CreateRegistrationSubmissionDataRequest { SubmissionId = _submissionId, RegistrationBlobName = "blob" }),
                new ApplicationSubmittedStep("F1", ApprovalAt, new RegistrationSubmittedForRegulatorApprovalRequest { SubmissionId = _submissionId, SubmissionDate = ApprovalAt }),
                new DecisionStep("F1", DecisionAt, new RegulatorRegistrationDecisionRequest { SubmissionId = _submissionId, EventName = RegistrationEventNames.AcceptedByRegulator, DecisionDate = DecisionAt }),
            },
            new List<string> { "a plan warning" },
            1);
    }
}
