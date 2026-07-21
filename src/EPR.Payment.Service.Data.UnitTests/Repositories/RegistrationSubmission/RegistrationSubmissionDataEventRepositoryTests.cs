using EPR.Payment.Service.Common.Data.DataModels;
using EPR.Payment.Service.Common.Data.Interfaces;
using EPR.Payment.Service.Common.Data.Repositories.RegistrationSubmission;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.EntityFrameworkCore;

namespace EPR.Payment.Service.Data.UnitTests.Repositories.RegistrationSubmission
{
    [TestClass]
    public class RegistrationSubmissionDataEventRepositoryTests
    {
        private Mock<IAppDbContext> _dataContextMock = null!;
        private Mock<ILogger<RegistrationSubmissionDataEventRepository>> _loggerMock = null!;
        private RegistrationSubmissionDataEventRepository _sut = null!;
        private CancellationToken _ct;

        [TestInitialize]
        public void Init()
        {
            _dataContextMock = new Mock<IAppDbContext>();
            _loggerMock = new Mock<ILogger<RegistrationSubmissionDataEventRepository>>();
            _sut = new RegistrationSubmissionDataEventRepository(_dataContextMock.Object, _loggerMock.Object);
            _ct = CancellationToken.None;
        }

        [TestMethod]
        public void Constructor_NullContext_Throws()
        {
            Action act = () => new RegistrationSubmissionDataEventRepository(null!, _loggerMock.Object);
            act.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Constructor_NullLogger_Throws()
        {
            Action act = () => new RegistrationSubmissionDataEventRepository(_dataContextMock.Object, null!);
            act.Should().Throw<ArgumentNullException>();
        }

        [DataTestMethod]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow(null)]
        public async Task AddEventForLatestSubmissionAsync_NullOrWhitespaceEventName_Throws(string? eventName)
        {
            Func<Task> act = () => _sut.AddEventForLatestSubmissionAsync(Guid.NewGuid(), eventName!, DateTime.UtcNow, _ct);
            await act.Should().ThrowAsync<ArgumentException>();
        }

        [TestMethod]
        public async Task AddEventForLatestSubmissionAsync_NoMatchingSubmissionId_ReturnsNull()
        {
            _dataContextMock.Setup(c => c.RegistrationSubmissionData).ReturnsDbSet(Array.Empty<RegistrationSubmissionData>());

            var result = await _sut.AddEventForLatestSubmissionAsync(Guid.NewGuid(), "SubmittedForRegulatorApproval", DateTime.UtcNow, _ct);

            using (new AssertionScope())
            {
                result.Should().BeNull();
                _dataContextMock.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
            }
        }

        [TestMethod]
        public async Task AddEventForLatestSubmissionAsync_MultipleRows_PicksLatestByCreatedDate()
        {
            var submissionId = Guid.NewGuid();
            var older = new RegistrationSubmissionData
            {
                Id = Guid.NewGuid(),
                SubmissionId = submissionId,
                CreatedDate = new DateTimeOffset(2026, 5, 28, 0, 0, 0, TimeSpan.Zero),
            };
            var newer = new RegistrationSubmissionData
            {
                Id = Guid.NewGuid(),
                SubmissionId = submissionId,
                CreatedDate = new DateTimeOffset(2026, 5, 29, 0, 0, 0, TimeSpan.Zero),
            };
            _dataContextMock.Setup(c => c.RegistrationSubmissionData).ReturnsDbSet(new[] { older, newer });

            var eventsDbSetMock = new Mock<DbSet<RegistrationSubmissionDataEvent>>();
            _dataContextMock.Setup(c => c.RegistrationSubmissionDataEvents).Returns(eventsDbSetMock.Object);
            _dataContextMock.Setup(c => c.SaveChangesAsync(_ct)).ReturnsAsync(1);

            var result = await _sut.AddEventForLatestSubmissionAsync(submissionId, "SubmittedForRegulatorApproval", DateTime.UtcNow, _ct);

            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                eventsDbSetMock.Verify(
                    s => s.Add(It.Is<RegistrationSubmissionDataEvent>(e => e.RegistrationSubmissionDataId == newer.Id)),
                    Times.Once);
                _dataContextMock.Verify(c => c.SaveChangesAsync(_ct), Times.Once);
            }
        }

