using AutoFixture;
using AutoFixture.MSTest;
using EPR.Payment.Service.Common.Dtos.Requests;
using EPR.Payment.Service.Common.Dtos.Responses;
using EPR.Payment.Service.Common.UnitTests.TestHelpers;
using EPR.Payment.Service.Controllers.RegistrationFees;
using EPR.Payment.Service.Services.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Diagnostics.Metrics;

namespace EPR.Payment.Service.UnitTests.Controllers
{
    [TestClass]
    public class ProducersControllerTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IProducerFeesService> _producerFeesServiceMock;
        private readonly ProducerFeesController _controller;

        public ProducersControllerTests()
        {
            _fixture = new Fixture();
            _producerFeesServiceMock = new Mock<IProducerFeesService>();
            _controller = new ProducerFeesController(_producerFeesServiceMock.Object);
        }

        [TestMethod]
        [AutoMoqData]
        public async Task CalculateFees_ServiceReturnsAResult_ReturnsCalculatedResponse(
            [Frozen] RegistrationFeeResponseDto expectedFeesResponse)
        {
            var request = _fixture.Build<ProducerRegistrationRequestDto>().With(d => d.ProducerType , "L").With(x => x.NumberOfSubsidiaries , 20).Create();

            //Arrange
            _producerFeesServiceMock.Setup(i => i.CalculateFeesAsync(request)).ReturnsAsync(expectedFeesResponse);

            //Act
            var result = await _controller.CalculateFees(request);

            //Assert

            result.Result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeEquivalentTo(expectedFeesResponse);
        }

        [TestMethod]
        public async Task CalculateFees_ServiceReturnsBadRequest_WhenProducerTypeIsInvalid()
        {
            //Arrange
            var request = _fixture.Build<ProducerRegistrationRequestDto>().With(d => d.ProducerType, "A").With(x => x.NumberOfSubsidiaries, 20).Create();

            //Act
            var result = await _controller.CalculateFees(request);

            //Assert
            result.Result.Should().BeOfType<BadRequestObjectResult>();
            result.Result.As<BadRequestObjectResult>().Should().NotBeNull();
        }

        [TestMethod]
        public async Task CalculateFees_ServiceReturnsBadRequest_WhenNumberOfSubsidiariesIsInvalid()
        {
            //Arrange
            var request = _fixture.Build<ProducerRegistrationRequestDto>().With(d => d.ProducerType, "L").With(x => x.NumberOfSubsidiaries, 110).Create();

            //Act
            var result = await _controller.CalculateFees(request);

            //Assert
            result.Result.Should().BeOfType<BadRequestObjectResult>();
            result.Result.As<BadRequestObjectResult>().Should().NotBeNull();
        }
    }
}