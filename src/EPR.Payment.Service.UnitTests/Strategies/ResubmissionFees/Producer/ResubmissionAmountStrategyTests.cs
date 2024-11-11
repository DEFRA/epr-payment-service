using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.MSTest;
using EPR.Payment.Service.Common.Constants.RegistrationFees.Exceptions;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.RegistrationFees;
using EPR.Payment.Service.Common.Dtos.Request.ResubmissionFees.Producer;
using EPR.Payment.Service.Common.UnitTests.TestHelpers;
using EPR.Payment.Service.Common.ValueObjects.RegistrationFees;
using EPR.Payment.Service.Strategies.Interfaces.ResubmissionFees.Producer;
using EPR.Payment.Service.Strategies.ResubmissionFees.Producer;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;

namespace EPR.Payment.Service.UnitTests.Strategies.ResubmissionFees.Producer
{
    [TestClass]
    public class ResubmissionAmountStrategyTests
    {
        private IFixture _fixture = null!;

        [TestInitialize]
        public void TestInitialize()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
        }

        [TestMethod]
        public void Constructor_WhenFeesRepositoryIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            IProducerFeesRepository? nullRepository = null;

            // Act
            Action act = () => new ProducerResubmissionAmountStrategy(nullRepository!);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Value cannot be null. (Parameter 'feesRepository')");
        }

        [TestMethod]
        public void Constructor_WhenFeesRepositoryIsNotNull_ShouldInitializeResubmissionAmountStrategy()
        {
            // Arrange
            var feesRepositoryMock = _fixture.Create<Mock<IProducerFeesRepository>>();

            // Act
            var strategy = new ProducerResubmissionAmountStrategy(feesRepositoryMock.Object);

            // Assert
            using (new AssertionScope())
            {
                strategy.Should().NotBeNull();
                strategy.Should().BeAssignableTo<IResubmissionAmountStrategy<ProducerResubmissionFeeRequestDto, decimal>>();
            }
        }

        [TestMethod, AutoMoqData]
        public async Task CalculateFeeAsync_RepositoryReturnsAResult_ShouldReturnAmount(
            [Frozen] Mock<IProducerFeesRepository> feesRepositoryMock,
            ProducerResubmissionAmountStrategy strategy,
            [Frozen] decimal expectedAmount)
        {
            // Arrange
            var resubmissionDate = DateTime.Today;
            var producerResubmissionFeeRequestDto = new ProducerResubmissionFeeRequestDto
            {
                Regulator = "GB-ENG",
                ResubmissionDate = resubmissionDate
            };
            var regulatorType = RegulatorType.Create(producerResubmissionFeeRequestDto.Regulator);

            feesRepositoryMock.Setup(i => i.GetResubmissionAsync(regulatorType, resubmissionDate, CancellationToken.None)).ReturnsAsync(expectedAmount);

            // Act
            var result = await strategy.CalculateFeeAsync(producerResubmissionFeeRequestDto, CancellationToken.None);

            // Assert
            result.Should().Be(expectedAmount);
        }

        [TestMethod, AutoMoqData]
        public async Task CalculateFeeAsync_EmptyRegulator_ThrowsArgumentException(ProducerResubmissionAmountStrategy strategy)
        {
            // Act & Assert
            var producerResubmissionFeeRequestDto = new ProducerResubmissionFeeRequestDto { Regulator = string.Empty };
            await strategy.Invoking(async s => await s.CalculateFeeAsync(producerResubmissionFeeRequestDto, new CancellationToken()))
                .Should().ThrowAsync<ArgumentException>()
                .WithMessage("Regulator cannot be null or empty");
        }

        [TestMethod, AutoMoqData]
        public async Task CalculateFeeAsync_NullRegulator_ThrowsArgumentException(ProducerResubmissionAmountStrategy strategy)
        {
            // Act & Assert
            var producerResubmissionFeeRequestDto = new ProducerResubmissionFeeRequestDto { Regulator = null! };
            await strategy.Invoking(async s => await s.CalculateFeeAsync(producerResubmissionFeeRequestDto, new CancellationToken()))
                .Should().ThrowAsync<ArgumentException>()
                .WithMessage("Regulator cannot be null or empty");
        }

        [TestMethod, AutoMoqData]
        public async Task CalculateFeeAsync_ZeroFee_ThrowsKeyNotFoundException(
            [Frozen] Mock<IProducerFeesRepository> feesRepositoryMock,
            ProducerResubmissionAmountStrategy strategy)
        {
            // Arrange
            var resubmissionDate = DateTime.Today;
            var producerResubmissionFeeRequestDto = new ProducerResubmissionFeeRequestDto
            {
                Regulator = "GB-ENG",
                ResubmissionDate = resubmissionDate
            };
            var regulatorType = RegulatorType.Create(producerResubmissionFeeRequestDto.Regulator);

            // Set up the repository mock to return 0 fee
            feesRepositoryMock.Setup(i => i.GetResubmissionAsync(regulatorType, resubmissionDate, CancellationToken.None)).ReturnsAsync(0m);

            // Act & Assert
            await strategy.Invoking(async s => await s.CalculateFeeAsync(producerResubmissionFeeRequestDto, new CancellationToken()))
                .Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage(string.Format(ProducerFeesCalculationExceptions.InvalidRegulatorError, producerResubmissionFeeRequestDto.Regulator));
        }
    }
}