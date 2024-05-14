using AutoFixture.MSTest;
using EPR.Payment.Service.Common.Data;
using EPR.Payment.Service.Common.Data.DataModels.Lookups;
using EPR.Payment.Service.Common.Data.Repositories;
using EPR.Payment.Service.Common.UnitTests.Mocks;
using EPR.Payment.Service.Common.UnitTests.TestHelpers;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.EntityFrameworkCore;

namespace EPR.Payment.Service.Data.UnitTests.Repositories
{
    [TestClass]
    public class FeesRepositoryTests
    {
        private Mock<DbSet<AccreditationFees>> _accreditationFeesMock = null!;

        [TestInitialize]
        public void TestInitialize()
        {
            _accreditationFeesMock = MockIAccreditationFeesRepository.GetMock(true);
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetAllFees_WhenFeesRecordsExistInTheDatabase_ReturnsAllFeesRecords(
            [Frozen] Mock<FeesPaymentDataContext> _feesPaymentDataContextMock,
            [Greedy] AccreditationFeesRepository _accreditationFeesRepository)
        {
            //Arrange
            _feesPaymentDataContextMock.Setup(i => i.AccreditationFees).ReturnsDbSet(_accreditationFeesMock.Object);
            _accreditationFeesRepository = new AccreditationFeesRepository(_feesPaymentDataContextMock.Object);
            var resultEffectiveDate = new DateTime(2024, 3, 31);

            //Act
            var result = await _accreditationFeesRepository.GetAllFeesAsync();

            //Assert
            using (new AssertionScope())
            {
                result.Count.Should().Be(2);
                result[0].Amount.Should().Be(10.0M);
                result[0].Large.Should().Be(true);
                result[0].Regulator.Should().Be("GB-ENG");
                result[0].EffectiveFrom.Should().Be(resultEffectiveDate);
                result[0].EffectiveTo.Should().Be(null);
            }
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetAllFees_WhenFeesRecordDoesNotExistInTheDatabase_ReturnsNoFeesRecords(
            [Frozen] Mock<FeesPaymentDataContext> _feesPaymentDataContextMock,
            [Greedy] AccreditationFeesRepository _accreditationFeesRepository)
        {
            //Arrange
            _accreditationFeesMock = MockIAccreditationFeesRepository.GetMock(false);
            _feesPaymentDataContextMock.Setup(i => i.AccreditationFees).ReturnsDbSet(_accreditationFeesMock.Object);
            _accreditationFeesRepository = new AccreditationFeesRepository(_feesPaymentDataContextMock.Object);

            //Act
            var result = await _accreditationFeesRepository.GetAllFeesAsync();

            //Assert
            result.Count.Should().Be(0);
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetFeesAmount_WhenFeesExistInTheDatabase_ReturnsFessAmount(
            [Frozen] Mock<FeesPaymentDataContext> _feesPaymentDataContextMock,
            [Greedy] AccreditationFeesRepository _accreditationFeesRepository)
        {
            //Arrange
            bool isLarge = true;
            string regulator = "GB-ENG";
            _feesPaymentDataContextMock.Setup(i => i.AccreditationFees).ReturnsDbSet(_accreditationFeesMock.Object);
            _accreditationFeesRepository = new AccreditationFeesRepository(_feesPaymentDataContextMock.Object);

            //Act
            var result = await _accreditationFeesRepository.GetFeesAmountAsync(isLarge,regulator);

            //Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result.Should().Be(10.0M);
            }
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetFeesAmount_WhenFeesDoesNotExistInTheDatabase_ReturnsNull(
            [Frozen] Mock<FeesPaymentDataContext> _feesPaymentDataContextMock,
            [Greedy] AccreditationFeesRepository _accreditationFeesRepository)
        {
            //Arrange
            bool isLarge = true;
            string regulator = "GB-ENG-test";
            _feesPaymentDataContextMock.Setup(i => i.AccreditationFees).ReturnsDbSet(_accreditationFeesMock.Object);
            _accreditationFeesRepository = new AccreditationFeesRepository(_feesPaymentDataContextMock.Object);

            //Act
            var result = await _accreditationFeesRepository.GetFeesAmountAsync(isLarge, regulator);

            //Assert
            result.Should().BeNull();
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetFees_WhenFeesRecordExistInTheDatabase_ReturnsFeesRecord(
            [Frozen] Mock<FeesPaymentDataContext> _feesPaymentDataContextMock,
            [Greedy] AccreditationFeesRepository _accreditationFeesRepository)
        {
            //Arrange
            bool isLarge = true;
            string regulator = "GB-ENG";
            _feesPaymentDataContextMock.Setup(i => i.AccreditationFees).ReturnsDbSet(_accreditationFeesMock.Object);
            _accreditationFeesRepository = new AccreditationFeesRepository(_feesPaymentDataContextMock.Object);
            var resultEffectiveDate = new DateTime(2024, 3, 31);

            //Act
            var result = await _accreditationFeesRepository.GetFeesAsync(isLarge, regulator);

            //Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result!.Amount.Should().Be(10.0M);
                result!.Large.Should().Be(true);
                result!.Regulator.Should().Be("GB-ENG");
                result!.EffectiveFrom.Should().Be(resultEffectiveDate);
                result!.EffectiveTo.Should().Be(null);
            }
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetFees_WhenFeesRecordDoesNotExistInTheDatabase_ReturnsNoFeesRecord(
            [Frozen] Mock<FeesPaymentDataContext> _feesPaymentDataContextMock,
            [Greedy] AccreditationFeesRepository _accreditationFeesRepository)
        {
            //Arrange
            bool isLarge = false;
            string regulator = "GB-ENG";
            _feesPaymentDataContextMock.Setup(i => i.AccreditationFees).ReturnsDbSet(_accreditationFeesMock.Object);
            _accreditationFeesRepository = new AccreditationFeesRepository(_feesPaymentDataContextMock.Object);

            //Act
            var result = await _accreditationFeesRepository.GetFeesAsync(isLarge, regulator);

            //Assert
            result.Should().BeNull();
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetFeesCount_WhenFeesExistInTheDatabase_ReturnsCountOfFeesRecords(
            [Frozen] Mock<FeesPaymentDataContext> _feesPaymentDataContextMock,
            [Greedy] AccreditationFeesRepository _accreditationFeesRepository)
        {
            //Arrange
            _feesPaymentDataContextMock.Setup(i => i.AccreditationFees).ReturnsDbSet(_accreditationFeesMock.Object);
            _accreditationFeesRepository = new AccreditationFeesRepository(_feesPaymentDataContextMock.Object);

            //Act
            var result = await _accreditationFeesRepository.GetFeesCount();

            //Assert
            result.Should().Be(2);
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetFeesCount_WhenFeesDoesNotExistInTheDatabase_ReturnsZeroCountOfFeesRecords(
            [Frozen] Mock<FeesPaymentDataContext> _feesPaymentDataContextMock,
            [Greedy] AccreditationFeesRepository _accreditationFeesRepository)
        {
            //Arrange
            _accreditationFeesMock = MockIAccreditationFeesRepository.GetMock(false);
            _feesPaymentDataContextMock.Setup(i => i.AccreditationFees).ReturnsDbSet(_accreditationFeesMock.Object);
            _accreditationFeesRepository = new AccreditationFeesRepository(_feesPaymentDataContextMock.Object);

            //Act
            var result = await _accreditationFeesRepository.GetFeesCount();

            //Assert
            result.Should().Be(0);
        }
    }
}