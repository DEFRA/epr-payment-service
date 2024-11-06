using AutoFixture.MSTest;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.Producer;
using EPR.Payment.Service.Common.Dtos.Response.RegistrationFees;
using EPR.Payment.Service.Common.UnitTests.TestHelpers;
using EPR.Payment.Service.Services.Interfaces.Payments;
using EPR.Payment.Service.Services.Interfaces.RegistrationFees.Producer;
using EPR.Payment.Service.Services.RegistrationFees.Producer;
using EPR.Payment.Service.Strategies.Interfaces.RegistrationFees;
using EPR.Payment.Service.Strategies.Interfaces.RegistrationFees.Producer;
using EPR.Payment.Service.Strategies.RegistrationFees;
using FluentAssertions;
using FluentAssertions.Execution;
using FluentValidation;
using FluentValidation.Results;
using Moq;

namespace EPR.Payment.Service.UnitTests.Services.RegistrationFees.Producer
{
    [TestClass]
    public class ProducerFeesCalculatorServiceTests
    {
        private Mock<IBaseFeeCalculationStrategy<ProducerRegistrationFeesRequestDto, decimal>> _baseFeeCalculationStrategyMock = null!;
        private Mock<IOnlineMarketCalculationStrategy<ProducerRegistrationFeesRequestDto, decimal>> _onlineMarketCalculationStrategyMock = null!;
        private Mock<IBaseSubsidiariesFeeCalculationStrategy<ProducerRegistrationFeesRequestDto, SubsidiariesFeeBreakdown>> _subsidiariesFeeCalculationStrategyMock = null!;
        private Mock<ILateFeeCalculationStrategy<ProducerRegistrationFeesRequestDto, decimal>> _lateFeeCalculationStrategyMock = null!;
        private Mock<IValidator<ProducerRegistrationFeesRequestDto>> _validatorMock = null!;
        private Mock<IPaymentsService> _paymentsServiceMock = null!;
        private ProducerFeesCalculatorService? _calculatorService = null;

        [TestInitialize]
        public void TestInitialize()
        {
            _baseFeeCalculationStrategyMock = new Mock<IBaseFeeCalculationStrategy<ProducerRegistrationFeesRequestDto, decimal>>();
            _onlineMarketCalculationStrategyMock = new Mock<IOnlineMarketCalculationStrategy<ProducerRegistrationFeesRequestDto, decimal>>();
            _lateFeeCalculationStrategyMock = new Mock<ILateFeeCalculationStrategy<ProducerRegistrationFeesRequestDto, decimal>>();
            _subsidiariesFeeCalculationStrategyMock = new Mock<IBaseSubsidiariesFeeCalculationStrategy<ProducerRegistrationFeesRequestDto, SubsidiariesFeeBreakdown>>();
            _validatorMock = new Mock<IValidator<ProducerRegistrationFeesRequestDto>>();
            _paymentsServiceMock = new Mock<IPaymentsService>();

            _calculatorService = new ProducerFeesCalculatorService(
                _baseFeeCalculationStrategyMock.Object,
                _subsidiariesFeeCalculationStrategyMock.Object,
                _validatorMock.Object,
                _onlineMarketCalculationStrategyMock.Object,
                _lateFeeCalculationStrategyMock.Object,
                _paymentsServiceMock.Object
            );
        }

        [TestMethod]
        public void Constructor_WhenBaseFeeCalculationStrategyIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            IBaseFeeCalculationStrategy<ProducerRegistrationFeesRequestDto, decimal>? baseFeeCalculationStrategy = null;

