using AutoFixture.MSTest;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.Producer;
using EPR.Payment.Service.Common.UnitTests.TestHelpers;
using EPR.Payment.Service.Services.Interfaces.RegistrationFees.Producer;
using EPR.Payment.Service.Services.RegistrationFees.Producer;
using EPR.Payment.Service.Strategies.Interfaces.RegistrationFees.Producer;
using FluentAssertions;
using FluentAssertions.Execution;
using FluentValidation;
using FluentValidation.Results;
using Moq;

namespace EPR.Payment.Service.UnitTests.Services.RegistrationFees
{
    [TestClass]
    public class ProducerResubmissionServiceTests
    {
        private Mock<IResubmissionAmountStrategy> _resubmissionAmountStrategyMock = null!;
        private ProducerResubmissionService? _resubmissionService = null;
        private Mock<IValidator<RegulatorDto>> _producerResubmissionFeeRequestDtoMock = null!;

        [TestInitialize]
        public void TestInitialize()
        {
            _resubmissionAmountStrategyMock = new Mock<IResubmissionAmountStrategy>();
            _producerResubmissionFeeRequestDtoMock = new Mock<IValidator<RegulatorDto>>();

            _resubmissionService = new ProducerResubmissionService(
                _resubmissionAmountStrategyMock.Object,
                _producerResubmissionFeeRequestDtoMock.Object
            );
        }

        [TestMethod]
        public void Constructor_WhenAllDependenciesAreNotNull_ShouldInitializeProducerResubmissionService()
        {
            // Act
            var service = new ProducerResubmissionService(
                _resubmissionAmountStrategyMock.Object,
                _producerResubmissionFeeRequestDtoMock.Object);

            // Assert
            using (new AssertionScope())
            {
                service.Should().NotBeNull();
                service.Should().BeAssignableTo<IProducerResubmissionService>();
            }
        }

        [TestMethod]
        public void Constructor_WhenResubmissionAmountStrategyIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            IResubmissionAmountStrategy? resubmissionAmountStrategy = null;

            // Act
            Action act = () => new ProducerResubmissionService(
                resubmissionAmountStrategy!,
                _producerResubmissionFeeRequestDtoMock.Object);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'resubmissionAmountStrategy')");
        }

        [TestMethod]
        public void Constructor_WhenProducerResubmissionFeeRequestDtoValidatorIsNull_ShouldThrowArgumentNullException()
        {
            // Act
            Action act = () => new ProducerResubmissionService(
                _resubmissionAmountStrategyMock.Object,
                null!);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'producerResubmissionRequestValidator')");
        }

        [TestMethod, AutoMoqData]
        public async Task GetResubmissionAsync_RepositoryReturnsAResult_ShouldReturnAmount(
            [Frozen] RegulatorDto request,
            [Frozen] decimal expectedAmount
            )
        {
            //Arrange
            _producerResubmissionFeeRequestDtoMock.Setup(v => v.ValidateAsync(request, default)).ReturnsAsync(new ValidationResult());

            _resubmissionAmountStrategyMock.Setup(i => i.CalculateFeeAsync(request, CancellationToken.None)).ReturnsAsync(expectedAmount);

            //Act
            var result = await _resubmissionService!.GetResubmissionAsync(request, CancellationToken.None);

            //Assert
            result.Should().Be(expectedAmount);
        }

        [TestMethod, AutoMoqData]
        public async Task GetResubmissionAsync_ValiditonFails_ShouldThrowValidationException([Frozen] RegulatorDto request)
        {
            // Arrange

            var validationFailures = new List<ValidationFailure>
            {
                new ValidationFailure(nameof(request.Regulator), "Regulator is required.")
            };

            _producerResubmissionFeeRequestDtoMock.Setup(v => v.ValidateAsync(request, default)).ReturnsAsync(new ValidationResult(validationFailures));

            // Act & Assert
            await _resubmissionService.Invoking(async x => await x!.GetResubmissionAsync(request, CancellationToken.None))
                .Should().ThrowAsync<ValidationException>();
        }
    }
}
