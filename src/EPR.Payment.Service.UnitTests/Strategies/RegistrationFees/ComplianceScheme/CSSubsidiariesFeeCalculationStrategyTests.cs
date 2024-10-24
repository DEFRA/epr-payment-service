using AutoFixture.MSTest;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.RegistrationFees;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Common.Dtos.Response.RegistrationFees;
using EPR.Payment.Service.Common.UnitTests.TestHelpers;
using EPR.Payment.Service.Common.ValueObjects.RegistrationFees;
using EPR.Payment.Service.Strategies.Interfaces.Common;
using EPR.Payment.Service.Strategies.RegistrationFees.ComplianceScheme;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;

namespace EPR.Payment.Service.UnitTests.Strategies.RegistrationFees.ComplianceScheme
{
    [TestClass]
    public class CSSubsidiariesFeeCalculationStrategyTests
    {
        [TestMethod]
        public void Constructor_WhenFeesRepositoryIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            IComplianceSchemeFeesRepository? nullRepository = null;

            // Act
            Action act = () => new CSSubsidiariesFeeCalculationStrategy(nullRepository!);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Value cannot be null. (Parameter 'feesRepository')");
        }

        [TestMethod]
        public void Constructor_WhenFeesRepositoryIsNotNull_ShouldInitializeSubsidiariesFeeCalculationStrategy()
        {
            // Arrange
            var feesRepositoryMock = new Mock<IComplianceSchemeFeesRepository>();

            // Act
            var strategy = new CSSubsidiariesFeeCalculationStrategy(feesRepositoryMock.Object);

            // Assert
            using (new AssertionScope())
            {
                strategy.Should().NotBeNull();
                strategy.Should().BeAssignableTo<IFeeCalculationStrategy<ComplianceSchemeMemberWithRegulatorDto, SubsidiariesFeeBreakdown>>();
            }
        }

        [TestMethod, AutoMoqData]
        public async Task CalculateFeeAsync_WhenLargeWith50Subsidiaries_ReturnsCorrectSubsidiariesFee(
            [Frozen] Mock<IComplianceSchemeFeesRepository> feesRepositoryMock,
            CSSubsidiariesFeeCalculationStrategy strategy)
        {
            // Arrange
            var request = new ComplianceSchemeMemberWithRegulatorDto
            {
                NumberOfSubsidiaries = 50,
                Regulator = RegulatorType.GBEng,
                NoOfSubsidiariesOnlineMarketplace = 0,
                MemberType = "Large",
                SubmissionDate = DateTime.Now
            };

            feesRepositoryMock.Setup(repo => repo.GetOnlineMarketFeeAsync(request.Regulator, request.SubmissionDate, It.IsAny<CancellationToken>()))
                .ReturnsAsync(257900m); // £2579 in pence per Online Market Subsidiary

            feesRepositoryMock.Setup(repo => repo.GetFirstBandFeeAsync(request.Regulator, request.SubmissionDate, It.IsAny<CancellationToken>()))
                .ReturnsAsync(55800m); // £558 in pence per subsidiary

            feesRepositoryMock.Setup(repo => repo.GetSecondBandFeeAsync(request.Regulator, request.SubmissionDate, It.IsAny<CancellationToken>()))
                .ReturnsAsync(14000m); // £140 in pence per additional subsidiary

            // Act
            var result = await strategy.CalculateFeeAsync(request, CancellationToken.None);

            // Assert
            result.FeeBreakdowns.Select(i => i.TotalPrice).Sum().Should().Be(1536000m); // £15,360 in pence
        }

