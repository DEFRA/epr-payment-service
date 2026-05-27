using EPR.Payment.Service.Common.Constants.RegistrationFees;
using EPR.Payment.Service.Common.Constants.RegistrationFees.LookUps;
using EPR.Payment.Service.Common.Data.Helper;
using EPR.Payment.Service.Common.Data.Interfaces;
using EPR.Payment.Service.Common.Data.Repositories.RegistrationFees;
using EPR.Payment.Service.Common.UnitTests.Mocks;
using EPR.Payment.Service.Common.ValueObjects.RegistrationFees;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.EntityFrameworkCore;
using System.Data.Entity;
using System.Reflection;

namespace EPR.Payment.Service.Data.UnitTests.Repositories.Fees
{
    [TestClass]
    public class BaseFeeRepositoryTests
    {
        private Mock<IAppDbContext> _dbContextMock = null!;
        private FeesKeyValueStore _keyValueStore = null!;
        private TestFeeRepository _repository = null!;
        private Mock<DbSet<Common.Data.DataModels.Lookups.RegistrationFees>> _registrationFeesMock = null!;

        [TestInitialize]
        public void Setup()
        {
            _dbContextMock = new Mock<IAppDbContext>();
            _keyValueStore = new FeesKeyValueStore();
            _repository = new TestFeeRepository(_dbContextMock.Object, _keyValueStore);
            _registrationFeesMock = MockIRegistrationFeesRepository.GetRegistrationFeesMock();
        }

        [TestMethod]
        public void GetInMemoryKey_ShouldCreateCorrectKey()
        {
            // Arrange
            var groupType = GroupTypeConstants.ProducerType;
            var subGroupType = "Small";
            var regulator = RegulatorType.Create("GB-ENG");
            var expectedKey = "producertypeSmallGB-ENG";

            // Act
            var getInMemoryKeyMethod = typeof(BaseFeeRepository).GetMethod("GetInMemoryKey", BindingFlags.Static | BindingFlags.NonPublic);
            var result = (string?)getInMemoryKeyMethod!.Invoke(null, [groupType, subGroupType, regulator]);

            // Assert
            result.Should().Be(expectedKey);
        }

        [TestMethod]
        public async Task GetFeeAsync_IfKeyExistsInMemory_ShouldReturnFeeFromMemoryWithoutHittingDatabase()
        {
            // Arrange
            var groupType = GroupTypeConstants.ProducerType;
            var subGroupType = "Small";
            var regulator = RegulatorType.Create("GB-ENG");
            var submissionDate = DateTime.UtcNow;
            var expectedFee = 121600m;

            var cachedRows = new List<Common.Data.DataModels.Lookups.RegistrationFees>
            {
                new Common.Data.DataModels.Lookups.RegistrationFees
                {
                    Amount = expectedFee,
                    EffectiveFrom = DateTime.UtcNow.AddDays(-10),
                    EffectiveTo = DateTime.UtcNow.AddDays(10),
                },
            };

            var getInMemoryKeyMethod = typeof(BaseFeeRepository).GetMethod("GetInMemoryKey", BindingFlags.Static | BindingFlags.NonPublic);
            var inMemoryKey = (string?)getInMemoryKeyMethod!.Invoke(null, [groupType, subGroupType, regulator]);
            _keyValueStore.Add(inMemoryKey!, cachedRows);

            // Act
            var result = await _repository.GetFeeAsyncPublic(groupType, subGroupType, regulator, submissionDate, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                result.Should().Be(expectedFee);
                _dbContextMock.Verify(x => x.RegistrationFees, Times.Never);
            }
        }

        [TestMethod]
        public async Task GetFeeAsync_IfKeyDoesNotExistInMemory_ShouldFetchRowsFromDatabaseAndCacheThem()
        {
            // Arrange
            var groupType = GroupTypeConstants.ProducerType;
            var subGroupType = "Small";
            var regulator = RegulatorType.Create("GB-ENG");
            var submissionDate = DateTime.UtcNow;
            var expectedFee = 121600M;

            _dbContextMock.Setup(i => i.RegistrationFees).ReturnsDbSet(_registrationFeesMock.Object);

            var getInMemoryKeyMethod = typeof(BaseFeeRepository).GetMethod("GetInMemoryKey", BindingFlags.Static | BindingFlags.NonPublic);
            var inMemoryKey = (string?)getInMemoryKeyMethod!.Invoke(null, [groupType, subGroupType, regulator]);

            // Act
            var result = await _repository.GetFeeAsyncPublic(groupType, subGroupType, regulator, submissionDate, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                result.Should().Be(expectedFee);
                _keyValueStore.Data.Should().ContainKey(inMemoryKey!);
                _keyValueStore.Data[inMemoryKey!].Should().BeOfType<List<Common.Data.DataModels.Lookups.RegistrationFees>>();
            }
        }

