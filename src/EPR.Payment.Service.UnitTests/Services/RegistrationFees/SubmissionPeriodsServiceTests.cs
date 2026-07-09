using EPR.Payment.Service.Common.Data.DataModels.Lookups;
using EPR.Payment.Service.Common.Data.Interfaces;
using EPR.Payment.Service.Services.RegistrationFees;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using Moq.EntityFrameworkCore;

namespace EPR.Payment.Service.UnitTests.Services.RegistrationFees
{
    [TestClass]
    public class SubmissionPeriodsServiceTests
    {
        private Mock<IAppDbContext> _dbContextMock = null!;
        private SubmissionPeriodsService _sut = null!;
        private CancellationToken _ct;

        [TestInitialize]
        public void Init()
        {
            _dbContextMock = new Mock<IAppDbContext>();
            _sut = new SubmissionPeriodsService(_dbContextMock.Object);
            _ct = CancellationToken.None;
        }

        [TestMethod]
        public void Constructor_NullDbContext_Throws()
        {
            Action act = () => new SubmissionPeriodsService(null!);
            act.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public async Task GetAllAsync_ReturnsAllRows_OrderedByYearThenWindowType()
        {
            var rows = new[]
            {
                new SubmissionPeriod { Id = 1, WindowType = "Cso", RegistrationYear = 2025, OpeningDate = new DateTime(2024, 7, 1, 0, 0, 0, DateTimeKind.Utc), DeadlineDate = new DateTime(2025, 4, 2, 0, 0, 0, DateTimeKind.Utc), ClosingDate = new DateTime(2026, 2, 1, 0, 0, 0, DateTimeKind.Utc) },
                new SubmissionPeriod { Id = 2, WindowType = "Direct", RegistrationYear = 2025, OpeningDate = new DateTime(2024, 7, 1, 0, 0, 0, DateTimeKind.Utc), DeadlineDate = new DateTime(2025, 4, 2, 0, 0, 0, DateTimeKind.Utc), ClosingDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new SubmissionPeriod { Id = 3, WindowType = "CsoLargeProducer", RegistrationYear = 2026, OpeningDate = new DateTime(2025, 7, 1, 0, 0, 0, DateTimeKind.Utc), DeadlineDate = new DateTime(2025, 10, 2, 0, 0, 0, DateTimeKind.Utc), ClosingDate = new DateTime(2027, 1, 29, 0, 0, 0, DateTimeKind.Utc) },
            };
            _dbContextMock.Setup(c => c.SubmissionPeriod).ReturnsDbSet(rows);

            var result = await _sut.GetAllAsync(_ct);

            using (new AssertionScope())
            {
                result.Should().HaveCount(3);
                result[0].Id.Should().Be(1);
                result[0].WindowType.Should().Be("Cso");
                result[0].RegistrationYear.Should().Be(2025);
                result[1].Id.Should().Be(2);
                result[1].WindowType.Should().Be("Direct");
                result[2].Id.Should().Be(3);
                result[2].WindowType.Should().Be("CsoLargeProducer");
                result[2].RegistrationYear.Should().Be(2026);
            }
        }

        [TestMethod]
        public async Task GetAllAsync_EmptyTable_ReturnsEmpty()
        {
            _dbContextMock.Setup(c => c.SubmissionPeriod).ReturnsDbSet(Array.Empty<SubmissionPeriod>());

            var result = await _sut.GetAllAsync(_ct);

            result.Should().BeEmpty();
        }
    }
}
