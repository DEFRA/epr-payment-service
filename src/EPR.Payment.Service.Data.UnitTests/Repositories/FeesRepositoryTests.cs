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
//using NUnit.Framework;

namespace EPR.Payment.Service.Data.UnitTests.Repositories
{
    [TestClass]
    public class FeesRepositoryTests
    {
        private Mock<DbSet<AccreditationFees>> _accreditationFeesMock = null!;

        [TestInitialize]
        public void TestInitialize()
        {
            _accreditationFeesMock = MockIAccreditationFeesRepository.GetMock();
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetAllFees_WhenCalled_ReturnsFees(
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
                result.Count.Should().Be(1);
                result[0].Amount.Should().Be(10.0M);
                result[0].Large.Should().Be(true);
                result[0].Regulator.Should().Be("GB-ENG");
                result[0].EffectiveFrom.Should().Be(resultEffectiveDate);
                result[0].EffectiveTo.Should().Be(null);
            }
        }

        //[TestMethod]
        //[AutoMoqData]
        //public async Task GetFeesAmount_WhenCalled_ReturnsFessAmount(
        //    [Frozen] Mock<FeesPaymentDataContext> _feesPaymentDataContextMock,
        //    [Greedy] AccreditationFeesRepository _accreditationFeesRepository,
        //    [Values("Service1", "Service2")] bool isLarge,
        //    [Values("Service1", "Service2")] string serviceName)
        //{
        //    //Arrange
        //    _feesPaymentDataContextMock.Setup(i => i.AccreditationFees).ReturnsDbSet(_accreditationFeesMock.Object);
        //    _accreditationFeesRepository = new AccreditationFeesRepository(_feesPaymentDataContextMock.Object);
        //    var resultEffectiveDate = new DateTime(2024, 3, 31);

        //    //Act
        //    var result = await _accreditationFeesRepository.GetAllFeesAsync();

        //    //Assert
        //    using (new AssertionScope())
        //    {
        //        result.Count.Should().Be(1);
        //        result[0].Amount.Should().Be(10.0M);
        //        result[0].Large.Should().Be(true);
        //        result[0].Regulator.Should().Be("GB-ENG");
        //        result[0].EffectiveFrom.Should().Be(resultEffectiveDate);
        //        result[0].EffectiveTo.Should().Be(null);
        //    }
        //}
    }
}