        [TestMethod]
        public async Task GetFeeAsync_WhenQueriedAcrossDifferentDateBands_ShouldReturnCorrectFeePerDate()
        {
            // Regression test: previously the cache stored a single decimal keyed without the
            // submission date, so whichever date hit the cache first pinned the fee. Now the cache
            // stores the row set and each call filters by date.

            // Arrange
            var groupType = GroupTypeConstants.ComplianceScheme;
            var subGroupType = ComplianceSchemeConstants.Registration;
            var regulator = RegulatorType.Create("GB-ENG");

            _dbContextMock.Setup(i => i.RegistrationFees).ReturnsDbSet(_registrationFeesMock.Object);

            // Expired-band row: EffectiveFrom = UtcNow-30, EffectiveTo = UtcNow-15, Amount = 1280400.
            // Future-effective row: EffectiveFrom = UtcNow+5, EffectiveTo = UtcNow+20, Amount = 1480400.
            var expiredDate = DateTime.UtcNow.AddDays(-20);
            var futureDate = DateTime.UtcNow.AddDays(10);

            // Act — query the expired band first so it would have poisoned the old cache.
            var expiredFee = await _repository.GetFeeAsyncPublic(groupType, subGroupType, regulator, expiredDate, CancellationToken.None);
            var futureFee = await _repository.GetFeeAsyncPublic(groupType, subGroupType, regulator, futureDate, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                expiredFee.Should().Be(1280400m);
                futureFee.Should().Be(1480400m);
            }
        }

        [TestMethod]
        public async Task GetFeeAsync_FutureSubmissionDateCoveredByExistingRow_ShouldReturnThatRowsAmount()
        {
            // Arrange
            var groupType = GroupTypeConstants.ComplianceScheme;
            var subGroupType = ComplianceSchemeConstants.Registration;
            var regulator = RegulatorType.Create("GB-ENG");
            var futureDate = DateTime.UtcNow.AddDays(15);

            _dbContextMock.Setup(i => i.RegistrationFees).ReturnsDbSet(_registrationFeesMock.Object);

            // Act
            var result = await _repository.GetFeeAsyncPublic(groupType, subGroupType, regulator, futureDate, CancellationToken.None);

            // Assert — matches the "Future Effective" compliance scheme row (Amount = 1480400).
            result.Should().Be(1480400m);
        }

        [TestMethod]
        public async Task GetFeeAsync_FutureSubmissionDateNotCoveredByAnyRow_ShouldThrowArgumentException()
        {
            // Arrange
            var groupType = GroupTypeConstants.ComplianceScheme;
            var subGroupType = ComplianceSchemeConstants.Registration;
            var regulator = RegulatorType.Create("GB-ENG");
            var farFutureDate = DateTime.UtcNow.AddYears(5);

            _dbContextMock.Setup(i => i.RegistrationFees).ReturnsDbSet(_registrationFeesMock.Object);

            // Act & Assert
            await _repository.Invoking(r => r.GetFeeAsyncPublic(groupType, subGroupType, regulator, farFutureDate, CancellationToken.None))
                .Should().ThrowAsync<ArgumentException>()
                .WithMessage(ValidationMessages.SubmissionDateIsNotInRange);
        }

        [TestMethod]
        public async Task GetFeeAsync_WhenNoRowsExistForGroupSubGroupRegulator_ShouldReturnZero()
        {
            // Arrange
            var groupType = GroupTypeConstants.ProducerType;
            var subGroupType = "Small";
            var regulator = RegulatorType.Create("GB-SCT");
            var submissionDate = DateTime.UtcNow;

            _dbContextMock.Setup(i => i.RegistrationFees).ReturnsDbSet(_registrationFeesMock.Object);

            // Act
            var result = await _repository.GetFeeAsyncPublic(groupType, subGroupType, regulator, submissionDate, CancellationToken.None);

            // Assert
            result.Should().Be(0m);
        }
    }

    // Helper repository class to test BaseFeeRepository
    class TestFeeRepository : BaseFeeRepository
    {
        public TestFeeRepository(IAppDbContext dataContext, FeesKeyValueStore keyValueStore)
            : base(dataContext, keyValueStore) { }

        // Wrapper for testing protected GetFeeAsync
        public Task<decimal> GetFeeAsyncPublic(string groupType, string subGroupType, RegulatorType regulator, DateTime submissionDate, CancellationToken cancellationToken)
        {
            return GetFeeAsync(groupType, subGroupType, regulator, submissionDate, cancellationToken);
        }
    }

}
