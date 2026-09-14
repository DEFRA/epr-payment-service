using EPR.Payment.Service.Common.Data.DataModels;
using EPR.Payment.Service.Common.Data.Interfaces;
using EPR.Payment.Service.Common.Data.Repositories.RegistrationSubmission;
using EPR.Payment.Service.Common.Enums;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.EntityFrameworkCore;

namespace EPR.Payment.Service.Data.UnitTests.Repositories.RegistrationSubmission
{
    [TestClass]
    public class RegistrationFeeSnapshotRepositoryTests
    {
        private Mock<IAppDbContext> _dataContextMock = null!;
        private RegistrationFeeSnapshotRepository _sut = null!;
        private CancellationToken _ct;

        [TestInitialize]
        public void Init()
        {
            _dataContextMock = new Mock<IAppDbContext>();
            _sut = new RegistrationFeeSnapshotRepository(_dataContextMock.Object);
            _ct = CancellationToken.None;
        }

        [TestMethod]
        public void Constructor_NullContext_Throws()
        {
            Action act = () => new RegistrationFeeSnapshotRepository(null!);
            act.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public async Task CreateAsync_NullSnapshot_Throws()
        {
            Func<Task> act = () => _sut.CreateAsync(null!, _ct);
            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [TestMethod]
        public async Task CreateAsync_AddsSnapshotAndSavesOnce()
        {
            var dbSetMock = new Mock<DbSet<RegistrationFeeSnapshot>>();
            _dataContextMock.Setup(c => c.RegistrationFeeSnapshot).Returns(dbSetMock.Object);
            _dataContextMock.Setup(c => c.SaveChangesAsync(_ct)).ReturnsAsync(1);

            var snapshot = new RegistrationFeeSnapshot
            {
                Id = Guid.NewGuid(),
                RegistrationSubmissionDataId = Guid.NewGuid(),
                TotalFee = 1000m,
                LineItems = new List<RegistrationFeeLineItem>
                {
                    new()
                    {
                        FeeTypeId = FeeTypeIds.ProducerRegistrationFee,
                        FeeTypeName = nameof(FeeTypeIds.ProducerRegistrationFee),
                        Amount = 1000m,
                    },
                },
            };

            var resultId = await _sut.CreateAsync(snapshot, _ct);

            using (new AssertionScope())
            {
                resultId.Should().Be(snapshot.Id);
                dbSetMock.Verify(s => s.Add(snapshot), Times.Once);
                _dataContextMock.Verify(c => c.SaveChangesAsync(_ct), Times.Once);
            }
        }

        [TestMethod]
        public async Task GetByRegistrationSubmissionDataIdAsync_Match_ReturnsSnapshot()
        {
            var rsdId = Guid.NewGuid();
            var snapshot = new RegistrationFeeSnapshot
            {
                Id = Guid.NewGuid(),
                RegistrationSubmissionDataId = rsdId,
                TotalFee = 500m,
                LineItems = new List<RegistrationFeeLineItem>(),
            };
            _dataContextMock.Setup(c => c.RegistrationFeeSnapshot).ReturnsDbSet(new[] { snapshot });

            var result = await _sut.GetByRegistrationSubmissionDataIdAsync(rsdId, _ct);

            result.Should().NotBeNull();
            result!.Id.Should().Be(snapshot.Id);
        }

        [TestMethod]
        public async Task GetByRegistrationSubmissionDataIdAsync_NoMatch_ReturnsNull()
        {
            _dataContextMock.Setup(c => c.RegistrationFeeSnapshot).ReturnsDbSet(Array.Empty<RegistrationFeeSnapshot>());

            var result = await _sut.GetByRegistrationSubmissionDataIdAsync(Guid.NewGuid(), _ct);

            result.Should().BeNull();
        }

        [TestMethod]
        public async Task CreateAsync_DbUpdateException_Rethrows()
        {
            var dbSetMock = new Mock<DbSet<RegistrationFeeSnapshot>>();
            _dataContextMock.Setup(c => c.RegistrationFeeSnapshot).Returns(dbSetMock.Object);
            _dataContextMock.Setup(c => c.SaveChangesAsync(_ct))
                .ThrowsAsync(new DbUpdateException("save failed", new InvalidOperationException("inner")));

            var snapshot = new RegistrationFeeSnapshot
            {
                Id = Guid.NewGuid(),
                RegistrationSubmissionDataId = Guid.NewGuid(),
                TotalFee = 0m,
            };

            Func<Task> act = () => _sut.CreateAsync(snapshot, _ct);

            await act.Should().ThrowAsync<DbUpdateException>().WithMessage("save failed");
        }
    }
}
