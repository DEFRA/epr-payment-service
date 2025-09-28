using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.MSTest;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.RegistrationFees;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Common.UnitTests.TestHelpers;
using EPR.Payment.Service.Common.ValueObjects.RegistrationFees;
using EPR.Payment.Service.Strategies.Interfaces.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Strategies.RegistrationFees.ComplianceScheme;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;

namespace EPR.Payment.Service.UnitTests.Strategies.RegistrationFees.ComplianceScheme
{
    [TestClass]
    public class CSBaseFeeCalculationStrategyV3Tests
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

            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() => new CSBaseFeeCalculationStrategyV3(nullRepository!));
        }

        [TestMethod]
        public void Constructor_WhenFeesRepositoryIsNotNull_ShouldInitializeComplianceSchemeBaseFeeCalculationStrategy()
        {
            // Arrange
            var feesRepositoryMock = _fixture.Create<Mock<IComplianceSchemeFeesRepository>>();

            // Act
            var strategy = new CSBaseFeeCalculationStrategyV3(feesRepositoryMock.Object);

            // Assert
            using (new AssertionScope())
            {
                strategy.Should().NotBeNull();
                strategy.Should().BeAssignableTo<ICSBaseFeeCalculationStrategy<ComplianceSchemeFeesRequestV3Dto, decimal>>();
            }
        }

        [TestMethod]
        [AutoMoqData]
        public async Task CalculateFeeAsync_WhenValidRegulator_ReturnsBaseFee(
            [Frozen] Mock<IComplianceSchemeFeesRepository> feesRepositoryMock,
            CSBaseFeeCalculationStrategyV3 strategy)
        {
            // Arrange
            var request = new ComplianceSchemeFeesRequestV3Dto
            {
                Regulator = "GB-ENG",
                ApplicationReferenceNumber = "ABC123",
                SubmissionDate = DateTime.UtcNow,
                FileId = Guid.NewGuid(),
                ExternalId = Guid.NewGuid(),
                InvoicePeriod = new DateTimeOffset(),
                PayerId = 1,
                PayerTypeId = 1,
                ComplianceSchemeMembers = new List<ComplianceSchemeMemberDto>
                {
                    new ComplianceSchemeMemberDto
                    {
                        MemberId = "12345",
                        MemberType = "Small",
                        IsOnlineMarketplace = false,
                        NumberOfSubsidiaries = 5,
                        NoOfSubsidiariesOnlineMarketplace = 0
                    }
                }
            };
            var regulator = RegulatorType.Create(request.Regulator);

            feesRepositoryMock.Setup(repo => repo.GetBaseFeeAsync(regulator, request.SubmissionDate, It.IsAny<CancellationToken>()))
                .ReturnsAsync(1380400m); // £13,804 in pence

            // Act
            var result = await strategy.CalculateFeeAsync(request, CancellationToken.None);

            // Assert
            result.Should().Be(1380400m); // £13,804 in pence
        }
    }
}