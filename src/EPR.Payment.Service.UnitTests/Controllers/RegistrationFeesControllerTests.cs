using AutoFixture.MSTest;
using EPR.Payment.Service.Common.UnitTests.TestHelpers;
using EPR.Payment.Service.Controllers;
using EPR.Payment.Service.Services.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EPR.Payment.Service.UnitTests.Controllers
{
    [TestClass]
    public class RegistrationFeesControllerTests
    {
        private RegistrationFeesController _controller = null!;
        private Mock<IRegistrationFeesService> _registrationFeesServiceMock = null!;
        private CancellationToken _cancellationToken;

        [TestInitialize]
        public void TestInitialize()
        {
            _registrationFeesServiceMock = new Mock<IRegistrationFeesService>();
            _controller = new RegistrationFeesController(_registrationFeesServiceMock.Object);
            _cancellationToken = new CancellationToken();
        }

        [TestMethod]
        public void Constructor_WithValidArguments_ShouldInitializeCorrectly()
        {
            // Act
            var controller = new RegistrationFeesController(
                _registrationFeesServiceMock.Object
            );

            // Assert
            controller.Should().NotBeNull();
            controller.Should().BeAssignableTo<RegistrationFeesController>();
        }

        [TestMethod]
        public void Constructor_WithNullRegistrationFeesService_ShouldThrowArgumentNullException()
        {

            // Act
            Action act = () => new RegistrationFeesController(
                null!
            );

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithParameterName("registrationFeesService");
        }

        [TestMethod, AutoMoqData]
        public async Task GetProducerResubmissionAmountByRegulator_ServiceReturnsAResult_ShouldReturnOkResponse(
            [Frozen] string regulator,
            [Frozen] decimal expectedAmount)
        {
            //Arrange
            _registrationFeesServiceMock.Setup(i => i.GetProducerResubmissionAmountByRegulatorAsync(regulator, _cancellationToken)).ReturnsAsync(expectedAmount);

            //Act
            var result = await _controller.GetProducerResubmissionAmountByRegulator(regulator, _cancellationToken);

            //Assert
            result.Should().BeOfType<OkObjectResult>();
            result.As<OkObjectResult>().Should().NotBeNull();
        }

        [TestMethod, AutoMoqData]
        public async Task GetProducerResubmissionAmountByRegulator_ServiceThrowsException_ShouldReturnInternalServerError(
            [Frozen] string regulator)
        {
            // Arrange
            _registrationFeesServiceMock.Setup(i => i.GetProducerResubmissionAmountByRegulatorAsync(regulator, _cancellationToken))
                               .ThrowsAsync(new Exception("Test Exception"));

            // Act
            var result = await _controller.GetProducerResubmissionAmountByRegulator(regulator, _cancellationToken);

            // Assert
            result.Should().BeOfType<ObjectResult>().Which.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }

        [TestMethod]
        public async Task GetProducerResubmissionAmountByRegulator_EmptyRegulator_ShouldReturnBadRequest()
        {
            // Arrange
            string regulator = string.Empty;

            var result = await _controller.GetProducerResubmissionAmountByRegulator(regulator, _cancellationToken);

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public async Task GetProducerResubmissionAmountByRegulator_NullRegulator_ShouldReturnBadRequest()
        {
            // Arrange
            string regulator = null!;

            var result = await _controller.GetProducerResubmissionAmountByRegulator(regulator, _cancellationToken);

            result.Should().BeOfType<BadRequestObjectResult>();
        }
    }
}
