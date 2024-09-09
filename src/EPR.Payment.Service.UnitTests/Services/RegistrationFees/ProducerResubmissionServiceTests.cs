﻿using AutoFixture.MSTest;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees;
using EPR.Payment.Service.Common.UnitTests.TestHelpers;
using EPR.Payment.Service.Services.Interfaces.RegistrationFees;
using EPR.Payment.Service.Services.RegistrationFees;
using EPR.Payment.Service.Strategies.Interfaces.RegistrationFees;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;

namespace EPR.Payment.Service.UnitTests.Services.RegistrationFees
{
    [TestClass]
    public class ProducerResubmissionServiceTests
    {
        private Mock<IResubmissionAmountStrategy> _resubmissionAmountStrategyMock = null!;
        private ProducerResubmissionService? _resubmissionService = null;

        [TestInitialize]
        public void TestInitialize()
        {
            _resubmissionAmountStrategyMock = new Mock<IResubmissionAmountStrategy>();

            _resubmissionService = new ProducerResubmissionService(
                _resubmissionAmountStrategyMock.Object
            );
        }

        [TestMethod]
        public void Constructor_WhenDependencyINotNull_ShouldInitializeProducerResubmissionService()
        {
            // Act
            var service = new ProducerResubmissionService(
                _resubmissionAmountStrategyMock.Object);

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
                resubmissionAmountStrategy!);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'resubmissionAmountStrategy')");
        }

        [TestMethod, AutoMoqData]
        public async Task GetResubmissionAsync_RepositoryReturnsAResult_ShouldReturnAmount(
            [Frozen] string regulator,
            [Frozen] decimal expectedAmount
            )
        {
            //Arrange
            _resubmissionAmountStrategyMock.Setup(i => i.GetResubmissionAsync(regulator, CancellationToken.None)).ReturnsAsync(expectedAmount);

            //Act
            var result = await _resubmissionService!.GetResubmissionAsync(regulator, CancellationToken.None);

            //Assert
            result.Should().Be(expectedAmount);
        }

        [TestMethod, AutoMoqData]
        public async Task GetResubmissionAsync_RepositoryReturnsAResult_ShouldReturnNull(
            [Frozen] string regulator
            )
        {
            //Arrange
            _resubmissionAmountStrategyMock.Setup(i => i.GetResubmissionAsync(regulator, CancellationToken.None)).ReturnsAsync((decimal?)null);

            //Act
            var result = await _resubmissionService!.GetResubmissionAsync(regulator, CancellationToken.None);

            //Assert
            result.Should().BeNull();
        }

        [TestMethod]
        public async Task GetResubmissionAsync_EmptyRegulator_ThrowsArgumentException()
        {
            // Act & Assert
            await _resubmissionService.Invoking(async s => await s!.GetResubmissionAsync(string.Empty, new CancellationToken()))
                .Should().ThrowAsync<ArgumentException>()
                .WithMessage("regulator cannot be null or empty (Parameter 'regulator')");
        }

        [TestMethod]
        public async Task GetResubmissionAsync_NullRegulator_ThrowsArgumentException()
        {
            // Act & Assert
            await _resubmissionService.Invoking(async s => await s!.GetResubmissionAsync(null!, new CancellationToken()))
                .Should().ThrowAsync<ArgumentException>()
                .WithMessage("regulator cannot be null or empty (Parameter 'regulator')");
        }
    }
}