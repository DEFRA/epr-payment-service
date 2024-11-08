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
        public async Task GetFeeAsync_IfKeyExistsInMemory_ShouldReturnFeeFromMemory()
        {
            // Arrange
            var groupType = GroupTypeConstants.ProducerType;
            var subGroupType = "Small";
            var regulator = RegulatorType.Create("GB-ENG");
            var submissionDate = DateTime.UtcNow;
            var expectedFee = 100m;

            // Act
            var getInMemoryKeyMethod = typeof(BaseFeeRepository).GetMethod("GetInMemoryKey", BindingFlags.Static | BindingFlags.NonPublic);
            var inMemoryKey = (string?)getInMemoryKeyMethod!.Invoke(null, [groupType, subGroupType, regulator]);

            _keyValueStore.Add(inMemoryKey!, expectedFee);

            var result = await _repository.GetFeeAsyncPublic(groupType, subGroupType, regulator, submissionDate, CancellationToken.None);

            // Assert
            using(new AssertionScope())
            {
                result.Should().Be(expectedFee);
                _dbContextMock.Verify(x => x.RegistrationFees, Times.Never);
            }
        }

        [TestMethod]
        public async Task GetFeeAsync_IfKeyDoesNotExistInMemory_ShouldFetchFeeFromDatabase()
        {
            // Arrange
            var groupType = GroupTypeConstants.ProducerType;
            var subGroupType = "Small";
            var regulator = RegulatorType.Create("GB-ENG");
            var submissionDate = DateTime.UtcNow;
            var expectedFee = 121600M;

            _dbContextMock.Setup(i => i.RegistrationFees).ReturnsDbSet(_registrationFeesMock.Object);

            // Act
            var getInMemoryKeyMethod = typeof(BaseFeeRepository).GetMethod("GetInMemoryKey", BindingFlags.Static | BindingFlags.NonPublic);
            var inMemoryKey = (string?)getInMemoryKeyMethod!.Invoke(null, [groupType, subGroupType, regulator]);

            var result = await _repository.GetFeeAsyncPublic(groupType, subGroupType, regulator, submissionDate, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                result.Should().Be(expectedFee);
                _keyValueStore.Data.Should().ContainKey(inMemoryKey!).And.Subject[inMemoryKey!].Should().Be(expectedFee);
            }

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
