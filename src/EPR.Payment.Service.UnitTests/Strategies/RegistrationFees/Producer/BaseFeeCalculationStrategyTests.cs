using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.MSTest;
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
    public class BaseFeeCalculationStrategyTests
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

            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() => new BaseFeeCalculationStrategy(nullRepository!));
        }

        [TestMethod]
        public void Constructor_WhenFeesRepositoryIsNotNull_ShouldInitializeBaseFeeCalculationStrategy()
        {
            // Arrange
            var feesRepositoryMock = _fixture.Create<Mock<IProducerFeesRepository>>();

            // Act
            var strategy = new BaseFeeCalculationStrategy(feesRepositoryMock.Object);

            // Assert
            using (new AssertionScope())
            {
                strategy.Should().NotBeNull();
                strategy.Should().BeAssignableTo<IFeeCalculationStrategy<ProducerRegistrationFeesRequestDto, decimal>>();
            }
        }

        [TestMethod]
        [AutoMoqData]
        public async Task CalculateFeeAsync_WhenValidProducerType_ReturnsBaseFee(
            [Frozen] Mock<IProducerFeesRepository> feesRepositoryMock,
            BaseFeeCalculationStrategy strategy)
        {
            // Arrange
            var request = new ProducerRegistrationFeesRequestV2Dto
            {
                ProducerType = "Large",
                Regulator = "GB-ENG",
                ApplicationReferenceNumber = "A123",
                SubmissionDate = DateTime.UtcNow,
                FileId = Guid.NewGuid(),
                ExternalId = Guid.NewGuid(),
                InvoicePeriod = new DateTimeOffset(),
                PayerId = 1,
                PayerTypeId = 1
            };

            var regulator = RegulatorType.Create("GB-ENG");

            feesRepositoryMock.Setup(repo => repo.GetBaseFeeAsync("Large", regulator, request.SubmissionDate, It.IsAny<CancellationToken>()))
                .ReturnsAsync(262000m); // £2,620 in pence

            // Act
            var result = await strategy.CalculateFeeAsync(request, CancellationToken.None);

            // Assert
            result.Should().Be(262000m); // £2,620 in pence
        }

        [TestMethod]
        [AutoMoqData]
        public async Task CalculateFeeAsync_WhenRegulatorIsNull_ThrowsArgumentException(
            [Frozen] Mock<IProducerFeesRepository> feesRepositoryMock,
            BaseFeeCalculationStrategy strategy)
        {
            // Arrange
            var request = new ProducerRegistrationFeesRequestV2Dto
            {
                ProducerType = "Large",
                Regulator = null!, // Regulator is null
                ApplicationReferenceNumber = "A123",
                SubmissionDate = DateTime.UtcNow,
                FileId = Guid.NewGuid(),
                ExternalId = Guid.NewGuid(),
                InvoicePeriod = new DateTimeOffset(),
                PayerId = 1,
                PayerTypeId = 1
            };

            // Act & Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(() => strategy.CalculateFeeAsync(request, CancellationToken.None));
        }

        [TestMethod]
        [AutoMoqData]
        public async Task CalculateFeeAsync_WhenProducerTypeIsEmpty_ReturnsZeroBaseFee(
            [Frozen] Mock<IProducerFeesRepository> feesRepositoryMock,
            BaseFeeCalculationStrategy strategy)
        {
            // Arrange
            var request = new ProducerRegistrationFeesRequestV2Dto
            {
                ProducerType = string.Empty, // ProducerType is empty
                Regulator = "GB-ENG", // Valid Regulator
                ApplicationReferenceNumber = "A123",
                SubmissionDate = DateTime.UtcNow,
                FileId = Guid.NewGuid(),
                ExternalId = Guid.NewGuid(),
                InvoicePeriod = new DateTimeOffset(),
                PayerId = 1,
                PayerTypeId = 1
            };

            // Act
            var result = await strategy.CalculateFeeAsync(request, CancellationToken.None);

            // Assert
            result.Should().Be(0m); // Ensure that the result is zero
        }

        [TestMethod]
        [AutoMoqData]
        public async Task CalculateFeeAsync_WhenRegulatorIsEmpty_ThrowsArgumentException(
            [Frozen] Mock<IProducerFeesRepository> feesRepositoryMock,
            BaseFeeCalculationStrategy strategy)
        {
            // Arrange
            var request = new ProducerRegistrationFeesRequestV2Dto
            {
                ProducerType = "Large",
                Regulator = string.Empty, // Regulator is empty
                ApplicationReferenceNumber = "A123",
                SubmissionDate = DateTime.UtcNow,
                FileId = Guid.NewGuid(),
                ExternalId = Guid.NewGuid(),
                InvoicePeriod = new DateTimeOffset(),
                PayerId = 1,
                PayerTypeId = 1
            };

            // Act & Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(() => strategy.CalculateFeeAsync(request, CancellationToken.None));
        }
    }
}