using EPR.Payment.Service.Common.Data.DataModels;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.RegistrationSubmission;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationSubmission;
using EPR.Payment.Service.Services.RegistrationSubmission;
using EPR.Payment.Service.Services.RegistrationSubmission.Csv;
using EPR.Payment.Service.Services.RegistrationSubmission.Csv.Models;
using EPR.Payment.Service.Services.RegistrationSubmission.Storage;
using EPR.Payment.Service.UnitTests.Services.RegistrationSubmission.TestHelpers;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;

namespace EPR.Payment.Service.UnitTests.Services.RegistrationSubmission
{
    [TestClass]
    public class RegistrationSubmissionDataHandlerTests
    {
        private Mock<IRegistrationSubmissionDataRepository> _repositoryMock = null!;
        private Mock<IBlobReader> _blobReaderMock = null!;
        private Mock<ICsvStreamParser> _csvStreamParserMock = null!;
        private Mock<TimeProvider> _timeProviderMock = null!;
        private RegistrationSubmissionDataHandler _sut = null!;
        private CancellationToken _ct;
        private DateTimeOffset _now;

        [TestInitialize]
        public void Init()
        {
            _repositoryMock = new Mock<IRegistrationSubmissionDataRepository>();
            _blobReaderMock = new Mock<IBlobReader>();
            _csvStreamParserMock = new Mock<ICsvStreamParser>();
            _now = new DateTimeOffset(2026, 5, 29, 9, 0, 0, TimeSpan.Zero);
            _timeProviderMock = new Mock<TimeProvider>();
            _timeProviderMock.Setup(t => t.GetUtcNow()).Returns(_now);
            _sut = new RegistrationSubmissionDataHandler(_repositoryMock.Object, _blobReaderMock.Object, _csvStreamParserMock.Object, _timeProviderMock.Object);
            _ct = CancellationToken.None;
        }

        [TestMethod]
        public void Constructor_NullDependency_Throws()
        {
            using (new AssertionScope())
            {
                Action a1 = () => new RegistrationSubmissionDataHandler(null!, _blobReaderMock.Object, _csvStreamParserMock.Object, _timeProviderMock.Object);
                Action a2 = () => new RegistrationSubmissionDataHandler(_repositoryMock.Object, null!, _csvStreamParserMock.Object, _timeProviderMock.Object);
                Action a3 = () => new RegistrationSubmissionDataHandler(_repositoryMock.Object, _blobReaderMock.Object, null!, _timeProviderMock.Object);
                Action a4 = () => new RegistrationSubmissionDataHandler(_repositoryMock.Object, _blobReaderMock.Object, _csvStreamParserMock.Object, null!);
                a1.Should().Throw<ArgumentNullException>();
                a2.Should().Throw<ArgumentNullException>();
                a3.Should().Throw<ArgumentNullException>();
                a4.Should().Throw<ArgumentNullException>();
            }
        }

