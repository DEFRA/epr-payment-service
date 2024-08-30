﻿using AutoFixture.MSTest;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.RegistrationFees;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees;
using EPR.Payment.Service.Common.UnitTests.TestHelpers;
using EPR.Payment.Service.Common.ValueObjects.RegistrationFees;
using EPR.Payment.Service.Strategies.Interfaces.RegistrationFees;
using EPR.Payment.Service.Strategies.RegistrationFees.Producer;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;

namespace EPR.Payment.Service.UnitTests.Strategies.RegistrationFees
{
    [TestClass]
    public class SubsidiariesFeeCalculationStrategyTests
    {
        [TestMethod]
        [AutoMoqData]
        public void Constructor_ShouldThrowArgumentNullException_WhenFeesRepositoryIsNull()
        {
            // Arrange
            IProducerFeesRepository? nullRepository = null;

            // Act
            Action act = () => new SubsidiariesFeeCalculationStrategy(nullRepository!);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Value cannot be null. (Parameter 'feesRepository')");
        }

        [TestMethod]
        [AutoMoqData]
        public void Constructor_ShouldInitializeSubsidiariesFeeCalculationStrategy_WhenFeesRepositoryIsNotNull()
        {
            // Arrange
            var feesRepositoryMock = new Mock<IProducerFeesRepository>();

            // Act
            var strategy = new SubsidiariesFeeCalculationStrategy(feesRepositoryMock.Object);

            // Assert
            using (new AssertionScope())
            {
                strategy.Should().NotBeNull();
                strategy.Should().BeAssignableTo<IFeeCalculationStrategy<ProducerRegistrationFeesRequestDto>>();
            }
        }

        [TestMethod]
        [AutoMoqData]
        public async Task CalculateFeeAsync_LargeProducerWith50Subsidiaries_ReturnsCorrectSubsidiariesFee(
            [Frozen] Mock<IProducerFeesRepository> feesRepositoryMock,
            SubsidiariesFeeCalculationStrategy strategy)
        {
            // Arrange
            var request = new ProducerRegistrationFeesRequestDto
            {
                NumberOfSubsidiaries = 50,
                Regulator = "GB-ENG"
            };

            var regulator = RegulatorType.Create(request.Regulator);

            feesRepositoryMock.Setup(repo => repo.GetFirst20SubsidiariesFeeAsync(regulator, It.IsAny<CancellationToken>()))
                .ReturnsAsync(55800m); // £558 in pence per subsidiary

            feesRepositoryMock.Setup(repo => repo.GetAdditionalSubsidiariesFeeAsync(regulator, It.IsAny<CancellationToken>()))
                .ReturnsAsync(14000m); // £140 in pence per additional subsidiary

            // Act
            var result = await strategy.CalculateFeeAsync(request, CancellationToken.None);

            // Assert
            result.Should().Be(1536000m); // £15,360 in pence
        }

        [TestMethod]
        [AutoMoqData]
        public async Task CalculateFeeAsync_SubsidiariesCountIsZero_ThrowsArgumentException(
            [Frozen] Mock<IProducerFeesRepository> feesRepositoryMock,
            SubsidiariesFeeCalculationStrategy strategy)
        {
            // Arrange
            var request = new ProducerRegistrationFeesRequestDto
            {
                NumberOfSubsidiaries = 0,
                Regulator = "GB-ENG"
            };

            // Act
            var result = await strategy.CalculateFeeAsync(request, CancellationToken.None);

            // Assert
            result.Should().Be(0m);
        }

        [TestMethod]
        [AutoMoqData]
        public async Task CalculateFeeAsync_SubsidiariesCountIsNegative_ThrowsArgumentException(
            [Frozen] Mock<IProducerFeesRepository> feesRepositoryMock,
            SubsidiariesFeeCalculationStrategy strategy)
        {
            // Arrange
            var request = new ProducerRegistrationFeesRequestDto
            {
                NumberOfSubsidiaries = -5,
                Regulator = "GB-ENG"
            };

            // Act & Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(() => strategy.CalculateFeeAsync(request, CancellationToken.None));
        }

        [TestMethod]
        [AutoMoqData]
        public async Task CalculateFeeAsync_RegulatorIsNullOrEmpty_ThrowsArgumentException(
            [Frozen] Mock<IProducerFeesRepository> feesRepositoryMock,
            SubsidiariesFeeCalculationStrategy strategy)
        {
            // Arrange
            var request = new ProducerRegistrationFeesRequestDto
            {
                NumberOfSubsidiaries = 10,
                Regulator = null!
            };

            // Act & Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(() => strategy.CalculateFeeAsync(request, CancellationToken.None));
        }

        [TestMethod]
        [AutoMoqData]
        public async Task CalculateFeeAsync_SmallProducerWith10Subsidiaries_ReturnsCorrectFee(
            [Frozen] Mock<IProducerFeesRepository> feesRepositoryMock,
            SubsidiariesFeeCalculationStrategy strategy)
        {
            // Arrange
            var request = new ProducerRegistrationFeesRequestDto
            {
                NumberOfSubsidiaries = 10,
                Regulator = "GB-ENG"
            };

            var regulator = RegulatorType.Create(request.Regulator);

            feesRepositoryMock.Setup(repo => repo.GetFirst20SubsidiariesFeeAsync(regulator, It.IsAny<CancellationToken>()))
                .ReturnsAsync(55800m); // £558 in pence per subsidiary

            // Act
            var result = await strategy.CalculateFeeAsync(request, CancellationToken.None);

            // Assert
            result.Should().Be(558000m); // 10 subsidiaries at £558 each
        }
    }
}