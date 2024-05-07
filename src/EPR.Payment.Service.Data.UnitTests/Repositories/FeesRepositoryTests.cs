using EPR.Payment.Service.Common.Data;
using EPR.Payment.Service.Common.Data.DataModels.Lookups;
using EPR.Payment.Service.Common.Data.Repositories;
using EPR.Payment.Service.Common.UnitTests.Mocks;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;
using NUnit.Framework;

namespace EPR.Payment.Service.Data.UnitTests.Repositories
{
    [TestFixture]
    public class FeesRepositoryTests
    {
        private Mock<FeesPaymentDataContext> _feesPaymentDataContextMock = null!;
        private AccreditationFeesRepository _accreditationFeesRepository = null!;
        private Mock<DbSet<AccreditationFees>> _accreditationFeesMock = null!;

        [SetUp]
        public void SetUp()
        {
            _feesPaymentDataContextMock = new Mock<FeesPaymentDataContext>();
            _accreditationFeesMock = MockIAccreditationFeesRepository.GetMock();
            _feesPaymentDataContextMock.Setup(i => i.AccreditationFees).ReturnsDbSet(_accreditationFeesMock.Object);
            _accreditationFeesRepository = new AccreditationFeesRepository(_feesPaymentDataContextMock.Object);
        }

        [Test]
        public async Task FeesRepository_GetAllFees_ReturnsFess()
        {
            //Arrange
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

    }
}