        [TestMethod]
        public async Task AddEventForLatestSubmissionAsync_MatchFound_InsertsAndReturnsNewId()
        {
            var submissionId = Guid.NewGuid();
            var latest = new RegistrationSubmissionData
            {
                Id = Guid.NewGuid(),
                SubmissionId = submissionId,
                CreatedDate = DateTimeOffset.UtcNow,
            };
            _dataContextMock.Setup(c => c.RegistrationSubmissionData).ReturnsDbSet(new[] { latest });

            var captured = new List<RegistrationSubmissionDataEvent>();
            var eventsDbSetMock = new Mock<DbSet<RegistrationSubmissionDataEvent>>();
            eventsDbSetMock
                .Setup(s => s.Add(It.IsAny<RegistrationSubmissionDataEvent>()))
                .Callback<RegistrationSubmissionDataEvent>(e =>
                {
                    e.Id = e.Id == Guid.Empty ? Guid.NewGuid() : e.Id;
                    captured.Add(e);
                });
            _dataContextMock.Setup(c => c.RegistrationSubmissionDataEvents).Returns(eventsDbSetMock.Object);
            _dataContextMock.Setup(c => c.SaveChangesAsync(_ct)).ReturnsAsync(1);

            var eventDate = new DateTime(2026, 7, 20, 10, 0, 0, DateTimeKind.Utc);
            var result = await _sut.AddEventForLatestSubmissionAsync(submissionId, "SubmittedForRegulatorApproval", eventDate, _ct);

            using (new AssertionScope())
            {
                captured.Should().HaveCount(1);
                var stored = captured[0];
                stored.RegistrationSubmissionDataId.Should().Be(latest.Id);
                stored.EventName.Should().Be("SubmittedForRegulatorApproval");
                stored.EventDate.Should().Be(eventDate);
                stored.CreatedDate.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(5));
                result.Should().Be(stored.Id);
            }
        }

        [TestMethod]
        public async Task AddEventForLatestSubmissionAsync_UniqueConstraintViolation_SwallowsAndReturnsLatestId()
        {
            var submissionId = Guid.NewGuid();
            var latest = new RegistrationSubmissionData
            {
                Id = Guid.NewGuid(),
                SubmissionId = submissionId,
                CreatedDate = DateTimeOffset.UtcNow,
            };
            _dataContextMock.Setup(c => c.RegistrationSubmissionData).ReturnsDbSet(new[] { latest });

            var eventsDbSetMock = new Mock<DbSet<RegistrationSubmissionDataEvent>>();
            _dataContextMock.Setup(c => c.RegistrationSubmissionDataEvents).Returns(eventsDbSetMock.Object);
            _dataContextMock
                .Setup(c => c.SaveChangesAsync(_ct))
                .ThrowsAsync(new DbUpdateException("insert failed", new Exception("Violation of UNIQUE KEY constraint 'IX_RegistrationSubmissionDataEvents_SubmissionData_Event_Date_Unique'.")));

            var result = await _sut.AddEventForLatestSubmissionAsync(submissionId, "SubmittedForRegulatorApproval", DateTime.UtcNow, _ct);

            result.Should().Be(latest.Id);
        }

        [TestMethod]
        public async Task AddEventForLatestSubmissionAsync_NonUniqueDbUpdateException_Rethrows()
        {
            var submissionId = Guid.NewGuid();
            var latest = new RegistrationSubmissionData
            {
                Id = Guid.NewGuid(),
                SubmissionId = submissionId,
                CreatedDate = DateTimeOffset.UtcNow,
            };
            _dataContextMock.Setup(c => c.RegistrationSubmissionData).ReturnsDbSet(new[] { latest });

            var eventsDbSetMock = new Mock<DbSet<RegistrationSubmissionDataEvent>>();
            _dataContextMock.Setup(c => c.RegistrationSubmissionDataEvents).Returns(eventsDbSetMock.Object);
            _dataContextMock
                .Setup(c => c.SaveChangesAsync(_ct))
                .ThrowsAsync(new DbUpdateException("save failed", new InvalidOperationException("network glitch")));

            Func<Task> act = () => _sut.AddEventForLatestSubmissionAsync(submissionId, "SubmittedForRegulatorApproval", DateTime.UtcNow, _ct);

            await act.Should().ThrowAsync<DbUpdateException>().WithMessage("save failed");
        }
    }
}
