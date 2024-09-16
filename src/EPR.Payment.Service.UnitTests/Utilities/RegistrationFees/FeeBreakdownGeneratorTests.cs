using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.MSTest;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.RegistrationFees;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.Producer;
using EPR.Payment.Service.Common.Dtos.Response.RegistrationFees;
using EPR.Payment.Service.Common.UnitTests.TestHelpers;
using EPR.Payment.Service.Common.ValueObjects.RegistrationFees;
using EPR.Payment.Service.Utilities.RegistrationFees.Interfaces;
using EPR.Payment.Service.Utilities.RegistrationFees.Producer;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;

namespace EPR.Payment.Service.UnitTests.Utilities.RegistrationFees
{
    [TestClass]
    public class FeeBreakdownGeneratorTests
    {
        private Mock<IProducerFeesRepository> _feesRepositoryMock = null!;
        private IFeeBreakdownGenerator<ProducerRegistrationFeesRequestDto, RegistrationFeesResponseDto>? _feeBreakdownGenerator = null;
        private IFixture _fixture = null!;

        [TestInitialize]
        public void TestInitialize()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _feesRepositoryMock = _fixture.Freeze<Mock<IProducerFeesRepository>>();
            _feeBreakdownGenerator = new FeeBreakdownGenerator(_feesRepositoryMock.Object);
        }

        [TestMethod]
        public void Constructor_WhenFeesRepositoryIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            IProducerFeesRepository? nullRepository = null;

