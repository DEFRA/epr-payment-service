using AutoFixture.MSTest;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.RegistrationFees;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.Producer;
using EPR.Payment.Service.Common.Dtos.Response.RegistrationFees;
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
    public class SubsidiariesFeeCalculationStrategyTests
    {
        [TestMethod]
        [AutoMoqData]
        public void Constructor_WhenFeesRepositoryIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            IProducerFeesRepository? nullRepository = null;

            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() => new SubsidiariesFeeCalculationStrategy(nullRepository!));
        }

        [TestMethod]
        [AutoMoqData]
        public void Constructor_WhenFeesRepositoryIsNotNull_ShouldInitializeSubsidiariesFeeCalculationStrategy()
        {
            // Arrange
            var feesRepositoryMock = new Mock<IProducerFeesRepository>();

            // Act
            var strategy = new SubsidiariesFeeCalculationStrategy(feesRepositoryMock.Object);

            // Assert
            using (new AssertionScope())
            {
                strategy.Should().NotBeNull();
                strategy.Should().BeAssignableTo<IFeeCalculationStrategy<ProducerRegistrationFeesRequestDto, SubsidiariesFeeBreakdown>>();
            }
        }

        [TestMethod]
        [AutoMoqData]
        public async Task CalculateFeeAsync_WhenLargeProducerWith50Subsidiaries_ReturnsCorrectSubsidiariesFee(
            [Frozen] Mock<IProducerFeesRepository> feesRepositoryMock,
            SubsidiariesFeeCalculationStrategy strategy)
        {
            // Arrange
            var request = new ProducerRegistrationFeesRequestV2Dto
            {
                ProducerType = "Large",
                NumberOfSubsidiaries = 50,
                Regulator = "GB-ENG",
                ApplicationReferenceNumber = "A123",
                NoOfSubsidiariesOnlineMarketplace = 0,
                SubmissionDate = DateTime.UtcNow,
                FileId = Guid.NewGuid(),
                ExternalId = Guid.NewGuid(),
                InvoicePeriod = new DateTimeOffset(),
                PayerId = 1,
                PayerTypeId = 1
            };

            var regulator = RegulatorType.Create(request.Regulator);

            feesRepositoryMock.Setup(repo => repo.GetOnlineMarketFeeAsync(regulator, request.SubmissionDate, It.IsAny<CancellationToken>()))
                .ReturnsAsync(257900m); // £2579 in pence per Online Market Subsidiary

            feesRepositoryMock.Setup(repo => repo.GetFirstBandFeeAsync(regulator, request.SubmissionDate, It.IsAny<CancellationToken>()))
                .ReturnsAsync(55800m); // £558 in pence per subsidiary

            feesRepositoryMock.Setup(repo => repo.GetSecondBandFeeAsync(regulator, request.SubmissionDate, It.IsAny<CancellationToken>()))
                .ReturnsAsync(14000m); // £140 in pence per additional subsidiary

            // Act
            var result = await strategy.CalculateFeeAsync(request, CancellationToken.None);

            // Assert
            result.FeeBreakdowns.Select(i => i.TotalPrice).Sum().Should().Be(1536000m); // £15,360 in pence
        }

        [TestMethod]
        [AutoMoqData]
        public async Task CalculateFeeAsync_WhenLargeProducerWith50SubsidiariesAndWithOMP_ReturnsCorrectSubsidiariesFee(
            [Frozen] Mock<IProducerFeesRepository> feesRepositoryMock,
            SubsidiariesFeeCalculationStrategy strategy)
        {
            // Arrange
            var request = new ProducerRegistrationFeesRequestV2Dto
            {
                ProducerType = "Large",
                NumberOfSubsidiaries = 50,
                Regulator = "GB-ENG",
                ApplicationReferenceNumber = "A123",
                NoOfSubsidiariesOnlineMarketplace = 2,
                SubmissionDate = DateTime.UtcNow,
                FileId = Guid.NewGuid(),
                ExternalId = Guid.NewGuid(),
                InvoicePeriod = new DateTimeOffset(),
                PayerId = 1,
                PayerTypeId = 1
            };

            var regulator = RegulatorType.Create(request.Regulator);

            feesRepositoryMock.Setup(repo => repo.GetOnlineMarketFeeAsync(regulator, request.SubmissionDate, It.IsAny<CancellationToken>()))
                .ReturnsAsync(257900m); // £2579 in pence per Online Market Subsidiary

            feesRepositoryMock.Setup(repo => repo.GetFirstBandFeeAsync(regulator, request.SubmissionDate, It.IsAny<CancellationToken>()))
                .ReturnsAsync(55800m); // £558 in pence per subsidiary

            feesRepositoryMock.Setup(repo => repo.GetSecondBandFeeAsync(regulator, request.SubmissionDate, It.IsAny<CancellationToken>()))
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

        [TestMethod]
        [AutoMoqData]
        public async Task CalculateFeeAsync_WhenLargeProducerWith101Subsidiaries_ReturnsCorrectSubsidiariesFee(
            [Frozen] Mock<IProducerFeesRepository> feesRepositoryMock,
            SubsidiariesFeeCalculationStrategy strategy)
        {
            // Arrange
            var request = new ProducerRegistrationFeesRequestV2Dto
            {
                ProducerType = "Large",
                NumberOfSubsidiaries = 101,
                Regulator = "GB-ENG",
                NoOfSubsidiariesOnlineMarketplace = 0,
                ApplicationReferenceNumber = "A123",
                SubmissionDate = DateTime.UtcNow,
                FileId = Guid.NewGuid(),
                ExternalId = Guid.NewGuid(),
                InvoicePeriod = new DateTimeOffset(),
                PayerId = 1,
                PayerTypeId = 1
            };

            var regulator = RegulatorType.Create(request.Regulator);

            feesRepositoryMock.Setup(repo => repo.GetOnlineMarketFeeAsync(regulator, request.SubmissionDate, It.IsAny<CancellationToken>()))
                .ReturnsAsync(257900m); // £2579 in pound per Online Market Subsidiary

            feesRepositoryMock.Setup(repo => repo.GetFirstBandFeeAsync(regulator, request.SubmissionDate, It.IsAny<CancellationToken>()))
                .ReturnsAsync(55800m); // £558 in pence per subsidiary

            feesRepositoryMock.Setup(repo => repo.GetSecondBandFeeAsync(regulator, request.SubmissionDate, It.IsAny<CancellationToken>()))
                .ReturnsAsync(14000m); // £140 in pence per additional subsidiary

            feesRepositoryMock.Setup(repo => repo.GetThirdBandFeeAsync(regulator, request.SubmissionDate, It.IsAny<CancellationToken>()))
                .ReturnsAsync(0m); // £0 in pence per additional subsidiary

            // Act
            var result = await strategy.CalculateFeeAsync(request, CancellationToken.None);

            // Assert
            result.FeeBreakdowns.Select(i => i.TotalPrice).Sum().Should().Be(2236000m); // £22,360 in pence
        }

        [TestMethod]
        [AutoMoqData]
        public async Task CalculateFeeAsync_WhenLargeProducerWith101SubsidiariesAndWithOMP_ReturnsCorrectSubsidiariesFee(
            [Frozen] Mock<IProducerFeesRepository> feesRepositoryMock,
            SubsidiariesFeeCalculationStrategy strategy)
        {
            // Arrange
            var request = new ProducerRegistrationFeesRequestV2Dto
            {
                ProducerType = "Large",
                NumberOfSubsidiaries = 101,
                Regulator = "GB-ENG",
                NoOfSubsidiariesOnlineMarketplace = 2,
                ApplicationReferenceNumber = "A123",
                SubmissionDate = DateTime.UtcNow,
                FileId = Guid.NewGuid(),
                ExternalId = Guid.NewGuid(),
                InvoicePeriod = new DateTimeOffset(),
                PayerId = 1,
                PayerTypeId = 1
            };

            var regulator = RegulatorType.Create(request.Regulator);

            feesRepositoryMock.Setup(repo => repo.GetOnlineMarketFeeAsync(regulator, request.SubmissionDate, It.IsAny<CancellationToken>()))
                .ReturnsAsync(257900m); // £2579 in pound per Online Market Subsidiary

            feesRepositoryMock.Setup(repo => repo.GetFirstBandFeeAsync(regulator, request.SubmissionDate, It.IsAny<CancellationToken>()))
                .ReturnsAsync(55800m); // £558 in pence per subsidiary

            feesRepositoryMock.Setup(repo => repo.GetSecondBandFeeAsync(regulator, request.SubmissionDate, It.IsAny<CancellationToken>()))
                .ReturnsAsync(14000m); // £140 in pence per additional subsidiary

            feesRepositoryMock.Setup(repo => repo.GetThirdBandFeeAsync(regulator, request.SubmissionDate, It.IsAny<CancellationToken>()))
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

        [TestMethod]
        [AutoMoqData]
        public async Task CalculateFeeAsync_WhenSubsidiariesCountIsZero_ReturnsZeroFee(
            [Frozen] Mock<IProducerFeesRepository> feesRepositoryMock,
            SubsidiariesFeeCalculationStrategy strategy)
        {
            // Arrange
            var request = new ProducerRegistrationFeesRequestV2Dto
            {
                ProducerType = "Large",
                NumberOfSubsidiaries = 0,
                Regulator = "GB-ENG",
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
            result.FeeBreakdowns.Select(i => i.TotalPrice).Sum().Should().Be(0m);
        }

        [TestMethod]
        [AutoMoqData]
        public async Task CalculateFeeAsync_WhenRegulatorIsNullOrEmpty_ThrowsArgumentException(
            [Frozen] Mock<IProducerFeesRepository> feesRepositoryMock,
            SubsidiariesFeeCalculationStrategy strategy)
        {
            // Arrange
            var request = new ProducerRegistrationFeesRequestV2Dto
            {
                ProducerType = "Large",
                NumberOfSubsidiaries = 10,
                Regulator = null!,
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
        public async Task CalculateFeeAsync_WhenSmallProducerWith10Subsidiaries_ReturnsCorrectFee(
            [Frozen] Mock<IProducerFeesRepository> feesRepositoryMock,
            SubsidiariesFeeCalculationStrategy strategy)
        {
            // Arrange
            var request = new ProducerRegistrationFeesRequestV2Dto
            {
                ProducerType = "Large",
                NumberOfSubsidiaries = 10,
                Regulator = "GB-ENG",
                ApplicationReferenceNumber = "A123",
                SubmissionDate = DateTime.UtcNow,
                FileId = Guid.NewGuid(),
                ExternalId = Guid.NewGuid(),
                InvoicePeriod = new DateTimeOffset(),
                PayerId = 1,
                PayerTypeId = 1
            };

            var regulator = RegulatorType.Create(request.Regulator);

            feesRepositoryMock.Setup(repo => repo.GetOnlineMarketFeeAsync(regulator, request.SubmissionDate, It.IsAny<CancellationToken>()))
                .ReturnsAsync(257900m); // £2579 in pence per Online Market Subsidiary

            feesRepositoryMock.Setup(repo => repo.GetFirstBandFeeAsync(regulator, request.SubmissionDate, It.IsAny<CancellationToken>()))
                .ReturnsAsync(55800m); // £558 in pence per subsidiary

            // Act
            var result = await strategy.CalculateFeeAsync(request, CancellationToken.None);

            // Assert
            result.FeeBreakdowns.Select(i => i.TotalPrice).Sum().Should().Be(558000m); // 10 subsidiaries at £558 each
        }

        [TestMethod]
        [AutoMoqData]
        public async Task CalculateFeeAsync_WhenSmallProducerWith10SubsidiariesWithOMP_ReturnsCorrectFee(
            [Frozen] Mock<IProducerFeesRepository> feesRepositoryMock,
            SubsidiariesFeeCalculationStrategy strategy)
        {
            // Arrange
            var request = new ProducerRegistrationFeesRequestV2Dto
            {
                ProducerType = "Large",
                NumberOfSubsidiaries = 10,
                Regulator = "GB-ENG",
                NoOfSubsidiariesOnlineMarketplace = 2,
                ApplicationReferenceNumber = "A123",
                SubmissionDate = DateTime.UtcNow,
                FileId = Guid.NewGuid(),
                ExternalId = Guid.NewGuid(),
                InvoicePeriod = new DateTimeOffset(),
                PayerId = 1,
                PayerTypeId = 1
            };

            var regulator = RegulatorType.Create(request.Regulator);

            feesRepositoryMock.Setup(repo => repo.GetOnlineMarketFeeAsync(regulator, request.SubmissionDate, It.IsAny<CancellationToken>()))
                .ReturnsAsync(257900m); // £2579 in pence per Online Market Subsidiary

            feesRepositoryMock.Setup(repo => repo.GetFirstBandFeeAsync(regulator, request.SubmissionDate, It.IsAny<CancellationToken>()))
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

        [TestMethod]
        [AutoMoqData]
        public async Task CalculateFeeAsync_WhenLargeProducerWithClosedLoopRecyclingSubsidiaries_PopulatesClosedLoopBreakdown(
            [Frozen] Mock<IProducerFeesRepository> feesRepositoryMock,
            SubsidiariesFeeCalculationStrategy strategy)
        {
            var request = new ProducerRegistrationFeesRequestDto
            {
                ProducerType = "Large",
                NumberOfSubsidiaries = 5,
                Regulator = "GB-ENG",
                ApplicationReferenceNumber = "A123",
                NoOfSubsidiariesOnlineMarketplace = 0,
                NoOfSubsidiariesClosedLoopRecycling = 3,
                SubmissionDate = DateTime.UtcNow
            };
            var regulator = RegulatorType.Create(request.Regulator);

            feesRepositoryMock.Setup(r => r.GetOnlineMarketFeeAsync(regulator, request.SubmissionDate, It.IsAny<CancellationToken>())).ReturnsAsync(0m);
            feesRepositoryMock.Setup(r => r.GetClosedLoopRecyclingFeeAsync(regulator, request.SubmissionDate, It.IsAny<CancellationToken>())).ReturnsAsync(254800m);
            feesRepositoryMock.Setup(r => r.GetFirstBandFeeAsync(regulator, request.SubmissionDate, It.IsAny<CancellationToken>())).ReturnsAsync(55800m);
            feesRepositoryMock.Setup(r => r.GetSecondBandFeeAsync(regulator, request.SubmissionDate, It.IsAny<CancellationToken>())).ReturnsAsync(14000m);

            var result = await strategy.CalculateFeeAsync(request, CancellationToken.None);

            using (new AssertionScope())
            {
                result.CountOfClosedLoopRecyclingSubsidiaries.Should().Be(3);
                result.UnitClosedLoopRecyclingFees.Should().Be(254800m);
                result.TotalSubsidiariesClosedLoopRecyclingFees.Should().Be(764400m); // 3 * £2,548
            }
        }

        [TestMethod]
        [AutoMoqData]
        public async Task CalculateFeeAsync_WhenNoClosedLoopRecyclingSubsidiaries_DoesNotCallClosedLoopRepoAndReturnsZero(
            [Frozen] Mock<IProducerFeesRepository> feesRepositoryMock,
            SubsidiariesFeeCalculationStrategy strategy)
        {
            var request = new ProducerRegistrationFeesRequestDto
            {
                ProducerType = "Large",
                NumberOfSubsidiaries = 5,
                Regulator = "GB-ENG",
                ApplicationReferenceNumber = "A123",
                NoOfSubsidiariesOnlineMarketplace = 0,
                NoOfSubsidiariesClosedLoopRecycling = 0,
                SubmissionDate = DateTime.UtcNow
            };
            var regulator = RegulatorType.Create(request.Regulator);

            feesRepositoryMock.Setup(r => r.GetOnlineMarketFeeAsync(regulator, request.SubmissionDate, It.IsAny<CancellationToken>())).ReturnsAsync(0m);
            feesRepositoryMock.Setup(r => r.GetFirstBandFeeAsync(regulator, request.SubmissionDate, It.IsAny<CancellationToken>())).ReturnsAsync(55800m);
            feesRepositoryMock.Setup(r => r.GetSecondBandFeeAsync(regulator, request.SubmissionDate, It.IsAny<CancellationToken>())).ReturnsAsync(14000m);

            var result = await strategy.CalculateFeeAsync(request, CancellationToken.None);

            using (new AssertionScope())
            {
                result.CountOfClosedLoopRecyclingSubsidiaries.Should().Be(0);
                result.UnitClosedLoopRecyclingFees.Should().Be(0m);
                result.TotalSubsidiariesClosedLoopRecyclingFees.Should().Be(0m);
            }
            feesRepositoryMock.Verify(r => r.GetClosedLoopRecyclingFeeAsync(It.IsAny<RegulatorType>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}