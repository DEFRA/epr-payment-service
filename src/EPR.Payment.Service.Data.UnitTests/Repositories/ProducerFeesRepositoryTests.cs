using AutoFixture;
using AutoFixture.MSTest;
using Azure.Core;
using EPR.Payment.Service.Common.Data;
using EPR.Payment.Service.Common.Data.DataModels.Lookups;
using EPR.Payment.Service.Common.Data.Repositories;
using EPR.Payment.Service.Common.Dtos.Requests;
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
    public class ProducerRegitrationFeesRepositoryTests
    {
        private Mock<DbSet<ProducerRegitrationFees>> _feesMock;
        private readonly Mock<FeesPaymentDataContext> _feesPaymentDataContextMock;
        private readonly ProducerFeesRepository _feesRepository;
        private readonly IFixture _fixture;

        public ProducerRegitrationFeesRepositoryTests()
        {
            _feesMock = MockIProducerRegitrationFeesRepository.GetMock(true);
            _feesPaymentDataContextMock = new Mock<FeesPaymentDataContext>();
            _feesRepository = new ProducerFeesRepository(_feesPaymentDataContextMock.Object);
            _fixture = new Fixture();
        }

        [TestMethod]
        public async Task GetProducerFeesAmountAsync_WhenFeesExistInTheDatabase_ReturnsFessAmount()
        {
            //Arrange
            var request = _fixture.Build<ProducerRegistrationRequestDto>().With(d => d.ProducerType, "L").With(x => x.Country, "GB-ENG").Create();
            _feesPaymentDataContextMock.Setup(i => i.ProducerRegitrationFees).ReturnsDbSet(_feesMock.Object);

            //Act
            var result = await _feesRepository.GetProducerFeesAmountAsync(request);

            //Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result.Should().Be(10.0M);
            }
        }

        [TestMethod]
        public async Task GetProducerFeesAmountAsync_WhenFeesDoesNotExistInTheDatabase_ReturnsNull()
        {
            //Arrange
            var request = _fixture.Build<ProducerRegistrationRequestDto>().With(d => d.ProducerType, "L").With(x => x.Country, "GB-ENG-test").Create();
            _feesPaymentDataContextMock.Setup(i => i.ProducerRegitrationFees).ReturnsDbSet(_feesMock.Object);

            //Act
            var result = await _feesRepository.GetProducerFeesAmountAsync(request);

            //Assert
            result.Should().BeNull();
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetProducerRegitrationFeesCount_WhenFeesExistInTheDatabase_ReturnsCountOfFeesRecords()
        {
            //Arrange
            _feesPaymentDataContextMock.Setup(i => i.ProducerRegitrationFees).ReturnsDbSet(_feesMock.Object);

            //Act
            var result = await _feesRepository.GetProducerRegitrationFeesCount();

            //Assert
            result.Should().Be(2);
        }

        [TestMethod]
        public async Task GetProducerRegitrationFeesCount_WhenFeesDoesNotExistInTheDatabase_ReturnsZeroCountOfFeesRecords()
        {
            //Arrange
            _feesMock = MockIProducerRegitrationFeesRepository.GetMock(false);
            _feesPaymentDataContextMock.Setup(i => i.ProducerRegitrationFees).ReturnsDbSet(_feesMock.Object);

            //Act
            var result = await _feesRepository.GetProducerRegitrationFeesCount();

            //Assert
            result.Should().Be(0);
        }
    }
}