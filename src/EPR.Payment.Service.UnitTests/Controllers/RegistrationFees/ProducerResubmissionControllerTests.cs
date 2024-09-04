﻿using AutoFixture.MSTest;
using EPR.Payment.Service.Common.UnitTests.TestHelpers;
using EPR.Payment.Service.Controllers.RegistrationFees;
using EPR.Payment.Service.Services.Interfaces.RegistrationFees;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EPR.Payment.Service.UnitTests.Controllers.RegistrationFees
{
    [TestClass]
    public class ProducerResubmissionControllerTests
    {
        private ProducerResubmissionController _controller = null!;
        private Mock<IProducerResubmissionService> _registrationFeesServiceMock = null!;
        private CancellationToken _cancellationToken;

        [TestInitialize]
        public void TestInitialize()
        {
            _registrationFeesServiceMock = new Mock<IProducerResubmissionService>();
            _controller = new ProducerResubmissionController(_registrationFeesServiceMock.Object);
            _cancellationToken = new CancellationToken();
        }

        [TestMethod]
        public void Constructor_WithValidArguments_ShouldInitializeCorrectly()
        {
            // Act
            var controller = new ProducerResubmissionController(
                _registrationFeesServiceMock.Object
            );

            // Assert
            controller.Should().NotBeNull();
            controller.Should().BeAssignableTo<ProducerResubmissionController>();
        }

        [TestMethod]
        public void Constructor_WithNullRegistrationFeesService_ShouldThrowArgumentNullException()
        {

            // Act
            Action act = () => new ProducerResubmissionController(
                null!
            );

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithParameterName("producerResubmissionService");
        }

        [TestMethod, AutoMoqData]
        public async Task GetResubmissionAsync_ServiceReturnsAResult_ShouldReturnOkResponse(
            [Frozen] string regulator,
            [Frozen] decimal expectedAmount)
        {
            //Arrange
            _registrationFeesServiceMock.Setup(i => i.GetResubmissionAsync(regulator, _cancellationToken)).ReturnsAsync(expectedAmount);

            //Act
            var result = await _controller.GetResubmissionAsync(regulator, _cancellationToken);

            //Assert
            result.Should().BeOfType<OkObjectResult>();
            result.As<OkObjectResult>().Should().NotBeNull();
        }

        [TestMethod, AutoMoqData]
        public async Task GetResubmissionAsync_ServiceThrowsException_ShouldReturnInternalServerError(
            [Frozen] string regulator)
        {
            // Arrange
            _registrationFeesServiceMock.Setup(i => i.GetResubmissionAsync(regulator, _cancellationToken))
                               .ThrowsAsync(new Exception("Test Exception"));

            // Act
            var result = await _controller.GetResubmissionAsync(regulator, _cancellationToken);

            // Assert
            result.Should().BeOfType<ObjectResult>().Which.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }

        [TestMethod]
        public async Task GetResubmissionAsync_EmptyRegulator_ShouldReturnBadRequest()
        {
            // Arrange
            string regulator = string.Empty;

            var result = await _controller.GetResubmissionAsync(regulator, _cancellationToken);

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public async Task GetResubmissionAsync_NullRegulator_ShouldReturnBadRequest()
        {
            // Arrange
            string regulator = null!;

            var result = await _controller.GetResubmissionAsync(regulator, _cancellationToken);

            result.Should().BeOfType<BadRequestObjectResult>();
        }
    }
}