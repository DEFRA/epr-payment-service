using EPR.Payment.Service.Common.Data.DataModels.Lookups;
using EPR.Payment.Service.Common.Data.DataModels.Migration;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.Migration;
using EPR.Payment.Service.Options;
using EPR.Payment.Service.Services.Migration.RegistrationHistory;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.Extensions.Logging;
using Moq;

namespace EPR.Payment.Service.UnitTests.Services.Migration.RegistrationHistory
{
    [TestClass]
    public class RegistrationHistoryMigrationRunnerTests
    {
        private const string RunKey = "registration-history-test";
        private static readonly DateTimeOffset Now = new(2026, 10, 5, 12, 0, 0, TimeSpan.Zero);
        private static readonly DateTime DeclaredAt = new(2026, 3, 1, 9, 0, 0, DateTimeKind.Utc);

        private readonly Guid _validSubmissionId = Guid.NewGuid();
        private readonly Guid _unmappedSubmissionId = Guid.NewGuid();

        private RegistrationHistoryMigrationOptions _options = null!;
        private CommonDataApiOptions _apiOptions = null!;
        private Mock<ICommonDataApiClient> _clientMock = null!;
        private Mock<IRegistrationHistoryMigrationRepository> _repositoryMock = null!;
        private Mock<IRegistrationHistorySubmissionReplayer> _replayerMock = null!;
        private Mock<ILogger<RegistrationHistoryMigrationRunner>> _loggerMock = null!;
        private List<Dictionary<string, object>> _logScopes = null!;
        private MigrationRun? _existingRun;
        private MigrationRun _createdRun = null!;
        private List<RegistrationHistoryStaging> _stagedRows = null!;
        private List<int> _batchSizes = null!;
        private List<RegistrationSubmissionMigration> _updatedSubmissions = null!;
        private MigrationStatusCounts _finalCounts = null!;
        private Dictionary<string, ExcludedSubmission> _excluded = null!;