            // Act
            Action act = () => new ProducerFeesCalculatorService(
                baseFeeCalculationStrategy!,
                _subsidiariesFeeCalculationStrategyMock.Object,
                _validatorMock.Object,
                _onlineMarketCalculationStrategyMock.Object,
                _lateFeeCalculationStrategyMock.Object,
                _paymentsServiceMock.Object);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'baseFeeCalculationStrategy')");
        }

        [TestMethod]
        public void Constructor_WhenSubsidiariesFeeCalculationStrategyIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            BaseSubsidiariesFeeCalculationStrategy<ProducerRegistrationFeesRequestDto>? subsidiariesFeeCalculationStrategy = null;

            // Act
            Action act = () => new ProducerFeesCalculatorService(
                _baseFeeCalculationStrategyMock.Object,
                subsidiariesFeeCalculationStrategy!,
                _validatorMock.Object,
                _onlineMarketCalculationStrategyMock.Object,
                _lateFeeCalculationStrategyMock.Object,
                _paymentsServiceMock.Object);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'subsidiariesFeeCalculationStrategy')");
        }

        [TestMethod]
        public void Constructor_WhenValidatorIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            IValidator<ProducerRegistrationFeesRequestDto>? validator = null;

            // Act
            Action act = () => new ProducerFeesCalculatorService(
                _baseFeeCalculationStrategyMock.Object,
                _subsidiariesFeeCalculationStrategyMock.Object,
                validator!,
                _onlineMarketCalculationStrategyMock.Object,
                _lateFeeCalculationStrategyMock.Object,
                _paymentsServiceMock.Object);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'validator')");
        }


        [TestMethod]
        public void Constructor_WhenOnlineMarketCalculationStrategyIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            IOnlineMarketCalculationStrategy<ProducerRegistrationFeesRequestDto, decimal>? onlineMarketCalculationStrategy = null;

            // Act
            Action act = () => new ProducerFeesCalculatorService(
                _baseFeeCalculationStrategyMock.Object,
                _subsidiariesFeeCalculationStrategyMock.Object,
                _validatorMock.Object,
                onlineMarketCalculationStrategy!,
                _lateFeeCalculationStrategyMock.Object,
                _paymentsServiceMock.Object);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'onlineMarketCalculationStrategy')");
        }

        [TestMethod]
        public void Constructor_WhenLateFeeCalculationStrategyIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            ILateFeeCalculationStrategy<ProducerRegistrationFeesRequestDto, decimal>? lateFeeCalculationStrategy = null;

            // Act
            Func<ProducerFeesCalculatorService> servie = () => new ProducerFeesCalculatorService(
                _baseFeeCalculationStrategyMock.Object,
                _subsidiariesFeeCalculationStrategyMock.Object,
                _validatorMock.Object,
                _onlineMarketCalculationStrategyMock.Object!,
                lateFeeCalculationStrategy!,
                _paymentsServiceMock.Object);

            // Assert
            servie.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'lateFeeCalculationStrategy')");
        }

        [TestMethod]
        public void Constructor_WhenPaymentServiceIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            IPaymentsService? paymentsService = null;

            // Act
            Func<ProducerFeesCalculatorService> servie = () => new ProducerFeesCalculatorService(
                _baseFeeCalculationStrategyMock.Object,
                _subsidiariesFeeCalculationStrategyMock.Object,
                _validatorMock.Object,
                _onlineMarketCalculationStrategyMock.Object!,
                _lateFeeCalculationStrategyMock.Object,
                paymentsService!);

            // Assert
            servie.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'paymentsService')");
        }

        [TestMethod]
        public void Constructor_WhenAllDependenciesAreNotNull_ShouldInitializeProducerFeesCalculatorService()
        {
            // Act
            var service = new ProducerFeesCalculatorService(
                _baseFeeCalculationStrategyMock.Object,
                _subsidiariesFeeCalculationStrategyMock.Object,
                _validatorMock.Object,
                _onlineMarketCalculationStrategyMock.Object,
                _lateFeeCalculationStrategyMock.Object,
                _paymentsServiceMock.Object);

            // Assert
            using (new AssertionScope())
            {
                service.Should().NotBeNull();
                service.Should().BeAssignableTo<IProducerFeesCalculatorService>();
            }
        }

        [TestMethod, AutoMoqData]
        public async Task CalculateFeesAsync_WhenLargeProducerWith50Subsidiaries_ReturnsCorrectFees([Frozen] SubsidiariesFeeBreakdown ExpectedSubsidiariesFeeBreakdown)
        {
            // Arrange
            var request = new ProducerRegistrationFeesRequestDto
            {
                ProducerType = "Large",
                NumberOfSubsidiaries = 50,
                Regulator = "GB-ENG",
                NoOfSubsidiariesOnlineMarketplace = 5,
                ApplicationReferenceNumber = "A123",
                IsLateFeeApplicable = false,
                SubmissionDate = DateTime.Now
            };

            _baseFeeCalculationStrategyMock.Setup(strategy => strategy.CalculateFeeAsync(It.IsAny<ProducerRegistrationFeesRequestDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(262000m); // £2,620 represented in pence

            _onlineMarketCalculationStrategyMock.Setup(strategy => strategy.CalculateFeeAsync(It.IsAny<ProducerRegistrationFeesRequestDto>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(0m); // Online Market fee in pence

            _subsidiariesFeeCalculationStrategyMock.Setup(strategy => strategy.CalculateFeeAsync(It.IsAny<ProducerRegistrationFeesRequestDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(ExpectedSubsidiariesFeeBreakdown); // Expected Subsidiaries Fee Breakdown

            _validatorMock.Setup(v => v.Validate(It.IsAny<ProducerRegistrationFeesRequestDto>()))
                .Returns(new ValidationResult());

            _paymentsServiceMock.Setup(s => s.GetPreviousPaymentsByReferenceAsync(request.ApplicationReferenceNumber, It.IsAny<CancellationToken>()))
                .ReturnsAsync(100M);

            // Act
            var result = await _calculatorService!.CalculateFeesAsync(request, CancellationToken.None);

            // Assert

            using (new AssertionScope())
            {
                result.ProducerRegistrationFee.Should().Be(262000m); // £2,620 represented in pence
                result.ProducerOnlineMarketPlaceFee.Should().Be(0m); // Online Market fee in pence
                result.ProducerLateRegistrationFee.Should().Be(0m); // Late fee in pence
                result.SubsidiariesFee.Should().Be(result.SubsidiariesFeeBreakdown.TotalSubsidiariesOMPFees + result.SubsidiariesFeeBreakdown.FeeBreakdowns.Select(i => i.TotalPrice).Sum());
                result.SubsidiariesFeeBreakdown.Should().Be(ExpectedSubsidiariesFeeBreakdown); // Expected Subsidiaries Fee Breakdown
                result.TotalFee.Should().Be(result.ProducerRegistrationFee + result.SubsidiariesFee); // Total fee in pence
                result.PreviousPayment.Should().Be(100M); 
                result.OutstandingPayment.Should().Be(result.TotalFee - result.PreviousPayment);
            }
        }

        [TestMethod, AutoMoqData]
        public async Task CalculateFeesAsync_WhenLargeProducerWith50SubsidiariesWithProducerOMP_ReturnsCorrectFees(
            [Frozen] SubsidiariesFeeBreakdown ExpectedSubsidiariesFeeBreakdown)
        {
            // Arrange
            var request = new ProducerRegistrationFeesRequestDto
            {
                ProducerType = "Large",
                NumberOfSubsidiaries = 50,
                Regulator = "GB-ENG",
                ApplicationReferenceNumber = "A123",
                IsProducerOnlineMarketplace = true,
                IsLateFeeApplicable = false,
                SubmissionDate = DateTime.Now
            };

            _baseFeeCalculationStrategyMock.Setup(strategy => strategy.CalculateFeeAsync(It.IsAny<ProducerRegistrationFeesRequestDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(262000m); // £2,620 represented in pence

            _onlineMarketCalculationStrategyMock.Setup(strategy => strategy.CalculateFeeAsync(It.IsAny<ProducerRegistrationFeesRequestDto>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(257900m); // Online Market fee in pence

            _subsidiariesFeeCalculationStrategyMock.Setup(strategy => strategy.CalculateFeeAsync(It.IsAny<ProducerRegistrationFeesRequestDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(ExpectedSubsidiariesFeeBreakdown); // Total subsidiaries fee in pence

            _validatorMock.Setup(v => v.Validate(It.IsAny<ProducerRegistrationFeesRequestDto>()))
                .Returns(new ValidationResult());

            _paymentsServiceMock.Setup(s => s.GetPreviousPaymentsByReferenceAsync(request.ApplicationReferenceNumber, It.IsAny<CancellationToken>()))
                .ReturnsAsync(100M);

            // Act
            var result = await _calculatorService!.CalculateFeesAsync(request, CancellationToken.None);

            // Assert

            using (new AssertionScope())
            {
                result.ProducerRegistrationFee.Should().Be(262000m); // £2,620 represented in pence
                result.ProducerOnlineMarketPlaceFee.Should().Be(257900m); // Online Market fee in pence
                result.ProducerLateRegistrationFee.Should().Be(0m); // Late fee in pence
                result.SubsidiariesFeeBreakdown.Should().Be(ExpectedSubsidiariesFeeBreakdown); // Expected Subsidiaries Fee Breakdown
                result.TotalFee.Should().Be(result.ProducerRegistrationFee + result.ProducerOnlineMarketPlaceFee + result.SubsidiariesFee); // Total fee in pence
                result.PreviousPayment.Should().Be(100M);
                result.OutstandingPayment.Should().Be(result.TotalFee - result.PreviousPayment);
            }
        }

        [TestMethod, AutoMoqData]
        public async Task CalculateFeesAsync_WhenLargeProducerWith10Subsidiaries_ReturnsCorrectFees([Frozen] SubsidiariesFeeBreakdown ExpectedSubsidiariesFeeBreakdown)
        {
            // Arrange
            var request = new ProducerRegistrationFeesRequestDto
            {
                ProducerType = "Large",
                NumberOfSubsidiaries = 10,
                Regulator = "GB-ENG",
                ApplicationReferenceNumber = "A123",
                IsLateFeeApplicable = false,
                SubmissionDate = DateTime.Now
            };

            _baseFeeCalculationStrategyMock.Setup(strategy => strategy.CalculateFeeAsync(It.IsAny<ProducerRegistrationFeesRequestDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(262000m); // £2,620 represented in pence

            _onlineMarketCalculationStrategyMock.Setup(strategy => strategy.CalculateFeeAsync(It.IsAny<ProducerRegistrationFeesRequestDto>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(0m); // Online Market fee in pence

            _subsidiariesFeeCalculationStrategyMock.Setup(strategy => strategy.CalculateFeeAsync(It.IsAny<ProducerRegistrationFeesRequestDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(ExpectedSubsidiariesFeeBreakdown); // Subsidiaries Fee Breakdown

            _validatorMock.Setup(v => v.Validate(It.IsAny<ProducerRegistrationFeesRequestDto>()))
                .Returns(new ValidationResult());

            _paymentsServiceMock.Setup(s => s.GetPreviousPaymentsByReferenceAsync(request.ApplicationReferenceNumber, It.IsAny<CancellationToken>()))
                .ReturnsAsync(100M);

            // Act
            var result = await _calculatorService!.CalculateFeesAsync(request, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                result.ProducerRegistrationFee.Should().Be(262000m); // £2,620 represented in pence
                result.ProducerOnlineMarketPlaceFee.Should().Be(0m); // Online Market fee in pence
                result.ProducerLateRegistrationFee.Should().Be(0m); // Late fee in pence
                result.SubsidiariesFeeBreakdown.Should().Be(ExpectedSubsidiariesFeeBreakdown); // Expected Subsidiaries Fee Breakdown
                result.TotalFee.Should().Be(result.ProducerRegistrationFee + result.SubsidiariesFee); // Total fee in pence
                result.PreviousPayment.Should().Be(100M);
                result.OutstandingPayment.Should().Be(result.TotalFee - result.PreviousPayment);
            }
        }
        [TestMethod, AutoMoqData]
        public async Task CalculateFeesAsync_WhenLargeProducerWith10SubsidiariesWithProducerOMP_ReturnsCorrectFees(
            [Frozen] SubsidiariesFeeBreakdown ExpectedSubsidiariesFeeBreakdown)
        {
            // Arrange
            var request = new ProducerRegistrationFeesRequestDto
            {
                ProducerType = "Large",
                NumberOfSubsidiaries = 10,
                Regulator = "GB-ENG",
                ApplicationReferenceNumber = "A123",
                IsProducerOnlineMarketplace = true,
                IsLateFeeApplicable = false,
                SubmissionDate = DateTime.Now
            };

            _baseFeeCalculationStrategyMock.Setup(strategy => strategy.CalculateFeeAsync(It.IsAny<ProducerRegistrationFeesRequestDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(262000m); // £2,620 represented in pence

            _onlineMarketCalculationStrategyMock.Setup(strategy => strategy.CalculateFeeAsync(It.IsAny<ProducerRegistrationFeesRequestDto>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(257900m); // Online Market fee in pence

            _subsidiariesFeeCalculationStrategyMock.Setup(strategy => strategy.CalculateFeeAsync(It.IsAny<ProducerRegistrationFeesRequestDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(ExpectedSubsidiariesFeeBreakdown); // Total subsidiaries fee in pence

            _validatorMock.Setup(v => v.Validate(It.IsAny<ProducerRegistrationFeesRequestDto>()))
                .Returns(new ValidationResult());

            _paymentsServiceMock.Setup(s => s.GetPreviousPaymentsByReferenceAsync(request.ApplicationReferenceNumber, It.IsAny<CancellationToken>()))
                .ReturnsAsync(100M);

            // Act
            var result = await _calculatorService!.CalculateFeesAsync(request, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                result.ProducerRegistrationFee.Should().Be(262000m); // £2,620 represented in pence
                result.ProducerOnlineMarketPlaceFee.Should().Be(257900m); // Online Market fee in pence
                result.ProducerLateRegistrationFee.Should().Be(0m); // Late fee in pence
                result.SubsidiariesFeeBreakdown.Should().Be(ExpectedSubsidiariesFeeBreakdown); // Expected Subsidiaries Fee Breakdown
                result.TotalFee.Should().Be(result.ProducerRegistrationFee + result.ProducerOnlineMarketPlaceFee + result.SubsidiariesFee); // Total fee in pence
                result.PreviousPayment.Should().Be(100M);
                result.OutstandingPayment.Should().Be(result.TotalFee - result.PreviousPayment);
            }
        }

        [TestMethod, AutoMoqData]
        public async Task CalculateFeesAsync_WhenLargeProducerWithNoBaseFeeAnd50Subsidiaries_ReturnsCorrectFees(
            [Frozen] SubsidiariesFeeBreakdown ExpectedSubsidiariesFeeBreakdown)
        {
            // Arrange
            var request = new ProducerRegistrationFeesRequestDto
            {
                ProducerType = "",// ProducerType is empty to indicate no base fee
                NumberOfSubsidiaries = 50,
                Regulator = "GB-ENG",
                ApplicationReferenceNumber = "A123",
                IsLateFeeApplicable = false,
                SubmissionDate = DateTime.Now
            };

            _baseFeeCalculationStrategyMock.Setup(strategy => strategy.CalculateFeeAsync(It.IsAny<ProducerRegistrationFeesRequestDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(0m); // No base fee for any ProducerType

            _onlineMarketCalculationStrategyMock.Setup(strategy => strategy.CalculateFeeAsync(It.IsAny<ProducerRegistrationFeesRequestDto>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(0m); // Online Market fee in pence

            _subsidiariesFeeCalculationStrategyMock.Setup(strategy => strategy.CalculateFeeAsync(It.IsAny<ProducerRegistrationFeesRequestDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(ExpectedSubsidiariesFeeBreakdown); // Total subsidiaries fee in pence

            _validatorMock.Setup(v => v.Validate(It.IsAny<ProducerRegistrationFeesRequestDto>()))
                .Returns(new ValidationResult());

            _paymentsServiceMock.Setup(s => s.GetPreviousPaymentsByReferenceAsync(request.ApplicationReferenceNumber, It.IsAny<CancellationToken>()))
                .ReturnsAsync(100M);

            // Act
            var result = await _calculatorService!.CalculateFeesAsync(request, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                result.ProducerRegistrationFee.Should().Be(0m); // No base fee
                result.ProducerOnlineMarketPlaceFee.Should().Be(0m); // Online Market fee in pence
                result.ProducerLateRegistrationFee.Should().Be(0m); // Late fee in pence
                result.SubsidiariesFeeBreakdown.Should().Be(ExpectedSubsidiariesFeeBreakdown); // Expected Subsidiaries Fee Breakdown
                result.TotalFee.Should().Be(result.ProducerRegistrationFee + result.SubsidiariesFee); // Total fee in pence
                result.PreviousPayment.Should().Be(100M);
                result.OutstandingPayment.Should().Be(result.TotalFee - result.PreviousPayment);
            }
        }

        [TestMethod, AutoMoqData]
        public async Task CalculateFeesAsync_WhenLargeProducerWithNoBaseFeeAnd50SubsidiariesWithProducerOMP_ReturnsCorrectFees(
            [Frozen] SubsidiariesFeeBreakdown ExpectedSubsidiariesFeeBreakdown)
        {
            // Arrange
            var request = new ProducerRegistrationFeesRequestDto
            {
                ProducerType = "",// ProducerType is empty to indicate no base fee
                NumberOfSubsidiaries = 50,
                Regulator = "GB-ENG",
                ApplicationReferenceNumber = "A123",
                IsProducerOnlineMarketplace = true,
                IsLateFeeApplicable = false,
                SubmissionDate = DateTime.Now
            };

            _baseFeeCalculationStrategyMock.Setup(strategy => strategy.CalculateFeeAsync(It.IsAny<ProducerRegistrationFeesRequestDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(0m); // No base fee for any ProducerType

            _onlineMarketCalculationStrategyMock.Setup(strategy => strategy.CalculateFeeAsync(It.IsAny<ProducerRegistrationFeesRequestDto>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(257900m); // Online Market fee in pence

            _subsidiariesFeeCalculationStrategyMock.Setup(strategy => strategy.CalculateFeeAsync(It.IsAny<ProducerRegistrationFeesRequestDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(ExpectedSubsidiariesFeeBreakdown); // Total subsidiaries fee in pence

            _validatorMock.Setup(v => v.Validate(It.IsAny<ProducerRegistrationFeesRequestDto>()))
                .Returns(new ValidationResult());

            _paymentsServiceMock.Setup(s => s.GetPreviousPaymentsByReferenceAsync(request.ApplicationReferenceNumber, It.IsAny<CancellationToken>()))
                .ReturnsAsync(100M);

            // Act
            var result = await _calculatorService!.CalculateFeesAsync(request, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                result.ProducerRegistrationFee.Should().Be(0m); // No base fee
                result.ProducerOnlineMarketPlaceFee.Should().Be(257900m); // Online Market fee in pence
                result.ProducerLateRegistrationFee.Should().Be(0m); // Late fee in pence
                result.SubsidiariesFeeBreakdown.Should().Be(ExpectedSubsidiariesFeeBreakdown); // Expected Subsidiaries Fee Breakdown
                result.TotalFee.Should().Be(result.ProducerRegistrationFee + result.ProducerOnlineMarketPlaceFee + result.SubsidiariesFee); // Total fee in pence
                result.PreviousPayment.Should().Be(100M);
                result.OutstandingPayment.Should().Be(result.TotalFee - result.PreviousPayment);
            }
        }

        [TestMethod, AutoMoqData]
        public async Task CalculateFeesAsync_WhenSmallProducerWith25Subsidiaries_ReturnsCorrectFees(
            [Frozen] SubsidiariesFeeBreakdown ExpectedSubsidiariesFeeBreakdown)
        {
            // Arrange
            var request = new ProducerRegistrationFeesRequestDto
            {
                ProducerType = "Small",
                NumberOfSubsidiaries = 25,
                Regulator = "GB-ENG",
                ApplicationReferenceNumber = "A123",
                IsLateFeeApplicable = false,
                SubmissionDate = DateTime.Now
            };

            _baseFeeCalculationStrategyMock.Setup(strategy => strategy.CalculateFeeAsync(It.IsAny<ProducerRegistrationFeesRequestDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(121600m); // £1,216 represented in pence

            _onlineMarketCalculationStrategyMock.Setup(strategy => strategy.CalculateFeeAsync(It.IsAny<ProducerRegistrationFeesRequestDto>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(0m); // Online Market fee in pence

            _subsidiariesFeeCalculationStrategyMock.Setup(strategy => strategy.CalculateFeeAsync(It.IsAny<ProducerRegistrationFeesRequestDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(ExpectedSubsidiariesFeeBreakdown); // Total subsidiaries fee in pence

            _validatorMock.Setup(v => v.Validate(It.IsAny<ProducerRegistrationFeesRequestDto>()))
                .Returns(new ValidationResult());

            _paymentsServiceMock.Setup(s => s.GetPreviousPaymentsByReferenceAsync(request.ApplicationReferenceNumber, It.IsAny<CancellationToken>()))
                .ReturnsAsync(100M);

            // Act
            var result = await _calculatorService!.CalculateFeesAsync(request, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                result.ProducerRegistrationFee.Should().Be(121600m); // £1,216 represented in pence
                result.ProducerOnlineMarketPlaceFee.Should().Be(0m); // Online Market fee in pence
                result.ProducerLateRegistrationFee.Should().Be(0m); // Late fee in pence
                result.SubsidiariesFeeBreakdown.Should().Be(ExpectedSubsidiariesFeeBreakdown); // Expected Subsidiaries Fee Breakdown
                result.TotalFee.Should().Be(result.ProducerRegistrationFee + result.SubsidiariesFee); // Total fee in pence
                result.PreviousPayment.Should().Be(100M);
                result.OutstandingPayment.Should().Be(result.TotalFee - result.PreviousPayment);
            }
        }

        [TestMethod, AutoMoqData]
        public async Task CalculateFeesAsync_WhenSmallProducerWith25SubsidiariesWithProducerOMP_ReturnsCorrectFees(
            [Frozen] SubsidiariesFeeBreakdown ExpectedSubsidiariesFeeBreakdown)
        {
            // Arrange
            var request = new ProducerRegistrationFeesRequestDto
            {
                ProducerType = "Small",
                NumberOfSubsidiaries = 25,
                Regulator = "GB-ENG",
                ApplicationReferenceNumber = "A123",
                IsProducerOnlineMarketplace = true,
                IsLateFeeApplicable = false,
                SubmissionDate = DateTime.Now
            };

            _baseFeeCalculationStrategyMock.Setup(strategy => strategy.CalculateFeeAsync(It.IsAny<ProducerRegistrationFeesRequestDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(121600m); // £1,216 represented in pence

            _onlineMarketCalculationStrategyMock.Setup(strategy => strategy.CalculateFeeAsync(It.IsAny<ProducerRegistrationFeesRequestDto>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(257900m); // Online Market fee in pence

            _subsidiariesFeeCalculationStrategyMock.Setup(strategy => strategy.CalculateFeeAsync(It.IsAny<ProducerRegistrationFeesRequestDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(ExpectedSubsidiariesFeeBreakdown); // Total subsidiaries fee in pence

            _validatorMock.Setup(v => v.Validate(It.IsAny<ProducerRegistrationFeesRequestDto>()))
                .Returns(new ValidationResult());

            _paymentsServiceMock.Setup(s => s.GetPreviousPaymentsByReferenceAsync(request.ApplicationReferenceNumber, It.IsAny<CancellationToken>()))
                .ReturnsAsync(100M);

            // Act
            var result = await _calculatorService!.CalculateFeesAsync(request, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                result.ProducerRegistrationFee.Should().Be(121600m); // £1,216 represented in pence
                result.ProducerOnlineMarketPlaceFee.Should().Be(257900m); // Online Market fee in pence
                result.ProducerLateRegistrationFee.Should().Be(0m); // Late fee in pence
                result.SubsidiariesFeeBreakdown.Should().Be(ExpectedSubsidiariesFeeBreakdown); // Expected Subsidiaries Fee Breakdown
                result.TotalFee.Should().Be(result.ProducerRegistrationFee + result.ProducerOnlineMarketPlaceFee + result.SubsidiariesFee); // Total fee in pence
                result.PreviousPayment.Should().Be(100M);
                result.OutstandingPayment.Should().Be(result.TotalFee - result.PreviousPayment);
            }
        }

        [TestMethod, AutoMoqData]
        public async Task CalculateFeesAsync_WhenSmallProducerWith20Subsidiaries_ReturnsCorrectFees(
            [Frozen] SubsidiariesFeeBreakdown ExpectedSubsidiariesFeeBreakdown)
        {
            // Arrange
            var request = new ProducerRegistrationFeesRequestDto
            {
                ProducerType = "Small",
                NumberOfSubsidiaries = 20,
                Regulator = "GB-ENG",
                ApplicationReferenceNumber = "A123",
                IsLateFeeApplicable = false,
                SubmissionDate = DateTime.Now
            };

            request.ProducerType = "Small";
            request.NumberOfSubsidiaries = 20;
            request.Regulator = "GB-ENG";

            _baseFeeCalculationStrategyMock.Setup(strategy => strategy.CalculateFeeAsync(It.IsAny<ProducerRegistrationFeesRequestDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(121600m); // £1,216 represented in pence

            _onlineMarketCalculationStrategyMock.Setup(strategy => strategy.CalculateFeeAsync(It.IsAny<ProducerRegistrationFeesRequestDto>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(0m); // Online Market fee in pence

            _subsidiariesFeeCalculationStrategyMock.Setup(strategy => strategy.CalculateFeeAsync(It.IsAny<ProducerRegistrationFeesRequestDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(ExpectedSubsidiariesFeeBreakdown); // Total subsidiaries fee in pence for 20 subsidiaries

            _validatorMock.Setup(v => v.Validate(It.IsAny<ProducerRegistrationFeesRequestDto>()))
                .Returns(new ValidationResult());

            _paymentsServiceMock.Setup(s => s.GetPreviousPaymentsByReferenceAsync(request.ApplicationReferenceNumber, It.IsAny<CancellationToken>()))
                .ReturnsAsync(100M);

            // Act
            var result = await _calculatorService!.CalculateFeesAsync(request, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                result.ProducerRegistrationFee.Should().Be(121600m); // £1,216 represented in pence
                result.ProducerOnlineMarketPlaceFee.Should().Be(0m); // Online Market fee in pence
                result.ProducerLateRegistrationFee.Should().Be(0m); // Late fee in pence
                result.SubsidiariesFeeBreakdown.Should().Be(ExpectedSubsidiariesFeeBreakdown); // Expected Subsidiaries Fee Breakdown
                result.TotalFee.Should().Be(result.ProducerRegistrationFee + result.SubsidiariesFee); // Total fee in pence
                result.PreviousPayment.Should().Be(100M);
                result.OutstandingPayment.Should().Be(result.TotalFee - result.PreviousPayment);
            }
        }

        [TestMethod, AutoMoqData]
        public async Task CalculateFeesAsync_WhenSmallProducerWith20SubsidiariesWithProducerOMP_ReturnsCorrectFees(
            [Frozen] SubsidiariesFeeBreakdown ExpectedSubsidiariesFeeBreakdown)
        {
            // Arrange
            var request = new ProducerRegistrationFeesRequestDto
            {
                ProducerType = "Small",
                NumberOfSubsidiaries = 20,
                Regulator = "GB-ENG",
                ApplicationReferenceNumber = "A123",
                IsProducerOnlineMarketplace = true,
                IsLateFeeApplicable = false,
                SubmissionDate = DateTime.Now
            };

            _baseFeeCalculationStrategyMock.Setup(strategy => strategy.CalculateFeeAsync(It.IsAny<ProducerRegistrationFeesRequestDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(121600m); // £1,216 represented in pence

            _onlineMarketCalculationStrategyMock.Setup(strategy => strategy.CalculateFeeAsync(It.IsAny<ProducerRegistrationFeesRequestDto>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(257900m); // Online Market fee in pence

            _subsidiariesFeeCalculationStrategyMock.Setup(strategy => strategy.CalculateFeeAsync(It.IsAny<ProducerRegistrationFeesRequestDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(ExpectedSubsidiariesFeeBreakdown); // Total subsidiaries fee in pence for 20 subsidiaries

            _validatorMock.Setup(v => v.Validate(It.IsAny<ProducerRegistrationFeesRequestDto>()))
                .Returns(new ValidationResult());

            _paymentsServiceMock.Setup(s => s.GetPreviousPaymentsByReferenceAsync(request.ApplicationReferenceNumber, It.IsAny<CancellationToken>()))
                .ReturnsAsync(100M);

            // Act
            var result = await _calculatorService!.CalculateFeesAsync(request, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                result.ProducerRegistrationFee.Should().Be(121600m); // £1,216 represented in pence
                result.ProducerOnlineMarketPlaceFee.Should().Be(257900m); // Online Market fee in pence
                result.ProducerLateRegistrationFee.Should().Be(0m); // Late fee in pence
                result.SubsidiariesFeeBreakdown.Should().Be(ExpectedSubsidiariesFeeBreakdown); // Expected Subsidiaries Fee Breakdown
                result.TotalFee.Should().Be(result.ProducerRegistrationFee + result.ProducerOnlineMarketPlaceFee + result.SubsidiariesFee); // Total fee in pence
                result.PreviousPayment.Should().Be(100M);
                result.OutstandingPayment.Should().Be(result.TotalFee - result.PreviousPayment);
            }
        }

        [TestMethod, AutoMoqData]
        public async Task CalculateFeesAsync_WhenLargeProducerWithNoSubsidiaries_ReturnsBaseFeeOnly(
            [Frozen] SubsidiariesFeeBreakdown ExpectedSubsidiariesFeeBreakdown)
        {
            // Arrange
            var request = new ProducerRegistrationFeesRequestDto
            {
                ProducerType = "Large",
                NumberOfSubsidiaries = 0,
                Regulator = "GB-ENG",
                ApplicationReferenceNumber = "A123",
                IsLateFeeApplicable = false,
                SubmissionDate = DateTime.Now
            };

            _baseFeeCalculationStrategyMock.Setup(strategy => strategy.CalculateFeeAsync(It.IsAny<ProducerRegistrationFeesRequestDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(262000m); // £2,620 represented in pence

            _onlineMarketCalculationStrategyMock.Setup(strategy => strategy.CalculateFeeAsync(It.IsAny<ProducerRegistrationFeesRequestDto>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(0m); // Online Market fee in pence

            _subsidiariesFeeCalculationStrategyMock.Setup(strategy => strategy.CalculateFeeAsync(It.IsAny<ProducerRegistrationFeesRequestDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(ExpectedSubsidiariesFeeBreakdown); // Expected Subsidiaries Fee Breakdown

            _validatorMock.Setup(v => v.Validate(It.IsAny<ProducerRegistrationFeesRequestDto>()))
                .Returns(new ValidationResult());

            _paymentsServiceMock.Setup(s => s.GetPreviousPaymentsByReferenceAsync(request.ApplicationReferenceNumber, It.IsAny<CancellationToken>()))
                .ReturnsAsync(100M);

            // Act
            var result = await _calculatorService!.CalculateFeesAsync(request, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                result.ProducerRegistrationFee.Should().Be(262000m); // £2,620 represented in pence
                result.ProducerOnlineMarketPlaceFee.Should().Be(0m); // Online Market fee in pence
                result.ProducerLateRegistrationFee.Should().Be(0m); // Late fee in pence
                result.SubsidiariesFeeBreakdown.Should().Be(ExpectedSubsidiariesFeeBreakdown); // Expected Subsidiaries Fee Breakdown
                result.TotalFee.Should().Be(result.ProducerRegistrationFee + result.SubsidiariesFee); // Total fee in pence
                result.PreviousPayment.Should().Be(100M);
                result.OutstandingPayment.Should().Be(result.TotalFee - result.PreviousPayment);
            }
        }


        [TestMethod, AutoMoqData]
        public async Task CalculateFeesAsync_WhenLargeProducerWithNoSubsidiariesWithProducerOMP_ReturnsBaseFeeOnly(
            [Frozen] SubsidiariesFeeBreakdown ExpectedSubsidiariesFeeBreakdown)
        {
            // Arrange
            var request = new ProducerRegistrationFeesRequestDto
            {
                ProducerType = "Large",
                NumberOfSubsidiaries = 0,
                Regulator = "GB-ENG",
                ApplicationReferenceNumber = "A123",
                IsProducerOnlineMarketplace = true,
                IsLateFeeApplicable = false,
                SubmissionDate = DateTime.Now
            };

            _baseFeeCalculationStrategyMock.Setup(strategy => strategy.CalculateFeeAsync(It.IsAny<ProducerRegistrationFeesRequestDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(262000m); // £2,620 represented in pence

            _onlineMarketCalculationStrategyMock.Setup(strategy => strategy.CalculateFeeAsync(It.IsAny<ProducerRegistrationFeesRequestDto>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(257900m); // Online Market fee in pence

            _subsidiariesFeeCalculationStrategyMock.Setup(strategy => strategy.CalculateFeeAsync(It.IsAny<ProducerRegistrationFeesRequestDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(ExpectedSubsidiariesFeeBreakdown); // Expected Subsidiaries Fee Breakdown

            _validatorMock.Setup(v => v.Validate(It.IsAny<ProducerRegistrationFeesRequestDto>()))
                .Returns(new ValidationResult());

            _paymentsServiceMock.Setup(s => s.GetPreviousPaymentsByReferenceAsync(request.ApplicationReferenceNumber, It.IsAny<CancellationToken>()))
                .ReturnsAsync(100M);

            // Act
            var result = await _calculatorService!.CalculateFeesAsync(request, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                result.ProducerRegistrationFee.Should().Be(262000m); // £2,620 represented in pence
                result.ProducerOnlineMarketPlaceFee.Should().Be(257900m); // Online Market fee in pence
                result.ProducerLateRegistrationFee.Should().Be(0m); // Late fee in pence
                result.SubsidiariesFeeBreakdown.Should().Be(ExpectedSubsidiariesFeeBreakdown); // Expected Subsidiaries Fee Breakdown
                result.TotalFee.Should().Be(result.ProducerRegistrationFee + result.ProducerOnlineMarketPlaceFee + result.SubsidiariesFee); // Total fee in pence
                result.PreviousPayment.Should().Be(100M);
                result.OutstandingPayment.Should().Be(result.TotalFee - result.PreviousPayment);
            }
        }

        [TestMethod, AutoMoqData]
        public async Task CalculateFeesAsync_WhenInvalidRequest_ThrowsValidationException(
            [Frozen] ProducerRegistrationFeesRequestDto request)
        {
            // Arrange
            request.ProducerType = null!;
            request.NumberOfSubsidiaries = -1;
            request.Regulator = null!;

            var validationFailures = new[]
            {
                new ValidationFailure("ProducerType", "ProducerType is required"),
                new ValidationFailure("NumberOfSubsidiaries", "Number of Subsidiaries must be greater than or equal to 0"),
                new ValidationFailure("Regulator", "Regulator is required")
            };

            _validatorMock.Setup(v => v.Validate(It.IsAny<ProducerRegistrationFeesRequestDto>()))
                .Returns(new ValidationResult(validationFailures));

            // Act & Assert
            await Assert.ThrowsExceptionAsync<ValidationException>(async () =>
            {
                await _calculatorService!.CalculateFeesAsync(request, CancellationToken.None);
            });
        }

        [TestMethod, AutoMoqData]
        public async Task CalculateFeesAsync_WhenArgumentExceptionOccurs_ThrowsInvalidOperationException(
            [Frozen] ProducerRegistrationFeesRequestDto request)
        {
            // Arrange
            _baseFeeCalculationStrategyMock.Setup(strategy => strategy.CalculateFeeAsync(It.IsAny<ProducerRegistrationFeesRequestDto>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new ArgumentException("Invalid argument"));

            _validatorMock.Setup(v => v.Validate(It.IsAny<ProducerRegistrationFeesRequestDto>()))
                .Returns(new ValidationResult());

            // Act & Assert
            var exception = await Assert.ThrowsExceptionAsync<InvalidOperationException>(async () =>
            {
                await _calculatorService!.CalculateFeesAsync(request, CancellationToken.None);
            });

            exception.Message.Should().Be("An error occurred while calculating fees.");
            exception.InnerException.Should().BeOfType<ArgumentException>();
        }

        [TestMethod, AutoMoqData]
        public async Task CalculateFeesAsync_WhenNoExceptionOccurs_CallsGenerateFeeBreakdown(
            [Frozen] SubsidiariesFeeBreakdown ExpectedSubsidiariesFeeBreakdown)
        {
            // Arrange
            var request = new ProducerRegistrationFeesRequestDto
            {
                ProducerType = "Large",
                NumberOfSubsidiaries = 10,
                Regulator = "GB-ENG",
                ApplicationReferenceNumber = "A123",
                IsLateFeeApplicable = false,
                SubmissionDate = DateTime.Now
            };

            _baseFeeCalculationStrategyMock.Setup(strategy => strategy.CalculateFeeAsync(It.IsAny<ProducerRegistrationFeesRequestDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(262000m); // £2,620 represented in pence

            _subsidiariesFeeCalculationStrategyMock.Setup(strategy => strategy.CalculateFeeAsync(It.IsAny<ProducerRegistrationFeesRequestDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(ExpectedSubsidiariesFeeBreakdown); // Total subsidiaries fee in pence

            _validatorMock.Setup(v => v.Validate(It.IsAny<ProducerRegistrationFeesRequestDto>()))
                .Returns(new ValidationResult());

            _paymentsServiceMock.Setup(s => s.GetPreviousPaymentsByReferenceAsync(request.ApplicationReferenceNumber, It.IsAny<CancellationToken>()))
                .ReturnsAsync(100M);

            // Act
            var result = await _calculatorService!.CalculateFeesAsync(request, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                result.ProducerRegistrationFee.Should().Be(262000m); // £2,620 represented in pence
                result.ProducerOnlineMarketPlaceFee.Should().Be(0m); // Online Market fee in pence
                result.ProducerLateRegistrationFee.Should().Be(0m); // Late fee in pence
                result.SubsidiariesFeeBreakdown.Should().Be(ExpectedSubsidiariesFeeBreakdown); // Expected Subsidiaries Fee Breakdown
                result.TotalFee.Should().Be(result.ProducerRegistrationFee + result.SubsidiariesFee); // Total fee in pence
                result.PreviousPayment.Should().Be(100M);
                result.OutstandingPayment.Should().Be(result.TotalFee - result.PreviousPayment);
            }
        }

        [TestMethod, AutoMoqData]
        public async Task CalculateFeesAsync_WhenLateFeeIsApplicableWith0Subscribers_AddProducerRegistratonLateFee(
            [Frozen] SubsidiariesFeeBreakdown ExpectedSubsidiariesFeeBreakdown)
        {
            // Arrange
            var request = new ProducerRegistrationFeesRequestDto
            {
                ProducerType = "Large",
                NumberOfSubsidiaries = 0,
                Regulator = "GB-ENG",
                ApplicationReferenceNumber = "A123",
                IsLateFeeApplicable = true,
                SubmissionDate = DateTime.Now
            };

            _baseFeeCalculationStrategyMock.Setup(strategy => strategy.CalculateFeeAsync(It.IsAny<ProducerRegistrationFeesRequestDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(262000m); // £2,620 represented in pence

            _lateFeeCalculationStrategyMock.Setup(strategy => strategy.CalculateFeeAsync(It.IsAny<ProducerRegistrationFeesRequestDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(33200m); // £332 represented in pence

            _subsidiariesFeeCalculationStrategyMock.Setup(strategy => strategy.CalculateFeeAsync(It.IsAny<ProducerRegistrationFeesRequestDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(ExpectedSubsidiariesFeeBreakdown); // Total subsidiaries fee in pence

            _validatorMock.Setup(v => v.Validate(It.IsAny<ProducerRegistrationFeesRequestDto>()))
                .Returns(new ValidationResult());

            _paymentsServiceMock.Setup(s => s.GetPreviousPaymentsByReferenceAsync(request.ApplicationReferenceNumber, It.IsAny<CancellationToken>()))
                .ReturnsAsync(100M);

            // Act
            var result = await _calculatorService!.CalculateFeesAsync(request, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                result.ProducerRegistrationFee.Should().Be(262000m); // £2,620 represented in pence
                result.ProducerOnlineMarketPlaceFee.Should().Be(0m); // Online Market fee in pence
                result.ProducerLateRegistrationFee.Should().Be(33200m); // Late fee in pence
                result.SubsidiariesFeeBreakdown.Should().Be(ExpectedSubsidiariesFeeBreakdown); // Expected Subsidiaries Fee Breakdown
                result.TotalFee.Should().Be(result.ProducerRegistrationFee + result.SubsidiariesFee + result.ProducerLateRegistrationFee); // Total fee in pence
                result.PreviousPayment.Should().Be(100M);
                result.OutstandingPayment.Should().Be(result.TotalFee - result.PreviousPayment);
            }
        }

        [TestMethod, AutoMoqData]
        public async Task CalculateFeesAsync_WhenLateFeeIsApplicable_AddProducerRegistratonLateFee(
            [Frozen] SubsidiariesFeeBreakdown ExpectedSubsidiariesFeeBreakdown)
        {
            // Arrange
            var request = new ProducerRegistrationFeesRequestDto
            {
                ProducerType = "Large",
                NumberOfSubsidiaries = 10,
                Regulator = "GB-ENG",
                ApplicationReferenceNumber = "A123",
                IsLateFeeApplicable = true,
                SubmissionDate = DateTime.Now
            };

            _baseFeeCalculationStrategyMock.Setup(strategy => strategy.CalculateFeeAsync(It.IsAny<ProducerRegistrationFeesRequestDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(262000m); // £2,620 represented in pence

            _lateFeeCalculationStrategyMock.Setup(strategy => strategy.CalculateFeeAsync(It.IsAny<ProducerRegistrationFeesRequestDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(33200m); // £332 represented in pence

            _subsidiariesFeeCalculationStrategyMock.Setup(strategy => strategy.CalculateFeeAsync(It.IsAny<ProducerRegistrationFeesRequestDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(ExpectedSubsidiariesFeeBreakdown); // Total subsidiaries fee in pence

            _validatorMock.Setup(v => v.Validate(It.IsAny<ProducerRegistrationFeesRequestDto>()))
                .Returns(new ValidationResult());

            _paymentsServiceMock.Setup(s => s.GetPreviousPaymentsByReferenceAsync(request.ApplicationReferenceNumber, It.IsAny<CancellationToken>()))
                .ReturnsAsync(100M);

            // Act
            var result = await _calculatorService!.CalculateFeesAsync(request, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                result.ProducerRegistrationFee.Should().Be(262000m); // £2,620 represented in pence
                result.ProducerOnlineMarketPlaceFee.Should().Be(0m); // Online Market fee in pence
                result.ProducerLateRegistrationFee.Should().Be(365200m); // Late fee in pence
                result.SubsidiariesFeeBreakdown.Should().Be(ExpectedSubsidiariesFeeBreakdown); // Expected Subsidiaries Fee Breakdown
                result.TotalFee.Should().Be(result.ProducerRegistrationFee + result.SubsidiariesFee + result.ProducerLateRegistrationFee); // Total fee in pence
                result.PreviousPayment.Should().Be(100M);
                result.OutstandingPayment.Should().Be(result.TotalFee - result.PreviousPayment);
            }
        }

        [TestMethod, AutoMoqData]
        public async Task CalculateFeesAsync_WhenLateFeeIsNotApplicable_DoNotAddProducerRegistratonLateFee(
            [Frozen] SubsidiariesFeeBreakdown ExpectedSubsidiariesFeeBreakdown)
        {
            // Arrange
            var request = new ProducerRegistrationFeesRequestDto
            {
                ProducerType = "Large",
                NumberOfSubsidiaries = 10,
                Regulator = "GB-ENG",
                ApplicationReferenceNumber = "A123",
                IsLateFeeApplicable = false,
                SubmissionDate = DateTime.Now
            };

            _baseFeeCalculationStrategyMock.Setup(strategy => strategy.CalculateFeeAsync(It.IsAny<ProducerRegistrationFeesRequestDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(262000m); // £2,620 represented in pence

            _lateFeeCalculationStrategyMock.Setup(strategy => strategy.CalculateFeeAsync(It.IsAny<ProducerRegistrationFeesRequestDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(0m); // £332 represented in pence

            _subsidiariesFeeCalculationStrategyMock.Setup(strategy => strategy.CalculateFeeAsync(It.IsAny<ProducerRegistrationFeesRequestDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(ExpectedSubsidiariesFeeBreakdown); // Total subsidiaries fee in pence

            _validatorMock.Setup(v => v.Validate(It.IsAny<ProducerRegistrationFeesRequestDto>()))
                .Returns(new ValidationResult());

            _paymentsServiceMock.Setup(s => s.GetPreviousPaymentsByReferenceAsync(request.ApplicationReferenceNumber, It.IsAny<CancellationToken>()))
                .ReturnsAsync(100M);

            // Act
            var result = await _calculatorService!.CalculateFeesAsync(request, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                result.ProducerRegistrationFee.Should().Be(262000m); // £2,620 represented in pence
                result.ProducerOnlineMarketPlaceFee.Should().Be(0m); // Online Market fee in pence
                result.ProducerLateRegistrationFee.Should().Be(0m); // Late fee in pence
                result.SubsidiariesFeeBreakdown.Should().Be(ExpectedSubsidiariesFeeBreakdown); // Expected Subsidiaries Fee Breakdown
                result.TotalFee.Should().Be(result.ProducerRegistrationFee + result.SubsidiariesFee + result.ProducerLateRegistrationFee); // Total fee in pence
                result.PreviousPayment.Should().Be(100M);
                result.OutstandingPayment.Should().Be(result.TotalFee - result.PreviousPayment);
            }
        }
    }
}
