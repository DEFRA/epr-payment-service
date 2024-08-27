using AutoFixture.MSTest;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories;
using EPR.Payment.Service.Common.UnitTests.TestHelpers;
using EPR.Payment.Service.Services;
using FluentAssertions;
using Moq;

namespace EPR.Payment.Service.UnitTests.Services
{
    [TestClass]
    public class RegistrationFeesServiceTests
    {
        private Mock<IRegistrationFeesRepository> _registrationFeesRepositoryMock = null!;
        private RegistrationFeesService _service = null!;
        private CancellationToken _cancellationToken;

        [TestInitialize]
        public void TestInitialize()
        {
            _registrationFeesRepositoryMock = new Mock<IRegistrationFeesRepository>();
            _cancellationToken = new CancellationToken();
            _service = new RegistrationFeesService(_registrationFeesRepositoryMock.Object);
        }

        [TestMethod]
        public void Constructor_WithValidArguments_ShouldInitializeCorrectly()
        {
            // Act
            var controller = new RegistrationFeesService(
                _registrationFeesRepositoryMock.Object
            );

            // Assert
            controller.Should().NotBeNull();
        }

        [TestMethod]
        public void Constructor_WithNullRegistrationFeesRepository_ShouldThrowArgumentNullException()
        {

            // Act
            Action act = () => new RegistrationFeesService(
                null!
            );

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithParameterName("registrationFeesRepository");
        }

        [TestMethod, AutoMoqData]
        public async Task GetProducerResubmissionAmountByRegulatorAsync_RepositoryReturnsAResult_ShouldReturnAmount(
            [Frozen] string regulator,
            [Frozen] decimal expectedAmount
            )
        {
            //Arrange
            _registrationFeesRepositoryMock.Setup(i => i.GetProducerResubmissionAmountByRegulatorAsync(regulator, _cancellationToken)).ReturnsAsync(expectedAmount);

            //Act
            var result = await _service.GetProducerResubmissionAmountByRegulatorAsync(regulator, _cancellationToken);

            //Assert
            result.Should().Be(expectedAmount);
        }

        [TestMethod, AutoMoqData]
        public async Task GetProducerResubmissionAmountByRegulatorAsync_RepositoryReturnsAResult_ShouldReturnNullMappedObject(
            [Frozen] string regulator
            )
        {
            //Arrange
            _registrationFeesRepositoryMock.Setup(i => i.GetProducerResubmissionAmountByRegulatorAsync(regulator, _cancellationToken)).ReturnsAsync((decimal?)null);

            //Act
            var result = await _service.GetProducerResubmissionAmountByRegulatorAsync(regulator, _cancellationToken);

            //Assert
            result.Should().BeNull();
        }

        [TestMethod]
        public async Task GetProducerResubmissionAmountByRegulatorAsync_EmptyRegulator_ThrowsArgumentException()
        {
            // Act & Assert
            await _service.Invoking(async s => await s.GetProducerResubmissionAmountByRegulatorAsync(string.Empty, new CancellationToken()))
                .Should().ThrowAsync<ArgumentException>()
                .WithMessage("regulator cannot be null or empty (Parameter 'regulator')");
        }

        [TestMethod]
        public async Task GetProducerResubmissionAmountByRegulatorAsync_NullRegulator_ThrowsArgumentException()
        {
            // Act & Assert
            await _service.Invoking(async s => await s.GetProducerResubmissionAmountByRegulatorAsync(null!, new CancellationToken()))
                .Should().ThrowAsync<ArgumentException>()
                .WithMessage("regulator cannot be null or empty (Parameter 'regulator')");
        }
    }
}
