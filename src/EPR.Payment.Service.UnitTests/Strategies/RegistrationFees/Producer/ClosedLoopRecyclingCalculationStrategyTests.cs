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
    public class ClosedLoopRecyclingCalculationStrategyTests
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
            IProducerFeesRepository? nullRepository = null;

            Assert.ThrowsException<ArgumentNullException>(() => new ClosedLoopRecyclingCalculationStrategy(nullRepository!));
        }

        [TestMethod]
        public void Constructor_WhenFeesRepositoryIsNotNull_ShouldInitializeStrategy()
        {
            var feesRepositoryMock = _fixture.Create<Mock<IProducerFeesRepository>>();

            var strategy = new ClosedLoopRecyclingCalculationStrategy(feesRepositoryMock.Object);

            using (new AssertionScope())
            {
                strategy.Should().NotBeNull();
                strategy.Should().BeAssignableTo<IFeeCalculationStrategy<ProducerRegistrationFeesRequestDto, decimal>>();
            }
        }

        [TestMethod, AutoMoqData]
        public async Task CalculateFeeAsync_WhenClosedLoopRecyclingTrueAndValidRegulator_ReturnsClosedLoopRecyclingFee(
            [Frozen] Mock<IProducerFeesRepository> feesRepositoryMock,
            ClosedLoopRecyclingCalculationStrategy strategy)
        {
            var request = new ProducerRegistrationFeesRequestDto
            {
                ProducerType = "Large",
                IsClosedLoopRecycling = true,
                Regulator = "GB-ENG",
                ApplicationReferenceNumber = "A123",
                SubmissionDate = DateTime.UtcNow
            };

            var regulator = RegulatorType.Create("GB-ENG");

            feesRepositoryMock.Setup(repo => repo.GetClosedLoopRecyclingFeeAsync(regulator, request.SubmissionDate, It.IsAny<CancellationToken>()))
                .ReturnsAsync(254800m);

            var result = await strategy.CalculateFeeAsync(request, CancellationToken.None);

            result.Should().Be(254800m);
        }

        [TestMethod, AutoMoqData]
        public async Task CalculateFeeAsync_WhenClosedLoopRecyclingFalse_ReturnsZeroFee(
            [Frozen] Mock<IProducerFeesRepository> feesRepositoryMock,
            ClosedLoopRecyclingCalculationStrategy strategy)
        {
            var request = new ProducerRegistrationFeesRequestDto
            {
                ProducerType = "Large",
                IsClosedLoopRecycling = false,
                Regulator = "GB-ENG",
                ApplicationReferenceNumber = "A123",
                SubmissionDate = DateTime.UtcNow
            };

            var result = await strategy.CalculateFeeAsync(request, CancellationToken.None);

            result.Should().Be(0m);
            feesRepositoryMock.Verify(r => r.GetClosedLoopRecyclingFeeAsync(It.IsAny<RegulatorType>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [TestMethod, AutoMoqData]
        public async Task CalculateFeeAsync_WhenClosedLoopRecyclingTrueAndRegulatorIsNull_ThrowsArgumentException(
            ClosedLoopRecyclingCalculationStrategy strategy)
        {
            var request = new ProducerRegistrationFeesRequestDto
            {
                ProducerType = "Large",
                IsClosedLoopRecycling = true,
                Regulator = null!,
                ApplicationReferenceNumber = "A123",
                SubmissionDate = DateTime.UtcNow
            };

            await Assert.ThrowsExceptionAsync<ArgumentException>(() => strategy.CalculateFeeAsync(request, CancellationToken.None));
        }

        [TestMethod, AutoMoqData]
        public async Task CalculateFeeAsync_WhenClosedLoopRecyclingTrueAndRegulatorIsEmpty_ThrowsArgumentException(
            ClosedLoopRecyclingCalculationStrategy strategy)
        {
            var request = new ProducerRegistrationFeesRequestDto
            {
                ProducerType = "Large",
                IsClosedLoopRecycling = true,
                Regulator = string.Empty,
                ApplicationReferenceNumber = "A123",
                SubmissionDate = DateTime.UtcNow
            };

            await Assert.ThrowsExceptionAsync<ArgumentException>(() => strategy.CalculateFeeAsync(request, CancellationToken.None));
        }
    }
}