        [TestInitialize]
        public void Init()
        {
            _options = new RegistrationHistoryMigrationOptions { Enabled = true, RunKey = RunKey, StagingBatchSize = 2, ProgressLogInterval = 1 };
            _apiOptions = new CommonDataApiOptions { BaseUrl = "https://common-data" };
            _clientMock = new Mock<ICommonDataApiClient>();
            _repositoryMock = new Mock<IRegistrationHistoryMigrationRepository>();
            _replayerMock = new Mock<IRegistrationHistorySubmissionReplayer>();
            _loggerMock = new Mock<ILogger<RegistrationHistoryMigrationRunner>>();
            _logScopes = new List<Dictionary<string, object>>();
            _loggerMock.Setup(l => l.BeginScope(It.IsAny<Dictionary<string, object>>()))
                .Callback<Dictionary<string, object>>(_logScopes.Add);
            _existingRun = null;
            _createdRun = new MigrationRun { Id = Guid.NewGuid(), RunKey = RunKey, Status = MigrationRunStatus.Running };
            _stagedRows = new List<RegistrationHistoryStaging>();
            _batchSizes = new List<int>();
            _updatedSubmissions = new List<RegistrationSubmissionMigration>();
            _finalCounts = new MigrationStatusCounts(2, 0, 1, 0, 1, 0);
            _excluded = new Dictionary<string, ExcludedSubmission>(StringComparer.OrdinalIgnoreCase);

            _clientMock.Setup(c => c.GetRegistrationHistoryAsync(It.IsAny<CancellationToken>())).Returns(Records().ToAsyncEnumerable());

            _repositoryMock.Setup(r => r.TryAcquireRunLockAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true);
            _repositoryMock.Setup(r => r.GetRunAsync(RunKey, It.IsAny<CancellationToken>())).ReturnsAsync(() => _existingRun);
            _repositoryMock.Setup(r => r.CreateRunAsync(RunKey, It.IsAny<string>(), Now, It.IsAny<CancellationToken>())).ReturnsAsync(() => _createdRun);
            _repositoryMock
                .Setup(r => r.AddStagingBatchAsync(It.IsAny<IReadOnlyCollection<RegistrationHistoryStaging>>(), It.IsAny<CancellationToken>()))
                .Callback<IReadOnlyCollection<RegistrationHistoryStaging>, CancellationToken>((rows, _) =>
                {
                    _batchSizes.Add(rows.Count);
                    _stagedRows.AddRange(rows);
                })
                .Returns(Task.CompletedTask);
            _repositoryMock.Setup(r => r.CreatePendingSubmissionsAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(2);
            _repositoryMock.Setup(r => r.GetSubmissionPeriodsAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<SubmissionPeriod> { new() { Id = 5, WindowType = "DirectLargeProducer", RegistrationYear = 2026 } });
            _repositoryMock.Setup(r => r.GetExcludedSubmissionsAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => _excluded);
            _repositoryMock.Setup(r => r.GetPendingSubmissionsAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => new List<RegistrationSubmissionMigration>
                {
                    new() { Id = Guid.NewGuid(), SubmissionId = _validSubmissionId.ToString(), Status = SubmissionMigrationStatus.Pending },
                    new() { Id = Guid.NewGuid(), SubmissionId = _unmappedSubmissionId.ToString(), Status = SubmissionMigrationStatus.Pending },
                });
            _repositoryMock.Setup(r => r.GetStagingForSubmissionAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Guid _, string submissionId, CancellationToken _) =>
                    Records().Where(r => r.SubmissionId == submissionId).Select(r => new RegistrationHistoryStaging
                    {
                        SubmissionId = r.SubmissionId,
                        FileId = r.FileId,
                        BlobName = r.BlobName,
                        SubmissionPeriod = r.SubmissionPeriod,
                        RegistrationJourney = r.RegistrationJourney,
                        RegulatorNation = r.RegulatorNation,
                        EventType = r.EventType,
                        EventTypeOrder = r.EventTypeOrder,
                        ReplayTs = r.ReplayTs,
                        EventDate = r.EventDate,
                        ApplicationReferenceNumber = r.ApplicationReferenceNumber,
                    }).ToList());
            _repositoryMock
                .Setup(r => r.UpdateSubmissionAsync(It.IsAny<RegistrationSubmissionMigration>(), It.IsAny<CancellationToken>()))
                .Callback<RegistrationSubmissionMigration, CancellationToken>((s, _) => _updatedSubmissions.Add(s))
                .Returns(Task.CompletedTask);
            _repositoryMock.Setup(r => r.GetStatusCountsAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(() => _finalCounts);
            _repositoryMock.Setup(r => r.ClearDownAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ClearDownResult(1, new List<int>()));

            _replayerMock
                .Setup(r => r.ReplayAsync(It.IsAny<Guid>(), It.IsAny<SubmissionReplayPlan>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new SubmissionOutcome(SubmissionMigrationStatus.Succeeded, FilesMigrated: 1));
        }

        [TestMethod]
        public void Constructor_NullDependency_Throws()
        {
            var options = Microsoft.Extensions.Options.Options.Create(_options);
            var apiOptions = Microsoft.Extensions.Options.Options.Create(_apiOptions);
            var logger = Mock.Of<ILogger<RegistrationHistoryMigrationRunner>>();

            using (new AssertionScope())
            {
                ((Action)(() => new RegistrationHistoryMigrationRunner(null!, apiOptions, _clientMock.Object, _repositoryMock.Object, _replayerMock.Object, TimeProvider.System, logger))).Should().Throw<ArgumentNullException>();
                ((Action)(() => new RegistrationHistoryMigrationRunner(options, null!, _clientMock.Object, _repositoryMock.Object, _replayerMock.Object, TimeProvider.System, logger))).Should().Throw<ArgumentNullException>();
                ((Action)(() => new RegistrationHistoryMigrationRunner(options, apiOptions, null!, _repositoryMock.Object, _replayerMock.Object, TimeProvider.System, logger))).Should().Throw<ArgumentNullException>();
                ((Action)(() => new RegistrationHistoryMigrationRunner(options, apiOptions, _clientMock.Object, null!, _replayerMock.Object, TimeProvider.System, logger))).Should().Throw<ArgumentNullException>();
                ((Action)(() => new RegistrationHistoryMigrationRunner(options, apiOptions, _clientMock.Object, _repositoryMock.Object, null!, TimeProvider.System, logger))).Should().Throw<ArgumentNullException>();
                ((Action)(() => new RegistrationHistoryMigrationRunner(options, apiOptions, _clientMock.Object, _repositoryMock.Object, _replayerMock.Object, null!, logger))).Should().Throw<ArgumentNullException>();
                ((Action)(() => new RegistrationHistoryMigrationRunner(options, apiOptions, _clientMock.Object, _repositoryMock.Object, _replayerMock.Object, TimeProvider.System, null!))).Should().Throw<ArgumentNullException>();
            }
        }

        [TestMethod]
        public async Task RunAsync_Disabled_DoesNothing()
        {
            _options.Enabled = false;

            await CreateSut().RunAsync(CancellationToken.None);

            _repositoryMock.VerifyNoOtherCalls();
            _clientMock.VerifyNoOtherCalls();
        }

        [DataTestMethod]
        [DataRow("", "https://common-data")]
        [DataRow(RunKey, "")]
        public async Task RunAsync_MissingConfiguration_DoesNothing(string runKey, string baseUrl)
        {
            _options.RunKey = runKey;
            _apiOptions.BaseUrl = baseUrl;

            await CreateSut().RunAsync(CancellationToken.None);

            _repositoryMock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public async Task RunAsync_LockHeldByAnotherInstance_DoesNotStart()
        {
            _repositoryMock.Setup(r => r.TryAcquireRunLockAsync(It.IsAny<CancellationToken>())).ReturnsAsync(false);

            await CreateSut().RunAsync(CancellationToken.None);

            _repositoryMock.Verify(r => r.GetRunAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
            _repositoryMock.Verify(r => r.ReleaseRunLockAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [TestMethod]
        public async Task RunAsync_ReleasesRunLockWhenFinished()
        {
            await CreateSut().RunAsync(CancellationToken.None);

            _repositoryMock.Verify(r => r.ReleaseRunLockAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [TestMethod]
        public async Task RunAsync_RunLockReleaseFails_DoesNotThrow()
        {
            _repositoryMock.Setup(r => r.ReleaseRunLockAsync(It.IsAny<CancellationToken>())).ThrowsAsync(new InvalidOperationException("connection broken"));

            Func<Task> act = () => CreateSut().RunAsync(CancellationToken.None);

            await act.Should().NotThrowAsync();
            _createdRun.Status.Should().Be(MigrationRunStatus.Completed);
        }

        [DataTestMethod]
        [DataRow(MigrationRunStatus.Completed)]
        [DataRow(MigrationRunStatus.CompletedWithErrors)]
        [DataRow(MigrationRunStatus.ClearedDown)]
        [DataRow(MigrationRunStatus.Failed)]
        public async Task RunAsync_RunKeyAlreadyFinished_NeverRunsAgain(MigrationRunStatus status)
        {
            _existingRun = new MigrationRun { Id = Guid.NewGuid(), RunKey = RunKey, Status = status };

            await CreateSut().RunAsync(CancellationToken.None);

            using (new AssertionScope())
            {
                _repositoryMock.Verify(r => r.CreateRunAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTimeOffset>(), It.IsAny<CancellationToken>()), Times.Never);
                _clientMock.VerifyNoOtherCalls();
                _replayerMock.VerifyNoOtherCalls();
            }
        }

        [TestMethod]
        public async Task RunAsync_NewRun_StagesInBatchesReplaysPlannableSubmissionsAndCompletes()
        {
            await CreateSut().RunAsync(CancellationToken.None);

            using (new AssertionScope())
            {
                _batchSizes.Should().Equal(2, 1);
                _stagedRows.Should().OnlyContain(r => r.RunId == _createdRun.Id);
                _createdRun.StagedRowCount.Should().Be(3);
                _createdRun.SubmissionCount.Should().Be(2);
                _createdRun.StagingCompletedAt.Should().Be(Now);

                _replayerMock.Verify(r => r.ReplayAsync(_createdRun.Id, It.Is<SubmissionReplayPlan>(p => p.SubmissionId == _validSubmissionId), It.IsAny<CancellationToken>()), Times.Once);
                _replayerMock.VerifyNoOtherCalls();

                _updatedSubmissions.Should().HaveCount(2);
                _updatedSubmissions.Single(s => s.SubmissionId == _validSubmissionId.ToString()).Status.Should().Be(SubmissionMigrationStatus.Succeeded);
                var unmapped = _updatedSubmissions.Single(s => s.SubmissionId == _unmappedSubmissionId.ToString());
                unmapped.Status.Should().Be(SubmissionMigrationStatus.NotRequired);
                unmapped.Reason.Should().Be(MigrationReasons.PeriodNotMapped);
                unmapped.ProcessedAt.Should().Be(Now);

                _createdRun.Status.Should().Be(MigrationRunStatus.Completed);
                _createdRun.SucceededCount.Should().Be(1);
                _createdRun.NotRequiredCount.Should().Be(1);
                _createdRun.CompletedAt.Should().Be(Now);
                _createdRun.ClearDownApplied.Should().BeFalse();
                _repositoryMock.Verify(r => r.ClearDownAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
            }
        }

        [TestMethod]
        public async Task RunAsync_ExcludedSubmission_RecordsExcludedWithoutPlanningOrReplaying()
        {
            _excluded[_validSubmissionId.ToString().ToUpperInvariant()] = new ExcludedSubmission
            {
                SubmissionId = _validSubmissionId.ToString().ToUpperInvariant(),
                Reason = MigrationReasons.CsvInvalid,
                Note = "No parent row",
            };
            _finalCounts = new MigrationStatusCounts(2, 0, 0, 0, 1, 1);

            await CreateSut().RunAsync(CancellationToken.None);

            using (new AssertionScope())
            {
                _replayerMock.VerifyNoOtherCalls();
                _repositoryMock.Verify(r => r.GetStagingForSubmissionAsync(It.IsAny<Guid>(), _validSubmissionId.ToString(), It.IsAny<CancellationToken>()), Times.Never);

                var excluded = _updatedSubmissions.Single(s => s.SubmissionId == _validSubmissionId.ToString());
                excluded.Status.Should().Be(SubmissionMigrationStatus.Excluded);
                excluded.Reason.Should().Be(MigrationReasons.KnownIssue);
                excluded.Error.Should().Be("CsvInvalid: No parent row");
                excluded.ProcessedAt.Should().Be(Now);
                _updatedSubmissions.Single(s => s.SubmissionId == _unmappedSubmissionId.ToString()).Status.Should().Be(SubmissionMigrationStatus.NotRequired);
            }
        }

        [TestMethod]
        public async Task RunAsync_ExcludedSubmissionWithoutNote_RecordsReasonAsError()
        {
            _excluded[_validSubmissionId.ToString()] = new ExcludedSubmission { SubmissionId = _validSubmissionId.ToString(), Reason = MigrationReasons.ArnMissing };

            await CreateSut().RunAsync(CancellationToken.None);

            _updatedSubmissions.Single(s => s.SubmissionId == _validSubmissionId.ToString()).Error.Should().Be(MigrationReasons.ArnMissing);
        }

        [TestMethod]
        public async Task RunAsync_OnlyExclusions_CompletesWithoutClearDown()
        {
            _excluded[_validSubmissionId.ToString()] = new ExcludedSubmission { SubmissionId = _validSubmissionId.ToString(), Reason = MigrationReasons.CsvInvalid };
            _finalCounts = new MigrationStatusCounts(2, 0, 0, 0, 1, 1);

            await CreateSut().RunAsync(CancellationToken.None);

            using (new AssertionScope())
            {
                _createdRun.Status.Should().Be(MigrationRunStatus.Completed);
                _createdRun.ExcludedCount.Should().Be(1);
                _createdRun.FailedCount.Should().Be(0);
                _createdRun.ClearDownApplied.Should().BeFalse();
                _repositoryMock.Verify(r => r.ClearDownAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
            }
        }

        [TestMethod]
        public async Task RunAsync_SubmissionFailsAndClearDownEnabled_ClearsDownAndKeepsLogs()
        {
            ReplayerFails();

            await CreateSut().RunAsync(CancellationToken.None);

            using (new AssertionScope())
            {
                _repositoryMock.Verify(r => r.ClearDownAsync(_createdRun.Id, It.IsAny<CancellationToken>()), Times.Once);
                _repositoryMock.Verify(r => r.ResetStagingAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once, "only the initial reset; staging is kept after clear-down");
                _createdRun.Status.Should().Be(MigrationRunStatus.ClearedDown);
                _createdRun.ClearDownApplied.Should().BeTrue();
                _createdRun.FailedCount.Should().Be(1);
                var failed = _updatedSubmissions.Single(s => s.SubmissionId == _validSubmissionId.ToString());
                failed.Reason.Should().Be(MigrationReasons.BlobNotFound);
                failed.Warnings.Should().Be("w1" + Environment.NewLine + "w2");
            }
        }

        [TestMethod]
        public async Task RunAsync_SubmissionFailsAndClearDownDisabled_CompletesWithErrors()
        {
            _options.ClearDownMigratedDataOnError = false;
            ReplayerFails();

            await CreateSut().RunAsync(CancellationToken.None);

            using (new AssertionScope())
            {
                _repositoryMock.Verify(r => r.ClearDownAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
                _createdRun.Status.Should().Be(MigrationRunStatus.CompletedWithErrors);
                _createdRun.ClearDownApplied.Should().BeFalse();
            }
        }

        [DataTestMethod]
        [DataRow(true, MigrationRunStatus.ClearedDown)]
        [DataRow(false, MigrationRunStatus.Failed)]
        public async Task RunAsync_RunLevelError_RecordsErrorAndAppliesClearDownSetting(bool clearDown, MigrationRunStatus expectedStatus)
        {
            _options.ClearDownMigratedDataOnError = clearDown;
            _clientMock.Setup(c => c.GetRegistrationHistoryAsync(It.IsAny<CancellationToken>())).Throws(new HttpRequestException("unreachable"));

            await CreateSut().RunAsync(CancellationToken.None);

            using (new AssertionScope())
            {
                _createdRun.Status.Should().Be(expectedStatus);
                _createdRun.ErrorMessage.Should().Contain("unreachable");
                _createdRun.CompletedAt.Should().Be(Now);
                _repositoryMock.Verify(r => r.ClearDownAsync(_createdRun.Id, It.IsAny<CancellationToken>()), clearDown ? Times.Once() : Times.Never());
                _replayerMock.VerifyNoOtherCalls();
            }
        }

        [TestMethod]
        public async Task RunAsync_ResumingRunWithCompletedStaging_SkipsStagingAndProcessesPending()
        {
            _existingRun = new MigrationRun { Id = Guid.NewGuid(), RunKey = RunKey, Status = MigrationRunStatus.Running, StagingCompletedAt = Now.AddHours(-1) };

            await CreateSut().RunAsync(CancellationToken.None);

            using (new AssertionScope())
            {
                _repositoryMock.Verify(r => r.CreateRunAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTimeOffset>(), It.IsAny<CancellationToken>()), Times.Never);
                _repositoryMock.Verify(r => r.ResetStagingAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
                _clientMock.VerifyNoOtherCalls();
                _replayerMock.Verify(r => r.ReplayAsync(_existingRun.Id, It.IsAny<SubmissionReplayPlan>(), It.IsAny<CancellationToken>()), Times.Once);
                _existingRun.Status.Should().Be(MigrationRunStatus.Completed);
            }
        }

        [TestMethod]
        public async Task RunAsync_ResumingRunWithIncompleteStaging_RestagesFromScratch()
        {
            _existingRun = new MigrationRun { Id = Guid.NewGuid(), RunKey = RunKey, Status = MigrationRunStatus.Running };

            await CreateSut().RunAsync(CancellationToken.None);

            using (new AssertionScope())
            {
                _repositoryMock.Verify(r => r.ResetStagingAsync(_existingRun.Id, It.IsAny<CancellationToken>()), Times.Once);
                _stagedRows.Should().HaveCount(3);
            }
        }

        [TestMethod]
        public async Task RunAsync_Cancelled_LeavesRunRunningForResume()
        {
            using var cts = new CancellationTokenSource();
            _replayerMock
                .Setup(r => r.ReplayAsync(It.IsAny<Guid>(), It.IsAny<SubmissionReplayPlan>(), It.IsAny<CancellationToken>()))
                .Callback(() => cts.Cancel())
                .ThrowsAsync(new OperationCanceledException(cts.Token));

            Func<Task> act = () => CreateSut().RunAsync(cts.Token);

            await act.Should().ThrowAsync<OperationCanceledException>();
            _createdRun.Status.Should().Be(MigrationRunStatus.Running);
            _repositoryMock.Verify(r => r.ClearDownAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
            _repositoryMock.Verify(r => r.ReleaseRunLockAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [TestMethod]
        public async Task RunAsync_NewRun_ScopesLogsByRunPhaseAndSubmission()
        {
            await CreateSut().RunAsync(CancellationToken.None);

            using (new AssertionScope())
            {
                _logScopes.Should().ContainSingle(s => s.ContainsKey("MigrationRunKey") && Equals(s["MigrationRunKey"], RunKey));
                _logScopes.Should().ContainSingle(s => s.ContainsKey("MigrationRunId")
                    && Equals(s["MigrationRunId"], _createdRun.Id)
                    && Equals(s["MigrationRunResumed"], false));
                _logScopes.Where(s => s.ContainsKey("MigrationPhase")).Select(s => s["MigrationPhase"])
                    .Should().Equal("Staging", "Replay", "Finish");
                _logScopes.Where(s => s.ContainsKey("SubmissionId")).Select(s => s["SubmissionId"])
                    .Should().Equal(_validSubmissionId.ToString(), _unmappedSubmissionId.ToString());
            }
        }

        private void ReplayerFails()
        {
            _finalCounts = new MigrationStatusCounts(2, 0, 0, 1, 1, 0);
            _replayerMock
                .Setup(r => r.ReplayAsync(It.IsAny<Guid>(), It.IsAny<SubmissionReplayPlan>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new SubmissionOutcome(SubmissionMigrationStatus.Failed, MigrationReasons.BlobNotFound, "missing", new List<string> { "w1", "w2" }));
        }

        private RegistrationHistoryMigrationRunner CreateSut()
        {
            var timeProviderMock = new Mock<TimeProvider>();
            timeProviderMock.Setup(t => t.GetUtcNow()).Returns(Now);

            return new RegistrationHistoryMigrationRunner(
                Microsoft.Extensions.Options.Options.Create(_options),
                Microsoft.Extensions.Options.Options.Create(_apiOptions),
                _clientMock.Object,
                _repositoryMock.Object,
                _replayerMock.Object,
                timeProviderMock.Object,
                _loggerMock.Object);
        }

        private IEnumerable<RegistrationHistoryRecord> Records()
        {
            yield return new RegistrationHistoryRecord
            {
                SubmissionId = _validSubmissionId.ToString(),
                FileId = "F1",
                BlobName = "blob-1",
                SubmissionPeriod = "January to December 2026",
                RegistrationJourney = "DirectLargeProducer",
                RegulatorNation = "GB-ENG",
                EventType = RegistrationHistoryEventTypes.Submitted,
                EventTypeOrder = 1,
                ReplayTs = DeclaredAt,
                EventDate = DeclaredAt,
                ApplicationReferenceNumber = "PEPR001",
            };
            yield return new RegistrationHistoryRecord
            {
                SubmissionId = _validSubmissionId.ToString(),
                FileId = "F1",
                EventType = RegistrationHistoryEventTypes.RegistrationApplicationSubmitted,
                EventTypeOrder = 2,
                ReplayTs = DeclaredAt.AddHours(1),
                EventDate = DeclaredAt.AddHours(1),
                ApplicationReferenceNumber = "PEPR001",
            };
            yield return new RegistrationHistoryRecord
            {
                SubmissionId = _unmappedSubmissionId.ToString(),
                FileId = "F9",
                BlobName = "blob-9",
                SubmissionPeriod = "January to December 2024",
                RegulatorNation = "GB-ENG",
                EventType = RegistrationHistoryEventTypes.Submitted,
                EventTypeOrder = 1,
                ReplayTs = DeclaredAt,
                EventDate = DeclaredAt,
                ApplicationReferenceNumber = "PEPR009",
            };
        }
    }
}
