using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.MSTest;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.RegistrationFees;
using EPR.Payment.Service.Common.Dtos.Request.ResubmissionFees.ComplianceScheme;
using EPR.Payment.Service.Common.UnitTests.TestHelpers;
using EPR.Payment.Service.Common.ValueObjects.RegistrationFees;
using EPR.Payment.Service.Strategies.Interfaces.ResubmissionFees.ComplianceScheme;
using EPR.Payment.Service.Strategies.ResubmissionFees.ComplianceScheme;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;

namespace EPR.Payment.Service.UnitTests.Strategies.ResubmissionFees.ComplianceScheme
{
    [TestClass]
    public class ComplianceSchemeResubmissionFeeCalculationStrategyTests
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
            IComplianceSchemeFeesRepository? nullRepository = null;

            // Act
            Action act = () => new ComplianceSchemeResubmissionFeeCalculationStrategy(nullRepository!);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Value cannot be null. (Parameter 'feesRepository')");
        }

        [TestMethod]
        public void Constructor_WhenFeesRepositoryIsNotNull_ShouldInitializeStrategy()
        {
            // Arrange
            var feesRepositoryMock = _fixture.Create<Mock<IComplianceSchemeFeesRepository>>();

            // Act
            var strategy = new ComplianceSchemeResubmissionFeeCalculationStrategy(feesRepositoryMock.Object);

            // Assert
            using (new AssertionScope())
            {
                strategy.Should().NotBeNull();
                strategy.Should().BeAssignableTo<IComplianceSchemeResubmissionFeeCalculationStrategy<ComplianceSchemeResubmissionFeeRequestDto, decimal>>();
            }
        }

        [TestMethod, AutoMoqData]
        public async Task CalculateFeeAsync_RepositoryReturnsAResult_ShouldReturnAmount(
            [Frozen] Mock<IComplianceSchemeFeesRepository> feesRepositoryMock,
            ComplianceSchemeResubmissionFeeCalculationStrategy strategy,
            [Frozen] decimal expectedAmount)
        {
            // Arrange
            var request = new ComplianceSchemeResubmissionFeeRequestDto
            {
                Regulator = "GB-ENG",
                ReferenceNumber = "12345",
                ResubmissionDate = DateTime.UtcNow,
                MemberCount = 1
            };
            var regulatorType = RegulatorType.Create(request.Regulator);

            feesRepositoryMock.Setup(i => i.GetResubmissionFeeAsync(regulatorType, CancellationToken.None)).ReturnsAsync(expectedAmount);

            // Act
            var result = await strategy.CalculateFeeAsync(request, CancellationToken.None);

            // Assert
            result.Should().Be(expectedAmount);
        }

        [TestMethod, AutoMoqData]
        public async Task CalculateFeeAsync_EmptyRegulator_ThrowsArgumentException(
            ComplianceSchemeResubmissionFeeCalculationStrategy strategy)
        {
            // Act & Assert
            var request = new ComplianceSchemeResubmissionFeeRequestDto
            {
                Regulator = string.Empty,
                ReferenceNumber = "12345", // Add required reference number
                ResubmissionDate = DateTime.UtcNow,
                MemberCount = 1
            };
            await strategy.Invoking(async s => await s.CalculateFeeAsync(request, new CancellationToken()))
                .Should().ThrowAsync<ArgumentException>()
                .WithMessage("Regulator cannot be null or empty");
        }

        [TestMethod, AutoMoqData]
        public async Task CalculateFeeAsync_NullRegulator_ThrowsArgumentException(
            ComplianceSchemeResubmissionFeeCalculationStrategy strategy)
        {
            // Act & Assert
            var request = new ComplianceSchemeResubmissionFeeRequestDto
            {
                Regulator = null!,
                ReferenceNumber = "12345", // Add required reference number
                ResubmissionDate = DateTime.UtcNow,
                MemberCount = 1
            };
            await strategy.Invoking(async s => await s.CalculateFeeAsync(request, new CancellationToken()))
                .Should().ThrowAsync<ArgumentException>()
                .WithMessage("Regulator cannot be null or empty");
        }

        [TestMethod, AutoMoqData]
        public async Task CalculateFeeAsync_ZeroFee_ThrowsKeyNotFoundException(
            [Frozen] Mock<IComplianceSchemeFeesRepository> feesRepositoryMock,
            ComplianceSchemeResubmissionFeeCalculationStrategy strategy)
        {
            // Arrange
            var request = new ComplianceSchemeResubmissionFeeRequestDto
            {
                Regulator = "GB-ENG",
                ReferenceNumber = "12345", // Add required reference number
                ResubmissionDate = DateTime.UtcNow,
                MemberCount = 1
            };
            var regulatorType = RegulatorType.Create(request.Regulator);

            // Set up the repository mock to return 0 fee
            feesRepositoryMock.Setup(i => i.GetResubmissionFeeAsync(regulatorType, CancellationToken.None)).ReturnsAsync(0m);

            // Act & Assert
            await strategy.Invoking(async s => await s.CalculateFeeAsync(request, new CancellationToken()))
                .Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage($"No resubmission fee found for regulator '{request.Regulator}'.");
        }
    }
}