            // Act
            Action act = () => new FeeBreakdownGenerator(nullRepository!);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Value cannot be null. (Parameter 'feesRepository')");
        }

        [TestMethod]
        public void Constructor_WhenFeesRepositoryIsNotNull_ShouldInitializeFeeBreakdownGenerator()
        {
            // Arrange
            var feesRepositoryMock = _fixture.Create<Mock<IProducerFeesRepository>>();

            // Act
            var feeBreakdownGenerator = new FeeBreakdownGenerator(feesRepositoryMock.Object);

            // Assert
            using (new AssertionScope())
            {
                feeBreakdownGenerator.Should().NotBeNull();
                feeBreakdownGenerator.Should().BeAssignableTo<IFeeBreakdownGenerator<ProducerRegistrationFeesRequestDto, RegistrationFeesResponseDto>>();
            }
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GenerateFeeBreakdown_WhenLargeProducerWith50SubsidiariesWithoutOnlineMarket_CreatesCorrectBreakdown(
            [Frozen] RegistrationFeesResponseDto response,
            [Frozen] ProducerRegistrationFeesRequestDto request)
        {
            // Arrange
            response.BaseFee = 262000m; // £2,620 in pence
            request.NumberOfSubsidiaries = 50;
            request.IsOnlineMarketplace = false;
            request.Regulator = "GB-ENG";
            response.FeeBreakdowns = new List<FeeBreakdown>();

            _feesRepositoryMock.Setup(repo => repo.GetFirst20SubsidiariesFeeAsync(It.IsAny<RegulatorType>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(55800m); // £558 in pence per subsidiary

            _feesRepositoryMock.Setup(repo => repo.GetAdditionalUpTo100SubsidiariesFeeAsync(It.IsAny<RegulatorType>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(14000m); // £140 in pence per subsidiary

            // Act
            await _feeBreakdownGenerator!.GenerateFeeBreakdownAsync(response, request, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                // Verify the count
                response.FeeBreakdowns.Should().HaveCount(3);

                // Verify individual breakdowns
                response.FeeBreakdowns.Should().ContainSingle(f => f.Description == "Producer Registration Fee (£2620)")
                    .Which.Amount.Should().Be(262000m);

                response.FeeBreakdowns.Should().ContainSingle(f => f.Description == "First 20 Subsidiaries Fee (£558 each)")
                    .Which.Amount.Should().Be(1116000m);

                response.FeeBreakdowns.Should().ContainSingle(f => f.Description == "Next 30 Subsidiaries Fee (£140 each)")
                    .Which.Amount.Should().Be(420000m);
            }
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GenerateFeeBreakdown_WhenLargeProducerWith50SubsidiariesWithOnlineMarket_CreatesCorrectBreakdown(
    [Frozen] RegistrationFeesResponseDto response,
    [Frozen] ProducerRegistrationFeesRequestDto request)
        {
            // Arrange
            response.BaseFee = 262000m; // £2,620 in pence
            request.NumberOfSubsidiaries = 50;
            request.IsOnlineMarketplace = true;
            response.OnlineMarket = 257900m; // £2,579 in pence
            request.Regulator = "GB-ENG";
            response.FeeBreakdowns = new List<FeeBreakdown>();

            _feesRepositoryMock.Setup(repo => repo.GetFirst20SubsidiariesFeeAsync(It.IsAny<RegulatorType>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(55800m); // £558 in pence per subsidiary

            _feesRepositoryMock.Setup(repo => repo.GetAdditionalUpTo100SubsidiariesFeeAsync(It.IsAny<RegulatorType>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(14000m); // £140 in pence per subsidiary

            // Act
            await _feeBreakdownGenerator!.GenerateFeeBreakdownAsync(response, request, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                // Verify the count
                response.FeeBreakdowns.Should().HaveCount(4);

                // Verify individual breakdowns
                response.FeeBreakdowns.Should().ContainSingle(f => f.Description == "Producer Registration Fee (£2620)")
                    .Which.Amount.Should().Be(262000m);

                response.FeeBreakdowns.Should().ContainSingle(f => f.Description == "Online Marketplace Fee (£2579)")
                     .Which.Amount.Should().Be(257900m);

                response.FeeBreakdowns.Should().ContainSingle(f => f.Description == "First 20 Subsidiaries Fee (£558 each)")
                    .Which.Amount.Should().Be(1116000m);

                response.FeeBreakdowns.Should().ContainSingle(f => f.Description == "Next 30 Subsidiaries Fee (£140 each)")
                    .Which.Amount.Should().Be(420000m);
            }
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GenerateFeeBreakdown_WhenLargeProducerWith110SubsidiariesWithoutOnlineMarket_CreatesCorrectBreakdown(
    [Frozen] RegistrationFeesResponseDto response,
    [Frozen] ProducerRegistrationFeesRequestDto request)
        {
            // Arrange
            response.BaseFee = 262000m; // £2,620 in pence
            request.NumberOfSubsidiaries = 110;
            request.IsOnlineMarketplace = false;
            request.Regulator = "GB-ENG";
            response.FeeBreakdowns = new List<FeeBreakdown>();

            _feesRepositoryMock.Setup(repo => repo.GetFirst20SubsidiariesFeeAsync(It.IsAny<RegulatorType>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(55800m); // £558 in pence per subsidiary

            _feesRepositoryMock.Setup(repo => repo.GetAdditionalUpTo100SubsidiariesFeeAsync(It.IsAny<RegulatorType>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(14000m); // £140 in pence per subsidiary

            _feesRepositoryMock.Setup(repo => repo.GetAdditionalMoreThan100SubsidiariesFeeAsync(It.IsAny<RegulatorType>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(0m); // £0 in pence per subsidiary

            // Act
            await _feeBreakdownGenerator!.GenerateFeeBreakdownAsync(response, request, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                // Verify the count
                response.FeeBreakdowns.Should().HaveCount(4);

                // Verify individual breakdowns
                response.FeeBreakdowns.Should().ContainSingle(f => f.Description == "Producer Registration Fee (£2620)")
                    .Which.Amount.Should().Be(262000m);

                response.FeeBreakdowns.Should().ContainSingle(f => f.Description == "First 20 Subsidiaries Fee (£558 each)")
                    .Which.Amount.Should().Be(1116000m);

                response.FeeBreakdowns.Should().ContainSingle(f => f.Description == "Next 80 Subsidiaries Fee (£140 each)")
               .Which.Amount.Should().Be(1120000m);

                response.FeeBreakdowns.Should().ContainSingle(f => f.Description == "Next 10 Subsidiaries Fee (£0 each)")
                    .Which.Amount.Should().Be(0m);
            }
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GenerateFeeBreakdown_WhenLargeProducerWith110SubsidiariesWitOnlineMarket_CreatesCorrectBreakdown(
    [Frozen] RegistrationFeesResponseDto response,
    [Frozen] ProducerRegistrationFeesRequestDto request)
        {
            // Arrange
            response.BaseFee = 262000m; // £2,620 in pence
            request.NumberOfSubsidiaries = 110;
            request.IsOnlineMarketplace = true;
            response.OnlineMarket = 257900m; // £2,579 in pence
            request.Regulator = "GB-ENG";
            response.FeeBreakdowns = new List<FeeBreakdown>();

            _feesRepositoryMock.Setup(repo => repo.GetFirst20SubsidiariesFeeAsync(It.IsAny<RegulatorType>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(55800m); // £558 in pence per subsidiary

            _feesRepositoryMock.Setup(repo => repo.GetAdditionalUpTo100SubsidiariesFeeAsync(It.IsAny<RegulatorType>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(14000m); // £140 in pence per subsidiary

            _feesRepositoryMock.Setup(repo => repo.GetAdditionalMoreThan100SubsidiariesFeeAsync(It.IsAny<RegulatorType>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(0m); // £0 in pence per subsidiary

            // Act
            await _feeBreakdownGenerator!.GenerateFeeBreakdownAsync(response, request, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                // Verify the count
                response.FeeBreakdowns.Should().HaveCount(5);

                // Verify individual breakdowns
                response.FeeBreakdowns.Should().ContainSingle(f => f.Description == "Producer Registration Fee (£2620)")
                    .Which.Amount.Should().Be(262000m);

                response.FeeBreakdowns.Should().ContainSingle(f => f.Description == "Online Marketplace Fee (£2579)")
                     .Which.Amount.Should().Be(257900m);

                response.FeeBreakdowns.Should().ContainSingle(f => f.Description == "First 20 Subsidiaries Fee (£558 each)")
                    .Which.Amount.Should().Be(1116000m);

                response.FeeBreakdowns.Should().ContainSingle(f => f.Description == "Next 80 Subsidiaries Fee (£140 each)")
               .Which.Amount.Should().Be(1120000m);

                response.FeeBreakdowns.Should().ContainSingle(f => f.Description == "Next 10 Subsidiaries Fee (£0 each)")
                    .Which.Amount.Should().Be(0m);
            }
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GenerateFeeBreakdown_WhenLargeProducerWith10SubsidiariesWithoutOnlineMarket_CreatesCorrectBreakdown(
            [Frozen] RegistrationFeesResponseDto response,
            [Frozen] ProducerRegistrationFeesRequestDto request)
        {
            // Arrange
            response.BaseFee = 262000m; // £2,620 in pence
            request.NumberOfSubsidiaries = 10;
            request.IsOnlineMarketplace = false;
            request.Regulator = "GB-ENG";
            response.FeeBreakdowns = new List<FeeBreakdown>();

            _feesRepositoryMock.Setup(repo => repo.GetFirst20SubsidiariesFeeAsync(It.IsAny<RegulatorType>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(55800m); // £558 in pence per subsidiary

            // Act
            await _feeBreakdownGenerator!.GenerateFeeBreakdownAsync(response, request, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                response.FeeBreakdowns.Should().HaveCount(2);

                response.FeeBreakdowns.Should().ContainSingle(f => f.Description == "Producer Registration Fee (£2620)")
                    .Which.Amount.Should().Be(262000m);

                response.FeeBreakdowns.Should().ContainSingle(f => f.Description == "First 10 Subsidiaries Fee (£558 each)")
                    .Which.Amount.Should().Be(558000m); // 10 subsidiaries at £558 each
            }
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GenerateFeeBreakdown_WhenLargeProducerWith10SubsidiariesWithOnlineMarket_CreatesCorrectBreakdown(
            [Frozen] RegistrationFeesResponseDto response,
            [Frozen] ProducerRegistrationFeesRequestDto request)
        {
            // Arrange
            response.BaseFee = 262000m; // £2,620 in pence
            request.NumberOfSubsidiaries = 10;
            request.IsOnlineMarketplace = true;
            response.OnlineMarket = 257900m; // £2,579 in pence
            request.Regulator = "GB-ENG";
            response.FeeBreakdowns = new List<FeeBreakdown>();

            _feesRepositoryMock.Setup(repo => repo.GetFirst20SubsidiariesFeeAsync(It.IsAny<RegulatorType>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(55800m); // £558 in pence per subsidiary

            // Act
            await _feeBreakdownGenerator!.GenerateFeeBreakdownAsync(response, request, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                response.FeeBreakdowns.Should().HaveCount(3);

                response.FeeBreakdowns.Should().ContainSingle(f => f.Description == "Producer Registration Fee (£2620)")
                    .Which.Amount.Should().Be(262000m);

                response.FeeBreakdowns.Should().ContainSingle(f => f.Description == "Online Marketplace Fee (£2579)")
                    .Which.Amount.Should().Be(257900m);

                response.FeeBreakdowns.Should().ContainSingle(f => f.Description == "First 10 Subsidiaries Fee (£558 each)")
                    .Which.Amount.Should().Be(558000m); // 10 subsidiaries at £558 each
            }
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GenerateFeeBreakdown_WhenLargeProducerWithNoBaseFeeAnd50SubsidiariesWithoutOnlineMarket_CreatesCorrectBreakdown(
            [Frozen] RegistrationFeesResponseDto response,
            [Frozen] ProducerRegistrationFeesRequestDto request)
        {
            // Arrange
            response.BaseFee = 0m; // No base fee
            request.NumberOfSubsidiaries = 50;
            request.IsOnlineMarketplace = false;
            request.Regulator = "GB-ENG";
            response.FeeBreakdowns = new List<FeeBreakdown>();

            _feesRepositoryMock.Setup(repo => repo.GetFirst20SubsidiariesFeeAsync(It.IsAny<RegulatorType>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(55800m); // £558 in pence per subsidiary

            _feesRepositoryMock.Setup(repo => repo.GetAdditionalUpTo100SubsidiariesFeeAsync(It.IsAny<RegulatorType>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(14000m); // £140 in pence per subsidiary

            // Act
            await _feeBreakdownGenerator!.GenerateFeeBreakdownAsync(response, request, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                response.FeeBreakdowns.Should().HaveCount(2);

                response.FeeBreakdowns.Should().NotContain(f => f.Description == "Producer Registration Fee");

                response.FeeBreakdowns.Should().ContainSingle(f => f.Description == "First 20 Subsidiaries Fee (£558 each)")
                    .Which.Amount.Should().Be(1116000m); // 20 subsidiaries at £558 each

                response.FeeBreakdowns.Should().ContainSingle(f => f.Description == "Next 30 Subsidiaries Fee (£140 each)")
                    .Which.Amount.Should().Be(420000m); // 30 subsidiaries at £140 each
            }
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GenerateFeeBreakdown_WhenLargeProducerWithNoBaseFeeAnd50SubsidiariesWithOnlineMarket_CreatesCorrectBreakdown(
            [Frozen] RegistrationFeesResponseDto response,
            [Frozen] ProducerRegistrationFeesRequestDto request)
        {
            // Arrange
            response.BaseFee = 0m; // No base fee
            request.NumberOfSubsidiaries = 50;
            request.IsOnlineMarketplace = true;
            response.OnlineMarket = 257900m; // £2,579 in pence
            request.Regulator = "GB-ENG";
            response.FeeBreakdowns = new List<FeeBreakdown>();

            _feesRepositoryMock.Setup(repo => repo.GetFirst20SubsidiariesFeeAsync(It.IsAny<RegulatorType>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(55800m); // £558 in pence per subsidiary

            _feesRepositoryMock.Setup(repo => repo.GetAdditionalUpTo100SubsidiariesFeeAsync(It.IsAny<RegulatorType>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(14000m); // £140 in pence per subsidiary

            // Act
            await _feeBreakdownGenerator!.GenerateFeeBreakdownAsync(response, request, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                response.FeeBreakdowns.Should().HaveCount(3);

                response.FeeBreakdowns.Should().NotContain(f => f.Description == "Producer Registration Fee");

                response.FeeBreakdowns.Should().ContainSingle(f => f.Description == "Online Marketplace Fee (£2579)")
                    .Which.Amount.Should().Be(257900m);

                response.FeeBreakdowns.Should().ContainSingle(f => f.Description == "First 20 Subsidiaries Fee (£558 each)")
                    .Which.Amount.Should().Be(1116000m); // 20 subsidiaries at £558 each

                response.FeeBreakdowns.Should().ContainSingle(f => f.Description == "Next 30 Subsidiaries Fee (£140 each)")
                    .Which.Amount.Should().Be(420000m); // 30 subsidiaries at £140 each
            }
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GenerateFeeBreakdown_WhenSmallProducerWith25SubsidiariesWithoutOnlineMarket_CreatesCorrectBreakdown(
            [Frozen] RegistrationFeesResponseDto response,
            [Frozen] ProducerRegistrationFeesRequestDto request)
        {
            // Arrange
            response.BaseFee = 121600m; // £1,216 in pence
            request.IsOnlineMarketplace = false;
            request.NumberOfSubsidiaries = 25;
            request.Regulator = "GB-ENG";
            response.FeeBreakdowns = new List<FeeBreakdown>();

            _feesRepositoryMock.Setup(repo => repo.GetFirst20SubsidiariesFeeAsync(It.IsAny<RegulatorType>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(55800m); // £558 in pence per subsidiary

            _feesRepositoryMock.Setup(repo => repo.GetAdditionalUpTo100SubsidiariesFeeAsync(It.IsAny<RegulatorType>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(14000m); // £140 in pence per subsidiary

            // Act
            await _feeBreakdownGenerator!.GenerateFeeBreakdownAsync(response, request, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                response.FeeBreakdowns.Should().HaveCount(3);

                response.FeeBreakdowns.Should().ContainSingle(f => f.Description == "Producer Registration Fee (£1216)")
                    .Which.Amount.Should().Be(121600m);

                response.FeeBreakdowns.Should().ContainSingle(f => f.Description == "First 20 Subsidiaries Fee (£558 each)")
                    .Which.Amount.Should().Be(1116000m);

                response.FeeBreakdowns.Should().ContainSingle(f => f.Description == "Next 5 Subsidiaries Fee (£140 each)")
                    .Which.Amount.Should().Be(70000m);
            }
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GenerateFeeBreakdown_WhenSmallProducerWith25SubsidiariesWithOnlineMarket_CreatesCorrectBreakdown(
            [Frozen] RegistrationFeesResponseDto response,
            [Frozen] ProducerRegistrationFeesRequestDto request)
        {
            // Arrange
            response.BaseFee = 121600m; // £1,216 in pence
            request.IsOnlineMarketplace = true;
            response.OnlineMarket = 257900m; // £2,579 in pence
            request.NumberOfSubsidiaries = 25;
            request.Regulator = "GB-ENG";
            response.FeeBreakdowns = new List<FeeBreakdown>();

            _feesRepositoryMock.Setup(repo => repo.GetFirst20SubsidiariesFeeAsync(It.IsAny<RegulatorType>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(55800m); // £558 in pence per subsidiary

            _feesRepositoryMock.Setup(repo => repo.GetAdditionalUpTo100SubsidiariesFeeAsync(It.IsAny<RegulatorType>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(14000m); // £140 in pence per subsidiary

            // Act
            await _feeBreakdownGenerator!.GenerateFeeBreakdownAsync(response, request, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                response.FeeBreakdowns.Should().HaveCount(4);

                response.FeeBreakdowns.Should().ContainSingle(f => f.Description == "Producer Registration Fee (£1216)")
                    .Which.Amount.Should().Be(121600m);

                response.FeeBreakdowns.Should().ContainSingle(f => f.Description == "Online Marketplace Fee (£2579)")
                    .Which.Amount.Should().Be(257900m);

                response.FeeBreakdowns.Should().ContainSingle(f => f.Description == "First 20 Subsidiaries Fee (£558 each)")
                    .Which.Amount.Should().Be(1116000m);

                response.FeeBreakdowns.Should().ContainSingle(f => f.Description == "Next 5 Subsidiaries Fee (£140 each)")
                    .Which.Amount.Should().Be(70000m);
            }
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GenerateFeeBreakdown_WhenSmallProducerWith20SubsidiariesWithoutOnlineMarket_CreatesCorrectBreakdown(
            [Frozen] RegistrationFeesResponseDto response,
            [Frozen] ProducerRegistrationFeesRequestDto request)
        {
            // Arrange
            response.BaseFee = 121600m; // £1,216 in pence
            request.IsOnlineMarketplace = false;
            request.NumberOfSubsidiaries = 20;
            request.Regulator = "GB-ENG";
            response.FeeBreakdowns = new List<FeeBreakdown>();

            _feesRepositoryMock.Setup(repo => repo.GetFirst20SubsidiariesFeeAsync(It.IsAny<RegulatorType>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(55800m); // £558 in pence per subsidiary

            // Act
            await _feeBreakdownGenerator!.GenerateFeeBreakdownAsync(response, request, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                response.FeeBreakdowns.Should().HaveCount(2);

                response.FeeBreakdowns.Should().ContainSingle(f => f.Description == "Producer Registration Fee (£1216)")
                    .Which.Amount.Should().Be(121600m);

                response.FeeBreakdowns.Should().ContainSingle(f => f.Description == "First 20 Subsidiaries Fee (£558 each)")
                    .Which.Amount.Should().Be(1116000m);
            }
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GenerateFeeBreakdown_WhenSmallProducerWith20SubsidiariesWithOnlineMarket_CreatesCorrectBreakdown(
        [Frozen] RegistrationFeesResponseDto response,
        [Frozen] ProducerRegistrationFeesRequestDto request)
        {
            // Arrange
            response.BaseFee = 121600m; // £1,216 in pence
            request.IsOnlineMarketplace = true;
            response.OnlineMarket = 257900m; // £2,579 in pence
            request.NumberOfSubsidiaries = 20;
            request.Regulator = "GB-ENG";
            response.FeeBreakdowns = new List<FeeBreakdown>();

            _feesRepositoryMock.Setup(repo => repo.GetFirst20SubsidiariesFeeAsync(It.IsAny<RegulatorType>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(55800m); // £558 in pence per subsidiary

            // Act
            await _feeBreakdownGenerator!.GenerateFeeBreakdownAsync(response, request, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                response.FeeBreakdowns.Should().HaveCount(3);

                response.FeeBreakdowns.Should().ContainSingle(f => f.Description == "Producer Registration Fee (£1216)")
                    .Which.Amount.Should().Be(121600m);

                response.FeeBreakdowns.Should().ContainSingle(f => f.Description == "Online Marketplace Fee (£2579)")
                    .Which.Amount.Should().Be(257900m);

                response.FeeBreakdowns.Should().ContainSingle(f => f.Description == "First 20 Subsidiaries Fee (£558 each)")
                    .Which.Amount.Should().Be(1116000m);
            }
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GenerateFeeBreakdown_WhenLargeProducerWithNoSubsidiariesWithoutOnlineMarket_CreatesBaseFeeOnly(
            [Frozen] RegistrationFeesResponseDto response,
            [Frozen] ProducerRegistrationFeesRequestDto request)
        {
            // Arrange
            response.BaseFee = 262000m; // £2,620 in pence
            request.IsOnlineMarketplace = false;
            request.NumberOfSubsidiaries = 0;
            request.Regulator = "GB-ENG";
            response.FeeBreakdowns = new List<FeeBreakdown>();

            // Act
            await _feeBreakdownGenerator!.GenerateFeeBreakdownAsync(response, request, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                response.FeeBreakdowns.Should().HaveCount(1);

                response.FeeBreakdowns.Should().ContainSingle(f => f.Description == "Producer Registration Fee (£2620)")
                .Which.Amount.Should().Be(262000m);

                response.FeeBreakdowns.Should().NotContain(f => f.Description.Contains("Subsidiaries Fee"));
            }
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GenerateFeeBreakdown_WhenLargeProducerWithNoSubsidiariesWithOnlineMarket_CreatesBaseFeeOnly(
        [Frozen] RegistrationFeesResponseDto response,
        [Frozen] ProducerRegistrationFeesRequestDto request)
        {
            // Arrange
            response.BaseFee = 262000m; // £2,620 in pence
            request.IsOnlineMarketplace = true;
            response.OnlineMarket = 257900m; // £2,579 in pence
            request.NumberOfSubsidiaries = 0;
            request.Regulator = "GB-ENG";
            response.FeeBreakdowns = new List<FeeBreakdown>();

            // Act
            await _feeBreakdownGenerator!.GenerateFeeBreakdownAsync(response, request, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                response.FeeBreakdowns.Should().HaveCount(2);

                response.FeeBreakdowns.Should().ContainSingle(f => f.Description == "Producer Registration Fee (£2620)")
                .Which.Amount.Should().Be(262000m);

                response.FeeBreakdowns.Should().ContainSingle(f => f.Description == "Online Marketplace Fee (£2579)")
                    .Which.Amount.Should().Be(257900m);

                response.FeeBreakdowns.Should().NotContain(f => f.Description.Contains("Subsidiaries Fee"));
            }
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GenerateFeeBreakdown_WhenSmallProducerWithNoSubsidiariesWithoutOnlineMarket_CreatesBaseFeeOnly(
            [Frozen] RegistrationFeesResponseDto response,
            [Frozen] ProducerRegistrationFeesRequestDto request)
        {
            // Arrange
            response.BaseFee = 121600m; // £1,216 in pence
            request.IsOnlineMarketplace = false;
            request.NumberOfSubsidiaries = 0;
            request.Regulator = "GB-ENG";
            response.FeeBreakdowns = new List<FeeBreakdown>();

            // Act
            await _feeBreakdownGenerator!.GenerateFeeBreakdownAsync(response, request, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                response.FeeBreakdowns.Should().HaveCount(1);

                response.FeeBreakdowns.Should().ContainSingle(f => f.Description == "Producer Registration Fee (£1216)")
                .Which.Amount.Should().Be(121600m);

                response.FeeBreakdowns.Should().NotContain(f => f.Description.Contains("Subsidiaries Fee"));
            }
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GenerateFeeBreakdown_WhenSmallProducerWithNoSubsidiariesWithOnlineMarket_CreatesBaseFeeOnly(
            [Frozen] RegistrationFeesResponseDto response,
            [Frozen] ProducerRegistrationFeesRequestDto request)
        {
            // Arrange
            response.BaseFee = 121600m; // £1,216 in pence
            request.IsOnlineMarketplace = true;
            response.OnlineMarket = 257900m; // £2,579 in pence
            request.NumberOfSubsidiaries = 0;
            request.Regulator = "GB-ENG";
            response.FeeBreakdowns = new List<FeeBreakdown>();

            // Act
            await _feeBreakdownGenerator!.GenerateFeeBreakdownAsync(response, request, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                response.FeeBreakdowns.Should().HaveCount(2);

                response.FeeBreakdowns.Should().ContainSingle(f => f.Description == "Producer Registration Fee (£1216)")
                .Which.Amount.Should().Be(121600m);

                response.FeeBreakdowns.Should().ContainSingle(f => f.Description == "Online Marketplace Fee (£2579)")
                    .Which.Amount.Should().Be(257900m);

                response.FeeBreakdowns.Should().NotContain(f => f.Description.Contains("Subsidiaries Fee"));
            }
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GenerateFeeBreakdown_WithWholeNumberPenceWithoutOnlineMarket_CreatesCorrectBreakdown(
            [Frozen] RegistrationFeesResponseDto response,
            [Frozen] ProducerRegistrationFeesRequestDto request)
        {
            // Arrange
            response.BaseFee = 262000.0000m; // £2,620.0000 in pence
            request.IsOnlineMarketplace = false;
            request.NumberOfSubsidiaries = 10;
            request.Regulator = "GB-ENG";
            response.FeeBreakdowns = new List<FeeBreakdown>();

            _feesRepositoryMock.Setup(repo => repo.GetFirst20SubsidiariesFeeAsync(It.IsAny<RegulatorType>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(55800.0000m); // £558.0000 in pence per subsidiary

            // Act
            await _feeBreakdownGenerator!.GenerateFeeBreakdownAsync(response, request, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                response.FeeBreakdowns.Should().HaveCount(2);

                // Description should now only display whole pounds
                response.FeeBreakdowns.Should().ContainSingle(f => f.Description == "Producer Registration Fee (£2620)")
                    .Which.Amount.Should().Be(262000.0000m); // Amount remains accurate with full precision (even though it's .0000)

                // Description should now only display whole pounds
                response.FeeBreakdowns.Should().ContainSingle(f => f.Description == "First 10 Subsidiaries Fee (£558 each)")
                    .Which.Amount.Should().Be(558000.0000m); // 10 subsidiaries at £558.0000 each, amount remains accurate with full precision
            }
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GenerateFeeBreakdown_WithWholeNumberPenceWithOnlineMarket_CreatesCorrectBreakdown(
            [Frozen] RegistrationFeesResponseDto response,
            [Frozen] ProducerRegistrationFeesRequestDto request)
        {
            // Arrange
            response.BaseFee = 262000.0000m; // £2,620.0000 in pence
            request.IsOnlineMarketplace = true;
            response.OnlineMarket = 257900.0000m; // £2,579 in pence
            request.NumberOfSubsidiaries = 10;
            request.Regulator = "GB-ENG";
            response.FeeBreakdowns = new List<FeeBreakdown>();

            _feesRepositoryMock.Setup(repo => repo.GetFirst20SubsidiariesFeeAsync(It.IsAny<RegulatorType>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(55800.0000m); // £558.0000 in pence per subsidiary

            // Act
            await _feeBreakdownGenerator!.GenerateFeeBreakdownAsync(response, request, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                response.FeeBreakdowns.Should().HaveCount(3);

                // Description should now only display whole pounds
                response.FeeBreakdowns.Should().ContainSingle(f => f.Description == "Producer Registration Fee (£2620)")
                    .Which.Amount.Should().Be(262000.0000m); // Amount remains accurate with full precision (even though it's .0000)


                response.FeeBreakdowns.Should().ContainSingle(f => f.Description == "Online Marketplace Fee (£2579)")
                    .Which.Amount.Should().Be(257900.0000m);

                // Description should now only display whole pounds
                response.FeeBreakdowns.Should().ContainSingle(f => f.Description == "First 10 Subsidiaries Fee (£558 each)")
                    .Which.Amount.Should().Be(558000.0000m); // 10 subsidiaries at £558.0000 each, amount remains accurate with full precision
            }
        }
    }
}