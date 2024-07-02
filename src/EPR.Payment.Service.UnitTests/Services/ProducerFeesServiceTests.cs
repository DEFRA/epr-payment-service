using AutoFixture;
using AutoFixture.MSTest;
using AutoMapper;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories;
using EPR.Payment.Service.Common.Dtos.Requests;
using EPR.Payment.Service.Common.Dtos.Responses;
using EPR.Payment.Service.Common.UnitTests.TestHelpers;
using EPR.Payment.Service.Services;
using FluentAssertions;
using Moq;
using Newtonsoft.Json.Linq;

namespace EPR.Payment.Service.UnitTests.Services
{
    [TestClass]
    public class ProducerFeesServiceTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IProducerFeesRepository> _producerFeesRepositoryMock;
        private readonly ProducerFeesService _producerFeesService;
        private readonly Mock<IMapper> _mapperMock;

        public ProducerFeesServiceTests()
        {
            _fixture = new Fixture();
            _mapperMock = new Mock<IMapper>();
            _producerFeesRepositoryMock = new Mock<IProducerFeesRepository>();
            _producerFeesService = new ProducerFeesService(_mapperMock.Object, _producerFeesRepositoryMock.Object);
        }

        [TestMethod]
        public async Task CalculateFeesAsync_ReturnsSuccessfulCalculatedResult()
        {
            //Arrange
            var request = _fixture.Build<ProducerRegistrationRequestDto>().With(d => d.ProducerType, "L").With(x => x.NumberOfSubsidiaries, 20).Create();
            var expectedFeesResponse = new RegistrationFeeResponseDto{ ProducersFee = 10, SubsidiariesFee = 20, BaseFee = 10, TotalFee = 30};

            _producerFeesRepositoryMock.Setup(i => i.GetProducerFeesAmountAsync(request)).ReturnsAsync(expectedFeesResponse.ProducersFee);
            _producerFeesRepositoryMock.Setup(i => i.GetProducerSubsFeesAmountAsync(request)).ReturnsAsync(expectedFeesResponse.SubsidiariesFee);

            //Act
            var result = await _producerFeesService.CalculateFeesAsync(request);

            //Assert
            result.Should().BeEquivalentTo(expectedFeesResponse);
        }

        [TestMethod]
        public async Task CalculateFeesAsync_ReturnsNoRecords_WhenProducerTypeIsInvalid()
        {
            //Arrange
            var request = _fixture.Build<ProducerRegistrationRequestDto>().With(d => d.ProducerType, "A").With(x => x.NumberOfSubsidiaries, 20).Create();
            var expectedResult = _fixture.Build<RegistrationFeeResponseDto>().With(d => d.TotalFee, 0).Create();

            _producerFeesRepositoryMock.Setup(i => i.GetProducerFeesAmountAsync(request)).ReturnsAsync((decimal?)null);
            _producerFeesRepositoryMock.Setup(i => i.GetProducerSubsFeesAmountAsync(request)).ReturnsAsync((decimal?)null);

            //Act
            var result = await _producerFeesService.CalculateFeesAsync(request);

            //Assert
            result.TotalFee.Should().Be(expectedResult.TotalFee);
        }

        [TestMethod]
        public async Task CalculateFeesAsync_ReturnsNoRecords_WhenNumberOfSubsidiariesIsInvalid()
        {
            //Arrange
            var request = _fixture.Build<ProducerRegistrationRequestDto>().With(d => d.ProducerType, "L").With(x => x.NumberOfSubsidiaries, 110).Create();
            var expectedResult = _fixture.Build<RegistrationFeeResponseDto>().With(d => d.TotalFee, 0).Create();

            _producerFeesRepositoryMock.Setup(i => i.GetProducerFeesAmountAsync(request)).ReturnsAsync((decimal?)null);
            _producerFeesRepositoryMock.Setup(i => i.GetProducerSubsFeesAmountAsync(request)).ReturnsAsync((decimal?)null);

            //Act
            var result = await _producerFeesService.CalculateFeesAsync(request);

            //Assert
            result.TotalFee.Should().Be(expectedResult.TotalFee);
        }

    }
}
