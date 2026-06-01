using AutoFixture.MSTest;
using EPR.Payment.Service.Common.UnitTests.TestHelpers;
using EPR.Payment.Service.Controllers.Registrations;
using EPR.Payment.Service.Services.Interfaces.Registrations;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace EPR.Payment.Service.UnitTests.Controllers.Registrations
{
    [TestClass]
    public class RegistrationSubmissionControllerTests
    {
        private Mock<IRegistrationSubmissionService> _serviceMock = null!;
        private Mock<ILogger<RegistrationSubmissionController>> _loggerMock = null!;
        private RegistrationSubmissionController _controller = null!;

        [TestInitialize]
        public void TestInitialize()
        {
            _serviceMock = new Mock<IRegistrationSubmissionService>();
            _loggerMock = new Mock<ILogger<RegistrationSubmissionController>>();
            _controller = new RegistrationSubmissionController(_serviceMock.Object, _loggerMock.Object);
        }

        [TestMethod]
        public void Constructor_WhenServiceIsNull_ShouldThrowArgumentNullException()
        {
            // Act
            Action act = () => new RegistrationSubmissionController(null!, _loggerMock.Object);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Value cannot be null. (Parameter 'registrationSubmissionService')");
        }

        [TestMethod]
        public void Constructor_WhenLoggerIsNull_ShouldThrowArgumentNullException()
        {
            // Act
            Action act = () => new RegistrationSubmissionController(_serviceMock.Object, null!);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Value cannot be null. (Parameter 'logger')");
        }

        [TestMethod, AutoMoqData]
        public async Task SubmissionExistsAsync_SubmissionExists_ShouldReturnNoContent(
            [Frozen] Mock<IRegistrationSubmissionService> serviceMock,
            [Greedy] RegistrationSubmissionController controllerUnderTest,
            Guid submissionId,
            CancellationTokenSource cancellationTokenSource)
        {
            // Arrange
            serviceMock
                .Setup(s => s.SubmissionExistsAsync(submissionId, cancellationTokenSource.Token))
                .ReturnsAsync(true);

            // Act
            IActionResult result = await controllerUnderTest.SubmissionExistsAsync(submissionId, cancellationTokenSource.Token);

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeOfType<NoContentResult>();

                serviceMock.Verify(s => s.SubmissionExistsAsync(submissionId, cancellationTokenSource.Token), Times.Once);
            }
        }

        [TestMethod, AutoMoqData]
        public async Task SubmissionExistsAsync_SubmissionDoesNotExist_ShouldReturnNotFound(
            [Frozen] Mock<IRegistrationSubmissionService> serviceMock,
            [Greedy] RegistrationSubmissionController controllerUnderTest,
            Guid submissionId,
            CancellationTokenSource cancellationTokenSource)
        {
            // Arrange
            serviceMock
                .Setup(s => s.SubmissionExistsAsync(submissionId, cancellationTokenSource.Token))
                .ReturnsAsync(false);

            // Act
            IActionResult result = await controllerUnderTest.SubmissionExistsAsync(submissionId, cancellationTokenSource.Token);

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeOfType<NotFoundResult>();

                serviceMock.Verify(s => s.SubmissionExistsAsync(submissionId, cancellationTokenSource.Token), Times.Once);
            }
        }

        [TestMethod, AutoMoqData]
        public async Task SubmissionExistsAsync_ServiceThrowsException_ShouldReturnInternalServerError(
            [Frozen] Mock<IRegistrationSubmissionService> serviceMock,
            [Greedy] RegistrationSubmissionController controllerUnderTest,
            Guid submissionId,
            CancellationTokenSource cancellationTokenSource)
        {
            // Arrange
            serviceMock
                .Setup(s => s.SubmissionExistsAsync(submissionId, cancellationTokenSource.Token))
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            IActionResult result = await controllerUnderTest.SubmissionExistsAsync(submissionId, cancellationTokenSource.Token);

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeOfType<ObjectResult>()
                    .Which.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);

                serviceMock.Verify(s => s.SubmissionExistsAsync(submissionId, cancellationTokenSource.Token), Times.Once);
            }
        }
    }
}
