using EPR.Payment.Service.Common.Data.DataModels;
using EPR.Payment.Service.Common.Data.Interfaces;
using EPR.Payment.Service.Common.Data.Repositories.RegistrationSubmission;
using EPR.Payment.Service.Data.UnitTests.TestHelpers;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.EntityFrameworkCore;

namespace EPR.Payment.Service.Data.UnitTests.Repositories.RegistrationSubmission
{
    [TestClass]
    public class RegistrationSubmissionDataRepositoryTests
    {
        private Mock<IAppDbContext> _dataContextMock = null!;
        private Mock<ILogger<RegistrationSubmissionDataRepository>> _loggerMock = null!;
        private RegistrationSubmissionDataRepository _sut = null!;
        private CancellationToken _ct;

        [TestInitialize]
        public void Init()
        {
            _dataContextMock = new Mock<IAppDbContext>();
            _loggerMock = new Mock<ILogger<RegistrationSubmissionDataRepository>>();
            _sut = new RegistrationSubmissionDataRepository(_dataContextMock.Object, _loggerMock.Object);
            _ct = CancellationToken.None;
        }

        [TestMethod]
        public void Constructor_NullContext_Throws()
        {
            Action act = () => new RegistrationSubmissionDataRepository(null!, NullLogger<RegistrationSubmissionDataRepository>.Instance);
            act.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Constructor_NullLogger_Throws()
        {
            Action act = () => new RegistrationSubmissionDataRepository(_dataContextMock.Object, null!);
            act.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public async Task GetBySubmissionAndFileAsync_Match_ReturnsEntity()
        {
            var submissionId = Guid.NewGuid();
            var fileId = Guid.NewGuid();
            var entity = new RegistrationSubmissionData { Id = Guid.NewGuid(), SubmissionId = submissionId, FileId = fileId };

            _dataContextMock.Setup(c => c.RegistrationSubmissionData).ReturnsDbSet(new[] { entity });

            var result = await _sut.GetBySubmissionAndFileAsync(submissionId, fileId, _ct);

            result.Should().NotBeNull();
            result!.Id.Should().Be(entity.Id);
        }

        [TestMethod]
        public async Task GetBySubmissionAndFileAsync_NoMatch_ReturnsNull()
        {
            _dataContextMock.Setup(c => c.RegistrationSubmissionData).ReturnsDbSet(Array.Empty<RegistrationSubmissionData>());

            var result = await _sut.GetBySubmissionAndFileAsync(Guid.NewGuid(), Guid.NewGuid(), _ct);

            result.Should().BeNull();
        }

        [TestMethod]
        public async Task CreateAsync_NullEntity_Throws()
        {
            Func<Task> act = () => _sut.CreateAsync(null!, _ct);
            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [TestMethod]
        public async Task GetLatestWithProducersAndSubsidiariesAsync_NoMatch_ReturnsNull()
        {
            _dataContextMock.Setup(c => c.RegistrationSubmissionData).ReturnsDbSet(Array.Empty<RegistrationSubmissionData>());

            var result = await _sut.GetLatestWithProducersAndSubsidiariesAsync(Guid.NewGuid(), _ct);

            result.Should().BeNull();
        }

        [TestMethod]
        public async Task GetLatestWithProducersAndSubsidiariesAsync_MultipleRows_ReturnsMostRecent()
        {
            var submissionId = Guid.NewGuid();
            var older = new RegistrationSubmissionData
            {
                Id = Guid.NewGuid(),
                SubmissionId = submissionId,
                CreatedDate = new DateTimeOffset(2026, 5, 28, 0, 0, 0, TimeSpan.Zero),
                Producers = new List<RegistrationSubmissionProducer>(),
            };
            var newer = new RegistrationSubmissionData
            {
                Id = Guid.NewGuid(),
                SubmissionId = submissionId,
                CreatedDate = new DateTimeOffset(2026, 5, 29, 0, 0, 0, TimeSpan.Zero),
                Producers = new List<RegistrationSubmissionProducer>(),
            };
            _dataContextMock.Setup(c => c.RegistrationSubmissionData).ReturnsDbSet(new[] { older, newer });

            var result = await _sut.GetLatestWithProducersAndSubsidiariesAsync(submissionId, _ct);

            result.Should().NotBeNull();
            result!.Id.Should().Be(newer.Id);
        }

        [TestMethod]
        public async Task CreateAsync_AddsEntityAndSavesOnce()
        {
            var dbSetMock = new Mock<DbSet<RegistrationSubmissionData>>();
            _dataContextMock.Setup(c => c.RegistrationSubmissionData).Returns(dbSetMock.Object);
            _dataContextMock.Setup(c => c.SaveChangesAsync(_ct)).ReturnsAsync(1);

            var entity = new RegistrationSubmissionData
            {
                Id = Guid.NewGuid(),
                SubmissionId = Guid.NewGuid(),
                FileId = Guid.NewGuid(),
                SubmissionPeriod = "period",
                SubmissionDate = DateTime.UtcNow,
                CreatedDate = DateTimeOffset.UtcNow,
                Producers = new List<RegistrationSubmissionProducer>
                {
                    new()
                    {
                        OrganisationId = "ORG-1",
                        OrganisationSize = "Large",
                        Subsidiaries = new List<RegistrationSubmissionSubsidiary>
                        {
                            new() { SubsidiaryId = "S-1" },
                        },
                    },
                },
            };

            var resultId = await _sut.CreateAsync(entity, _ct);

            using (new AssertionScope())
            {
                resultId.Should().Be(entity.Id);
                dbSetMock.Verify(s => s.Add(entity), Times.Once);
                _dataContextMock.Verify(c => c.SaveChangesAsync(_ct), Times.Once);
            }
        }

        [DataTestMethod]
        [DataRow(2601)]
        [DataRow(2627)]
        public async Task CreateAsync_UniqueViolation_RecoversByRequery(int sqlNumber)
        {
            var submissionId = Guid.NewGuid();
            var fileId = Guid.NewGuid();
            var existing = new RegistrationSubmissionData { Id = Guid.NewGuid(), SubmissionId = submissionId, FileId = fileId };

            var dbSetMock = new Mock<DbSet<RegistrationSubmissionData>>();
            _dataContextMock.SetupSequence(c => c.RegistrationSubmissionData)
                .Returns(dbSetMock.Object)
                .ReturnsDbSet(new[] { existing });
            _dataContextMock.Setup(c => c.SaveChangesAsync(_ct))
                .ThrowsAsync(new DbUpdateException("unique violation", SqlExceptionFactory.Create(sqlNumber)));

            var entity = new RegistrationSubmissionData
            {
                Id = Guid.NewGuid(),
                SubmissionId = submissionId,
                FileId = fileId,
                Producers = new List<RegistrationSubmissionProducer>(),
            };

            var result = await _sut.CreateAsync(entity, _ct);

            using (new AssertionScope())
            {
                result.Should().Be(existing.Id);
                _loggerMock.Verify(
                    x => x.Log(
                        LogLevel.Information,
                        It.IsAny<EventId>(),
                        It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Concurrent write detected")),
                        It.IsAny<Exception?>(),
                        It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                    Times.Once);
            }
        }

        [TestMethod]
        public async Task CreateAsync_UniqueViolation_NoExistingRowFound_Rethrows()
        {
            var dbSetMock = new Mock<DbSet<RegistrationSubmissionData>>();
            _dataContextMock.SetupSequence(c => c.RegistrationSubmissionData)
                .Returns(dbSetMock.Object)
                .ReturnsDbSet(Array.Empty<RegistrationSubmissionData>());
            _dataContextMock.Setup(c => c.SaveChangesAsync(_ct))
                .ThrowsAsync(new DbUpdateException("unique violation", SqlExceptionFactory.Create(2601)));

            var entity = new RegistrationSubmissionData
            {
                Id = Guid.NewGuid(),
                SubmissionId = Guid.NewGuid(),
                FileId = Guid.NewGuid(),
                Producers = new List<RegistrationSubmissionProducer>(),
            };

            Func<Task> act = () => _sut.CreateAsync(entity, _ct);

            await act.Should().ThrowAsync<DbUpdateException>();
        }

        [TestMethod]
        public async Task CreateAsync_OtherDbUpdateException_Rethrows()
        {
            var dbSetMock = new Mock<DbSet<RegistrationSubmissionData>>();
            _dataContextMock.Setup(c => c.RegistrationSubmissionData).Returns(dbSetMock.Object);
            _dataContextMock.Setup(c => c.SaveChangesAsync(_ct))
                .ThrowsAsync(new DbUpdateException("not a unique violation", new InvalidOperationException("inner")));

            var entity = new RegistrationSubmissionData
            {
                Id = Guid.NewGuid(),
                SubmissionId = Guid.NewGuid(),
                FileId = Guid.NewGuid(),
                Producers = new List<RegistrationSubmissionProducer>(),
            };

            Func<Task> act = () => _sut.CreateAsync(entity, _ct);

            await act.Should().ThrowAsync<DbUpdateException>().WithMessage("not a unique violation");
        }
    }
}