        [TestMethod, AutoMoqData]
        public async Task CalculateFeeAsync_WhenLargeWith50SubsidiariesAndWithOMP_ReturnsCorrectSubsidiariesFee(
            [Frozen] Mock<IComplianceSchemeFeesRepository> feesRepositoryMock,
            CSSubsidiariesFeeCalculationStrategy strategy)
        {
            // Arrange
            var request = new ComplianceSchemeMemberWithRegulatorDto
            {
                NumberOfSubsidiaries = 50,
                Regulator = RegulatorType.GBEng,
                NoOfSubsidiariesOnlineMarketplace = 2,
                MemberType = "Large",
                SubmissionDate = DateTime.Now
            };

            feesRepositoryMock.Setup(repo => repo.GetOnlineMarketFeeAsync(request.Regulator, request.SubmissionDate, It.IsAny<CancellationToken>()))
                .ReturnsAsync(257900m); // £2579 in pence per Online Market Subsidiary

            feesRepositoryMock.Setup(repo => repo.GetFirstBandFeeAsync(request.Regulator, request.SubmissionDate, It.IsAny<CancellationToken>()))
                .ReturnsAsync(55800m); // £558 in pence per subsidiary

            feesRepositoryMock.Setup(repo => repo.GetSecondBandFeeAsync(request.Regulator, request.SubmissionDate, It.IsAny<CancellationToken>()))
                .ReturnsAsync(14000m); // £140 in pence per additional subsidiary

            // Act
            var result = await strategy.CalculateFeeAsync(request, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                result.TotalSubsidiariesOMPFees.Should().Be(515800m);// £5,158 in pence
                result.FeeBreakdowns.Select(i => i.TotalPrice).Sum().Should().Be(1536000m); // £15,360 in pence
            }
        }

        [TestMethod, AutoMoqData]
        public async Task CalculateFeeAsync_WhenLargeWith101Subsidiaries_ReturnsCorrectSubsidiariesFee(
            [Frozen] Mock<IComplianceSchemeFeesRepository> feesRepositoryMock,
            CSSubsidiariesFeeCalculationStrategy strategy)
        {
            // Arrange
            var request = new ComplianceSchemeMemberWithRegulatorDto
            {
                NumberOfSubsidiaries = 101,
                Regulator = RegulatorType.GBEng,
                NoOfSubsidiariesOnlineMarketplace = 0,
                MemberType = "Large",
                SubmissionDate = DateTime.Now
            };

            feesRepositoryMock.Setup(repo => repo.GetOnlineMarketFeeAsync(request.Regulator, request.SubmissionDate, It.IsAny<CancellationToken>()))
                .ReturnsAsync(257900m); // £2579 in pound per Online Market Subsidiary

            feesRepositoryMock.Setup(repo => repo.GetFirstBandFeeAsync(request.Regulator, request.SubmissionDate, It.IsAny<CancellationToken>()))
                .ReturnsAsync(55800m); // £558 in pence per subsidiary

            feesRepositoryMock.Setup(repo => repo.GetSecondBandFeeAsync(request.Regulator, request.SubmissionDate, It.IsAny<CancellationToken>()))
                .ReturnsAsync(14000m); // £140 in pence per additional subsidiary

            feesRepositoryMock.Setup(repo => repo.GetThirdBandFeeAsync(request.Regulator, request.SubmissionDate, It.IsAny<CancellationToken>()))
                .ReturnsAsync(0m); // £0 in pence per additional subsidiary

            // Act
            var result = await strategy.CalculateFeeAsync(request, CancellationToken.None);

            // Assert
            result.FeeBreakdowns.Select(i => i.TotalPrice).Sum().Should().Be(2236000m); // £22,360 in pence
        }

        [TestMethod, AutoMoqData]
        public async Task CalculateFeeAsync_WhenLargeWith101SubsidiariesAndWithOMP_ReturnsCorrectSubsidiariesFee(
            [Frozen] Mock<IComplianceSchemeFeesRepository> feesRepositoryMock,
            CSSubsidiariesFeeCalculationStrategy strategy)
        {
            // Arrange
            var request = new ComplianceSchemeMemberWithRegulatorDto
            {
                NumberOfSubsidiaries = 101,
                Regulator = RegulatorType.GBEng,
                NoOfSubsidiariesOnlineMarketplace = 2,
                MemberType = "Large",
                SubmissionDate = DateTime.Now
            };

            feesRepositoryMock.Setup(repo => repo.GetOnlineMarketFeeAsync(request.Regulator, request.SubmissionDate, It.IsAny<CancellationToken>()))
                .ReturnsAsync(257900m); // £2579 in pound per Online Market Subsidiary

            feesRepositoryMock.Setup(repo => repo.GetFirstBandFeeAsync(request.Regulator, request.SubmissionDate, It.IsAny<CancellationToken>()))
                .ReturnsAsync(55800m); // £558 in pence per subsidiary

            feesRepositoryMock.Setup(repo => repo.GetSecondBandFeeAsync(request.Regulator, request.SubmissionDate, It.IsAny<CancellationToken>()))
                .ReturnsAsync(14000m); // £140 in pence per additional subsidiary

            feesRepositoryMock.Setup(repo => repo.GetThirdBandFeeAsync(request.Regulator, request.SubmissionDate, It.IsAny<CancellationToken>()))
                .ReturnsAsync(0m); // £0 in pence per additional subsidiary

            // Act
            var result = await strategy.CalculateFeeAsync(request, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                result.TotalSubsidiariesOMPFees.Should().Be(515800m);// £5,158 in pence
                result.FeeBreakdowns.Select(i => i.TotalPrice).Sum().Should().Be(2236000m); // £22,360 in pence
            }
        }