        [TestMethod]
        public async Task HandleAsync_NullRequest_Throws()
        {
            Func<Task> act = () => _sut.HandleAsync(null!, _ct);
            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [TestMethod]
        public async Task HandleAsync_ExistingSnapshot_ReturnsExistingIdAndSkipsProcessing()
        {
            var request = NewRequest();
            var existing = new RegistrationSubmissionData { Id = Guid.NewGuid(), SubmissionId = request.SubmissionId, FileId = request.FileId };
            _repositoryMock
                .Setup(r => r.GetBySubmissionAndFileAsync(request.SubmissionId, request.FileId, _ct))
                .ReturnsAsync(existing);

            var result = await _sut.HandleAsync(request, _ct);

            using (new AssertionScope())
            {
                result.Should().Be(existing.Id);
                _blobReaderMock.Verify(b => b.DownloadAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
                _csvStreamParserMock.Verify(c => c.ParseAsync(It.IsAny<Stream>(), It.IsAny<CsvHelper.Configuration.ClassMap<RegistrationCsvRow>>(), It.IsAny<CancellationToken>()), Times.Never);
                _repositoryMock.Verify(r => r.CreateAsync(It.IsAny<RegistrationSubmissionData>(), It.IsAny<CancellationToken>()), Times.Never);
            }
        }

        [TestMethod]
        public async Task HandleAsync_SingleProducerWithSubsidiaries_PersistsExpectedGraph()
        {
            var request = NewRequest();
            ArrangeNoExistingSnapshot(request);

            var rows = new[]
            {
                RegistrationCsvFixtureFactory.Producer("ORG-1", homeNationCode: "EN", organisationSize: "Large", packagingActivityOm: "Primary", joinerDate: "2026-04-01", closedLoopRegistration: "Yes"),
                RegistrationCsvFixtureFactory.Subsidiary("ORG-1", "SUB-1", packagingActivityOm: "Secondary", closedLoopRegistration: "yes"),
                RegistrationCsvFixtureFactory.Subsidiary("ORG-1", "SUB-2"),
            };
            ArrangeCsvRows(request.RegistrationBlobName, rows);

            RegistrationSubmissionData? captured = null;
            _repositoryMock
                .Setup(r => r.CreateAsync(It.IsAny<RegistrationSubmissionData>(), _ct))
                .Callback<RegistrationSubmissionData, CancellationToken>((e, _) => captured = e)
                .ReturnsAsync(Guid.NewGuid());

            await _sut.HandleAsync(request, _ct);

            using (new AssertionScope())
            {
                captured.Should().NotBeNull();
                captured!.SubmissionId.Should().Be(request.SubmissionId);
                captured.FileId.Should().Be(request.FileId);
                captured.ComplianceSchemeId.Should().Be(request.ComplianceSchemeId);
                captured.SubmissionPeriod.Should().Be(request.SubmissionPeriod);
                captured.SubmissionDate.Should().Be(request.SubmissionDate);
                captured.CreatedDate.Should().Be(_now);
                captured.Producers.Should().HaveCount(1);

                var producer = captured.Producers.Single();
                producer.OrganisationId.Should().Be("ORG-1");
                producer.OrganisationSize.Should().Be("Large");
                producer.NationId.Should().Be(1);
                producer.IsOnlineMarketplace.Should().BeTrue();
                producer.IsClosedLoopRecycling.Should().BeTrue();
                producer.IsNewJoiner.Should().BeTrue();
                producer.CreatedDate.Should().Be(_now);
                producer.Subsidiaries.Should().HaveCount(2);

                var sub1 = producer.Subsidiaries.Single(s => s.SubsidiaryId == "SUB-1");
                sub1.IsOnlineMarketplace.Should().BeTrue();
                sub1.IsClosedLoopRecycling.Should().BeTrue();
                sub1.IsNewJoiner.Should().BeFalse();
                sub1.CreatedDate.Should().Be(_now);

                var sub2 = producer.Subsidiaries.Single(s => s.SubsidiaryId == "SUB-2");
                sub2.IsOnlineMarketplace.Should().BeFalse();
                sub2.IsClosedLoopRecycling.Should().BeFalse();
                sub2.IsNewJoiner.Should().BeFalse();
            }
        }

        [TestMethod]
        public async Task HandleAsync_ComplianceSchemeFile_GroupsByOrganisation()
        {
            var request = NewRequest();
            ArrangeNoExistingSnapshot(request);

            var rows = new[]
            {
                RegistrationCsvFixtureFactory.Producer("ORG-A", homeNationCode: "NI", organisationSize: "Large"),
                RegistrationCsvFixtureFactory.Subsidiary("ORG-A", "A-1"),
                RegistrationCsvFixtureFactory.Producer("ORG-B", homeNationCode: "SC", organisationSize: "Small"),
                RegistrationCsvFixtureFactory.Subsidiary("ORG-B", "B-1"),
                RegistrationCsvFixtureFactory.Subsidiary("ORG-B", "B-2"),
                RegistrationCsvFixtureFactory.Producer("ORG-C", homeNationCode: "WS", organisationSize: "Large"),
            };
            ArrangeCsvRows(request.RegistrationBlobName, rows);

            RegistrationSubmissionData? captured = null;
            _repositoryMock
                .Setup(r => r.CreateAsync(It.IsAny<RegistrationSubmissionData>(), _ct))
                .Callback<RegistrationSubmissionData, CancellationToken>((e, _) => captured = e)
                .ReturnsAsync(Guid.NewGuid());

            await _sut.HandleAsync(request, _ct);

            using (new AssertionScope())
            {
                captured!.Producers.Should().HaveCount(3);
                captured.Producers.Single(p => p.OrganisationId == "ORG-A").Subsidiaries.Should().HaveCount(1);
                captured.Producers.Single(p => p.OrganisationId == "ORG-A").NationId.Should().Be(2);
                captured.Producers.Single(p => p.OrganisationId == "ORG-B").Subsidiaries.Should().HaveCount(2);
                captured.Producers.Single(p => p.OrganisationId == "ORG-B").NationId.Should().Be(3);
                captured.Producers.Single(p => p.OrganisationId == "ORG-C").Subsidiaries.Should().HaveCount(0);
                captured.Producers.Single(p => p.OrganisationId == "ORG-C").NationId.Should().Be(4);
            }
        }

        [TestMethod]
        public async Task HandleAsync_SubsidiaryWithoutProducer_Throws()
        {
            var request = NewRequest();
            ArrangeNoExistingSnapshot(request);

            var rows = new[]
            {
                RegistrationCsvFixtureFactory.Subsidiary("ORG-ORPHAN", "SUB-1"),
            };
            ArrangeCsvRows(request.RegistrationBlobName, rows);

            Func<Task> act = () => _sut.HandleAsync(request, _ct);

            await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("*ORG-ORPHAN*");
        }

        [DataTestMethod]
        [DataRow("L", "Large")]
        [DataRow("l", "Large")]
        [DataRow("Large", "Large")]
        [DataRow("LARGE", "Large")]
        [DataRow("S", "Small")]
        [DataRow("s", "Small")]
        [DataRow("Small", "Small")]
        [DataRow("SMALL", "Small")]
        [DataRow("Unknown", "Unknown")]
        [DataRow("", "")]
        public async Task HandleAsync_OrganisationSizeMapping_NormalisesCsvCode(string value, string expected)
        {
            var request = NewRequest();
            ArrangeNoExistingSnapshot(request);
            ArrangeCsvRows(request.RegistrationBlobName, new[] { RegistrationCsvFixtureFactory.Producer("ORG-1", organisationSize: value) });

            RegistrationSubmissionData? captured = null;
            _repositoryMock
                .Setup(r => r.CreateAsync(It.IsAny<RegistrationSubmissionData>(), _ct))
                .Callback<RegistrationSubmissionData, CancellationToken>((e, _) => captured = e)
                .ReturnsAsync(Guid.NewGuid());

            await _sut.HandleAsync(request, _ct);

            captured!.Producers.Single().OrganisationSize.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow("EN", 1)]
        [DataRow("en", 1)]
        [DataRow("NI", 2)]
        [DataRow("SC", 3)]
        [DataRow("WS", 4)]
        [DataRow("WA", 4)]
        [DataRow("XX", 0)]
        [DataRow("", 0)]
        public async Task HandleAsync_NationIdMapping(string code, int expected)
        {
            var request = NewRequest();
            ArrangeNoExistingSnapshot(request);
            ArrangeCsvRows(request.RegistrationBlobName, new[] { RegistrationCsvFixtureFactory.Producer("ORG-1", homeNationCode: code) });

            RegistrationSubmissionData? captured = null;
            _repositoryMock
                .Setup(r => r.CreateAsync(It.IsAny<RegistrationSubmissionData>(), _ct))
                .Callback<RegistrationSubmissionData, CancellationToken>((e, _) => captured = e)
                .ReturnsAsync(Guid.NewGuid());

            await _sut.HandleAsync(request, _ct);

            captured!.Producers.Single().NationId.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow("Primary", true)]
        [DataRow("primary", true)]
        [DataRow("Secondary", true)]
        [DataRow("SECONDARY", true)]
        [DataRow("None", false)]
        [DataRow("", false)]
        public async Task HandleAsync_OnlineMarketplaceMapping(string value, bool expected)
        {
            var request = NewRequest();
            ArrangeNoExistingSnapshot(request);
            ArrangeCsvRows(request.RegistrationBlobName, new[] { RegistrationCsvFixtureFactory.Producer("ORG-1", packagingActivityOm: value) });

            RegistrationSubmissionData? captured = null;
            _repositoryMock
                .Setup(r => r.CreateAsync(It.IsAny<RegistrationSubmissionData>(), _ct))
                .Callback<RegistrationSubmissionData, CancellationToken>((e, _) => captured = e)
                .ReturnsAsync(Guid.NewGuid());

            await _sut.HandleAsync(request, _ct);

            captured!.Producers.Single().IsOnlineMarketplace.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow("YES", true)]
        [DataRow("yes", true)]
        [DataRow("Yes", true)]
        [DataRow("No", false)]
        [DataRow("", false)]
        public async Task HandleAsync_ClosedLoopRecyclingMapping(string value, bool expected)
        {
            var request = NewRequest();
            ArrangeNoExistingSnapshot(request);
            ArrangeCsvRows(request.RegistrationBlobName, new[] { RegistrationCsvFixtureFactory.Producer("ORG-1", closedLoopRegistration: value) });

            RegistrationSubmissionData? captured = null;
            _repositoryMock
                .Setup(r => r.CreateAsync(It.IsAny<RegistrationSubmissionData>(), _ct))
                .Callback<RegistrationSubmissionData, CancellationToken>((e, _) => captured = e)
                .ReturnsAsync(Guid.NewGuid());

            await _sut.HandleAsync(request, _ct);

            captured!.Producers.Single().IsClosedLoopRecycling.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow("2026-04-01", true)]
        [DataRow("anything-non-empty", true)]
        [DataRow("", false)]
        [DataRow(" ", false)]
        public async Task HandleAsync_NewJoinerMapping(string value, bool expected)
        {
            var request = NewRequest();
            ArrangeNoExistingSnapshot(request);
            ArrangeCsvRows(request.RegistrationBlobName, new[] { RegistrationCsvFixtureFactory.Producer("ORG-1", joinerDate: value) });

            RegistrationSubmissionData? captured = null;
            _repositoryMock
                .Setup(r => r.CreateAsync(It.IsAny<RegistrationSubmissionData>(), _ct))
                .Callback<RegistrationSubmissionData, CancellationToken>((e, _) => captured = e)
                .ReturnsAsync(Guid.NewGuid());

            await _sut.HandleAsync(request, _ct);

            captured!.Producers.Single().IsNewJoiner.Should().Be(expected);
        }

        [TestMethod]
        public async Task HandleAsync_DownloadsBlobByRegistrationBlobName()
        {
            var request = NewRequest();
            ArrangeNoExistingSnapshot(request);
            ArrangeCsvRows(request.RegistrationBlobName, new[] { RegistrationCsvFixtureFactory.Producer("ORG-1") });

            _repositoryMock.Setup(r => r.CreateAsync(It.IsAny<RegistrationSubmissionData>(), _ct)).ReturnsAsync(Guid.NewGuid());

            await _sut.HandleAsync(request, _ct);

            _blobReaderMock.Verify(b => b.DownloadAsync(request.RegistrationBlobName, _ct), Times.Once);
        }

        private CreateRegistrationSubmissionDataRequest NewRequest() => new()
        {
            SubmissionId = Guid.NewGuid(),
            FileId = Guid.NewGuid(),
            RegistrationBlobName = $"av-blob-{Guid.NewGuid()}",
            ComplianceSchemeId = Guid.NewGuid(),
            SubmissionPeriod = "Jan to Jun 2026",
            SubmissionDate = new DateTime(2026, 5, 28, 12, 0, 0, DateTimeKind.Utc),
        };

        private void ArrangeNoExistingSnapshot(CreateRegistrationSubmissionDataRequest request)
        {
            _repositoryMock
                .Setup(r => r.GetBySubmissionAndFileAsync(request.SubmissionId, request.FileId, _ct))
                .ReturnsAsync((RegistrationSubmissionData?)null);
        }

        private void ArrangeCsvRows(string blobName, IEnumerable<RegistrationCsvRow> rows)
        {
            var stream = new MemoryStream();
            _blobReaderMock
                .Setup(b => b.DownloadAsync(blobName, _ct))
                .ReturnsAsync(stream);
            _csvStreamParserMock
                .Setup(c => c.ParseAsync(stream, It.IsAny<CsvHelper.Configuration.ClassMap<RegistrationCsvRow>>(), _ct))
                .Returns(RegistrationCsvFixtureFactory.ToAsyncEnumerable(rows));
        }
    }
}
