using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.MSTest;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.RegistrationFees;
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
    public class DefaultResubmissionAmountStrategyTests
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
            Action act = () => new DefaultResubmissionAmountStrategy(nullRepository!);

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
            var strategy = new DefaultResubmissionAmountStrategy(feesRepositoryMock.Object);

            // Assert
            using (new AssertionScope())
            {
                strategy.Should().NotBeNull();
                strategy.Should().BeAssignableTo<IResubmissionAmountStrategy>();
            }
        }

        [TestMethod, AutoMoqData]
        public async Task GetResubmissionAsync_RepositoryReturnsAResult_ShouldReturnAmount(
            [Frozen] Mock<IProducerFeesRepository> feesRepositoryMock,
            DefaultResubmissionAmountStrategy strategy,
            [Frozen] decimal expectedAmount
            )
        {
            //Arrange
            string regulator = "GB-ENG";
            var regulatorType = RegulatorType.Create(regulator);

            feesRepositoryMock.Setup(i => i.GetResubmissionAsync(regulatorType, CancellationToken.None)).ReturnsAsync(expectedAmount);

            //Act
            var result = await strategy.GetResubmissionAsync(regulator, CancellationToken.None);

            //Assert
            result.Should().Be(expectedAmount);
        }

        [TestMethod, AutoMoqData]
        public async Task GetResubmissionAsync_RepositoryReturnsAResult_ShouldReturnNull(
            [Frozen] Mock<IProducerFeesRepository> feesRepositoryMock,
            DefaultResubmissionAmountStrategy strategy
            )
        {
            //Arrange
            string regulator = "GB-ENG";
            var regulatorType = RegulatorType.Create(regulator);

            feesRepositoryMock.Setup(i => i.GetResubmissionAsync(regulatorType, CancellationToken.None)).ReturnsAsync((decimal?)null);

            //Act
            var result = await strategy.GetResubmissionAsync(regulator, CancellationToken.None);

            //Assert
            result.Should().BeNull();
        }

        [TestMethod, AutoMoqData]
        public async Task GetResubmissionAsync_EmptyRegulator_ThrowsArgumentException(DefaultResubmissionAmountStrategy strategy)
        {
            // Act & Assert
            await strategy.Invoking(async s => await s!.GetResubmissionAsync(string.Empty, new CancellationToken()))
                .Should().ThrowAsync<ArgumentException>()
                .WithMessage("regulator cannot be null or empty (Parameter 'regulator')");
        }

        [TestMethod, AutoMoqData]
        public async Task GetResubmissionAsync_NullRegulator_ThrowsArgumentException(DefaultResubmissionAmountStrategy strategy)
        {
            // Act & Assert
            await strategy.Invoking(async s => await s!.GetResubmissionAsync(null!, new CancellationToken()))
                .Should().ThrowAsync<ArgumentException>()
                .WithMessage("regulator cannot be null or empty (Parameter 'regulator')");
        }
    }
}