        [TestMethod, AutoMoqData]
        public async Task CalculateFeeAsync_WhenSubsidiariesCountIsZero_ReturnsZeroFee(
            CSSubsidiariesFeeCalculationStrategy strategy)
        {
            // Arrange
            var request = new ComplianceSchemeMemberWithRegulatorDto
            {
                NumberOfSubsidiaries = 0,
                Regulator = RegulatorType.GBEng,
                MemberType = "Large",
                SubmissionDate = DateTime.Now
            };

            // Act
            var result = await strategy.CalculateFeeAsync(request, CancellationToken.None);

            // Assert
            result.FeeBreakdowns.Select(i => i.TotalPrice).Sum().Should().Be(0m);
        }

        [TestMethod, AutoMoqData]
        public async Task CalculateFeeAsync_WhenSmallWith10Subsidiaries_ReturnsCorrectFee(
            [Frozen] Mock<IComplianceSchemeFeesRepository> feesRepositoryMock,
            CSSubsidiariesFeeCalculationStrategy strategy)
        {
            // Arrange
            var request = new ComplianceSchemeMemberWithRegulatorDto
            {
                NumberOfSubsidiaries = 10,
                Regulator = RegulatorType.GBEng,
                MemberType = "Large",
                SubmissionDate = DateTime.Now
            };

            feesRepositoryMock.Setup(repo => repo.GetOnlineMarketFeeAsync(request.Regulator, request.SubmissionDate, It.IsAny<CancellationToken>()))
                .ReturnsAsync(257900m); // £2579 in pence per Online Market Subsidiary

            feesRepositoryMock.Setup(repo => repo.GetFirstBandFeeAsync(request.Regulator, request.SubmissionDate, It.IsAny<CancellationToken>()))
                .ReturnsAsync(55800m); // £558 in pence per subsidiary

            // Act
            var result = await strategy.CalculateFeeAsync(request, CancellationToken.None);

            // Assert
            result.FeeBreakdowns.Select(i => i.TotalPrice).Sum().Should().Be(558000m); // 10 subsidiaries at £558 each
        }

        [TestMethod, AutoMoqData]
        public async Task CalculateFeeAsync_WhenSmallWith10SubsidiariesWithOMP_ReturnsCorrectFee(
            [Frozen] Mock<IComplianceSchemeFeesRepository> feesRepositoryMock,
            CSSubsidiariesFeeCalculationStrategy strategy)
        {
            // Arrange
            var request = new ComplianceSchemeMemberWithRegulatorDto
            {
                NumberOfSubsidiaries = 10,
                Regulator = RegulatorType.GBEng,
                NoOfSubsidiariesOnlineMarketplace = 2,
                MemberType = "Large",
                SubmissionDate = DateTime.Now
            };

            feesRepositoryMock.Setup(repo => repo.GetOnlineMarketFeeAsync(request.Regulator, request.SubmissionDate, It.IsAny<CancellationToken>()))
                .ReturnsAsync(257900m); // £2579 in pence per Online Market Subsidiary

            feesRepositoryMock.Setup(repo => repo.GetFirstBandFeeAsync(request.Regulator, request.SubmissionDate, It.IsAny<CancellationToken>()))
                .ReturnsAsync(55800m); // £558 in pence per subsidiary

            // Act
            var result = await strategy.CalculateFeeAsync(request, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                result.TotalSubsidiariesOMPFees.Should().Be(515800m);// £5,158 in pence
                result.FeeBreakdowns.Select(i => i.TotalPrice).Sum().Should().Be(558000m); // 10 subsidiaries at £558 each
            }
        }
    }
}
