using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.MSTest;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.RegistrationFees;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.Producer;
using EPR.Payment.Service.Common.UnitTests.TestHelpers;
using EPR.Payment.Service.Common.ValueObjects.RegistrationFees;
using EPR.Payment.Service.Strategies.Interfaces.RegistrationFees;
using EPR.Payment.Service.Strategies.RegistrationFees.Producer;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;

namespace EPR.Payment.Service.UnitTests.Strategies.RegistrationFees.Producer
{
    [TestClass]
    public class LateFeeCalculationStrategyTests
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
            Func<LateFeeCalculationStrategy> act = () => new LateFeeCalculationStrategy(nullRepository!);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Value cannot be null. (Parameter 'feesRepository')");
        }

        [TestMethod]
        public void Constructor_WhenFeesRepositoryIsNotNull_ShouldInitializeLateFeeCalculationStrategy()
        {
            // Arrange
            var feesRepositoryMock = _fixture.Create<Mock<IProducerFeesRepository>>();

            // Act
            var strategy = new LateFeeCalculationStrategy(feesRepositoryMock.Object);

            // Assert
            using (new AssertionScope())
            {
                strategy.Should().NotBeNull();
                strategy.Should().BeAssignableTo<IFeeCalculationStrategy<ProducerRegistrationFeesRequestDto, decimal>>();
            }
        }

        [TestMethod, AutoMoqData]
        public async Task CalculateFeeAsync_WhenLateFeeIsTrueWithValidRegulator_ReturnLateFee(
            [Frozen] Mock<IProducerFeesRepository> feesRepositoryMock,
            LateFeeCalculationStrategy strategy)
        {
            // Arrange
            var request = new ProducerRegistrationFeesRequestDto
            {
                ProducerType = "Large",
                IsLateFeeApplicable = true,
                Regulator = "GB-ENG",
                ApplicationReferenceNumber = "A123",
                SubmissionDate = DateTime.Now
            };

            var regulator = RegulatorType.Create("GB-ENG");

            feesRepositoryMock.Setup(repo => repo.GetLateFeeAsync(regulator, request.SubmissionDate, It.IsAny<CancellationToken>()))
                .ReturnsAsync(33200m);

            // Act
            var result = await strategy.CalculateFeeAsync(request, CancellationToken.None);

            // Assert
            result.Should().Be(33200m);
        }

        [TestMethod, AutoMoqData]
        public async Task CalculateFeeAsync_WhenLateFeeIsFalse_ReturnsZeroFee(
            [Frozen] Mock<IProducerFeesRepository> feesRepositoryMock,
            LateFeeCalculationStrategy strategy)
        {
            // Arrange
            var request = new ProducerRegistrationFeesRequestDto
            {
                ProducerType = "Large",
                IsLateFeeApplicable = false,
                Regulator = "GB-ENG",
                ApplicationReferenceNumber = "A123",
                SubmissionDate = DateTime.Now
            };

            // Act
            var result = await strategy.CalculateFeeAsync(request, CancellationToken.None);

            // Assert
            result.Should().Be(0m);
        }

        [TestMethod, AutoMoqData]
        public async Task CalculateFeeAsync_WhenLateFeeIsTrueRegulatorIsNull_ThrowsArgumentException(
            LateFeeCalculationStrategy strategy)
        {
            // Arrange
            var request = new ProducerRegistrationFeesRequestDto
            {
                ProducerType = "Large",
                IsLateFeeApplicable = true,
                Regulator = null!, // Regulator is null
                ApplicationReferenceNumber = "A123",
                SubmissionDate = DateTime.Now
            };

            // Act & Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(() => strategy.CalculateFeeAsync(request, CancellationToken.None));
        }

        [TestMethod, AutoMoqData]
        public async Task CalculateFeeAsync_WhenLateFeeIsTrueRegulatorIsEmpty_ThrowsArgumentException(
            LateFeeCalculationStrategy strategy)
        {
            // Arrange
            var request = new ProducerRegistrationFeesRequestDto
            {
                ProducerType = "Large",
                IsLateFeeApplicable = true,
                Regulator = string.Empty, // Regulator is empty
                ApplicationReferenceNumber = "A123",
                SubmissionDate = DateTime.Now
            };

            // Act & Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(() => strategy.CalculateFeeAsync(request, CancellationToken.None));
        }
    }
}
