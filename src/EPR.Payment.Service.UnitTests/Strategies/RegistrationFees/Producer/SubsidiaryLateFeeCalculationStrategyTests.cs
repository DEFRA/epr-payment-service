using AutoFixture;
using AutoFixture.AutoMoq;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.RegistrationFees;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.Producer;
using EPR.Payment.Service.Common.UnitTests.TestHelpers;
using EPR.Payment.Service.Common.ValueObjects.RegistrationFees;
using EPR.Payment.Service.Strategies.Interfaces.Common;
using EPR.Payment.Service.Strategies.RegistrationFees.Producer;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;

namespace EPR.Payment.Service.UnitTests.Strategies.RegistrationFees.Producer
{
    [TestClass]
    public class SubsidiaryLateFeeCalculationStrategyTests
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
            Func<SubsidiaryLateFeeCalculationStrategy> act = () => new SubsidiaryLateFeeCalculationStrategy(nullRepository!);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Value cannot be null. (Parameter 'feesRepository')");
        }

        [TestMethod]
        public void Constructor_WhenFeesRepositoryIsNotNull_ShouldInitializeSubsidiaryLateFeeCalculationStrategy()
        {
            // Arrange
            var feesRepositoryMock = _fixture.Create<Mock<IProducerFeesRepository>>();

            // Act
            var strategy = new SubsidiaryLateFeeCalculationStrategy(feesRepositoryMock.Object);

            // Assert
            using (new AssertionScope())
            {
                strategy.Should().NotBeNull();
                strategy.Should().BeAssignableTo<IFeeCalculationStrategy<ProducerRegistrationFeesRequestDto, decimal>>();
            }
        }

        [TestMethod, AutoMoqData]
        public async Task CalculateFeeAsync_WhenNumberOfLateSubsidiariesIsPositiveWithValidRegulator_ReturnsSubsidiaryLateFee(
            [Frozen] Mock<IProducerFeesRepository> feesRepositoryMock,
            SubsidiaryLateFeeCalculationStrategy strategy)
        {
            // Arrange
            var request = new ProducerRegistrationFeesRequestDto
            {
                ProducerType = "Large",
                NumberOfLateSubsidiaries = 2,
                Regulator = "GB-ENG",
                ApplicationReferenceNumber = "A123",
                SubmissionDate = DateTime.UtcNow,
            };

            var regulator = RegulatorType.Create("GB-ENG");

            feesRepositoryMock.Setup(repo => repo.GetSubsidiaryLateFeeAsync(regulator, request.SubmissionDate, It.IsAny<CancellationToken>()))
                .ReturnsAsync(77200m);

            // Act
            var result = await strategy.CalculateFeeAsync(request, CancellationToken.None);

            // Assert
            result.Should().Be(77200m);
        }

        [TestMethod, AutoMoqData]
        public async Task CalculateFeeAsync_WhenNumberOfLateSubsidiariesIsZero_ReturnsZeroFee(
            SubsidiaryLateFeeCalculationStrategy strategy)
        {
            // Arrange
            var request = new ProducerRegistrationFeesRequestDto
            {
                ProducerType = "Large",
                NumberOfLateSubsidiaries = 0,
                Regulator = "GB-ENG",
                ApplicationReferenceNumber = "A123",
                SubmissionDate = DateTime.UtcNow,
            };

            // Act
            var result = await strategy.CalculateFeeAsync(request, CancellationToken.None);

            // Assert
            result.Should().Be(0m);
        }

        [TestMethod, AutoMoqData]
        public async Task CalculateFeeAsync_WhenNumberOfLateSubsidiariesIsNegative_ReturnsZeroFee(
            SubsidiaryLateFeeCalculationStrategy strategy)
        {
            // Arrange - defensive: the caller (ComplianceSchemeCalculatorService/ProducerFeesCalculatorService)
            // is only ever expected to pass a non-negative count, but the strategy's own "<= 0" guard should
            // still hold if one ever slipped through.
            var request = new ProducerRegistrationFeesRequestDto
            {
                ProducerType = "Large",
                NumberOfLateSubsidiaries = -1,
                Regulator = "GB-ENG",
                ApplicationReferenceNumber = "A123",
                SubmissionDate = DateTime.UtcNow,
            };

            // Act
            var result = await strategy.CalculateFeeAsync(request, CancellationToken.None);

            // Assert
            result.Should().Be(0m);
        }

        [TestMethod, AutoMoqData]
        public async Task CalculateFeeAsync_WhenNumberOfLateSubsidiariesIsPositiveAndRegulatorIsNull_ThrowsArgumentException(
            SubsidiaryLateFeeCalculationStrategy strategy)
        {
            // Arrange
            var request = new ProducerRegistrationFeesRequestDto
            {
                ProducerType = "Large",
                NumberOfLateSubsidiaries = 1,
                Regulator = null!, // Regulator is null
                ApplicationReferenceNumber = "A123",
                SubmissionDate = DateTime.UtcNow,
            };

            // Act & Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(() => strategy.CalculateFeeAsync(request, CancellationToken.None));
        }

        [TestMethod, AutoMoqData]
        public async Task CalculateFeeAsync_WhenNumberOfLateSubsidiariesIsPositiveAndRegulatorIsEmpty_ThrowsArgumentException(
            SubsidiaryLateFeeCalculationStrategy strategy)
        {
            // Arrange
            var request = new ProducerRegistrationFeesRequestDto
            {
                ProducerType = "Large",
                NumberOfLateSubsidiaries = 1,
                Regulator = string.Empty, // Regulator is empty
                ApplicationReferenceNumber = "A123",
                SubmissionDate = DateTime.UtcNow,
            };

            // Act & Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(() => strategy.CalculateFeeAsync(request, CancellationToken.None));
        }
    }
}
