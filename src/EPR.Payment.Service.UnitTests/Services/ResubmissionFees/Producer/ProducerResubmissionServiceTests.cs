using AutoFixture.MSTest;
using EPR.Payment.Service.Common.Dtos.Request.ResubmissionFees.Producer;
using EPR.Payment.Service.Common.UnitTests.TestHelpers;
using EPR.Payment.Service.Services.Interfaces.Payments;
using EPR.Payment.Service.Services.Interfaces.ResubmissionFees.Producer;
using EPR.Payment.Service.Services.ResubmissionFees.Producer;
using EPR.Payment.Service.Strategies.Interfaces.ResubmissionFees.Producer;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;

namespace EPR.Payment.Service.UnitTests.Services.ResubmissionFees.Producer
{
    [TestClass]
    public class ProducerResubmissionServiceTests
    {
        private Mock<IResubmissionAmountStrategy<ProducerResubmissionFeeRequestDto, decimal>> _resubmissionAmountStrategyMock = null!;
        private Mock<IPaymentsService> _paymentsServiceMock = null!;
        private ProducerResubmissionService _resubmissionService = null!;
        private CancellationToken _cancellationToken;

        [TestInitialize]
        public void TestInitialize()
        {
            _resubmissionAmountStrategyMock = new Mock<IResubmissionAmountStrategy<ProducerResubmissionFeeRequestDto, decimal>>();
            _paymentsServiceMock = new Mock<IPaymentsService>();

            _resubmissionService = new ProducerResubmissionService(
                _resubmissionAmountStrategyMock.Object,
                _paymentsServiceMock.Object
            );

            _cancellationToken = new CancellationToken();
        }

        [TestMethod]
        public void Constructor_WhenAllDependenciesAreNotNull_ShouldInitializeProducerResubmissionService()
        {
            // Act
            var service = new ProducerResubmissionService(
                _resubmissionAmountStrategyMock.Object,
                _paymentsServiceMock.Object
            );

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
            IResubmissionAmountStrategy<ProducerResubmissionFeeRequestDto, decimal>? resubmissionAmountStrategy = null;

            // Act &  Assert
            Assert.ThrowsException<ArgumentNullException>(() => new ProducerResubmissionService(
                resubmissionAmountStrategy!,
                _paymentsServiceMock.Object
            ));
        }

        [TestMethod]
        public void Constructor_WhenPaymentsServiceIsNull_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() => new ProducerResubmissionService(
                _resubmissionAmountStrategyMock.Object,
                null!
            ));
        }

        [TestMethod, AutoMoqData]
        public async Task GetResubmissionFeeAsync_ShouldReturnCalculatedResponse(
            [Frozen] ProducerResubmissionFeeRequestDto request,
            [Frozen] decimal baseFee,
            [Frozen] decimal previousPayments)
        {
            // Arrange
            _resubmissionAmountStrategyMock
                .Setup(strategy => strategy.CalculateFeeAsync(request, _cancellationToken))
                .ReturnsAsync(baseFee);

            _paymentsServiceMock
                .Setup(service => service.GetPreviousPaymentsByReferenceAsync(request.ReferenceNumber, _cancellationToken))
                .ReturnsAsync(previousPayments);

            // Act
            var result = await _resubmissionService.GetResubmissionFeeAsync(request, _cancellationToken);

            // Assert
            using (new AssertionScope())
            {
                result.TotalResubmissionFee.Should().Be(baseFee);
                result.PreviousPayments.Should().Be(previousPayments);
                result.OutstandingPayment.Should().Be(baseFee - previousPayments);
            }
        }

        [TestMethod, AutoMoqData]
        public async Task GetResubmissionFeeAsync_WithNoPreviousPayments_ShouldReturnTotalFeeAsOutstanding(
            [Frozen] ProducerResubmissionFeeRequestDto request,
            [Frozen] decimal baseFee)
        {
            // Arrange
            _resubmissionAmountStrategyMock
                .Setup(strategy => strategy.CalculateFeeAsync(request, _cancellationToken))
                .ReturnsAsync(baseFee);

            _paymentsServiceMock
                .Setup(service => service.GetPreviousPaymentsByReferenceAsync(request.ReferenceNumber, _cancellationToken))
                .ReturnsAsync(0);

            // Act
            var result = await _resubmissionService.GetResubmissionFeeAsync(request, _cancellationToken);

            // Assert
            result.TotalResubmissionFee.Should().Be(baseFee);
            result.PreviousPayments.Should().Be(0);
            result.OutstandingPayment.Should().Be(baseFee);
        }

        [TestMethod, AutoMoqData]
        public async Task GetResubmissionFeeAsync_WithFullPreviousPayment_ShouldReturnZeroOutstanding(
            [Frozen] ProducerResubmissionFeeRequestDto request,
            [Frozen] decimal baseFee)
        {
            // Arrange
            _resubmissionAmountStrategyMock
                .Setup(strategy => strategy.CalculateFeeAsync(request, _cancellationToken))
                .ReturnsAsync(baseFee);

            _paymentsServiceMock
                .Setup(service => service.GetPreviousPaymentsByReferenceAsync(request.ReferenceNumber, _cancellationToken))
                .ReturnsAsync(baseFee);

            // Act
            var result = await _resubmissionService.GetResubmissionFeeAsync(request, _cancellationToken);

            // Assert
            result.TotalResubmissionFee.Should().Be(baseFee);
            result.PreviousPayments.Should().Be(baseFee);
            result.OutstandingPayment.Should().Be(0);
        }

        [TestMethod, AutoMoqData]
        public async Task GetResubmissionFeeAsync_WithOverpayment_ShouldReturnZeroOutstandingPayment(
            [Frozen] ProducerResubmissionFeeRequestDto request,
            [Frozen] int baseFee)
        {
            // Arrange
            int overpayment = baseFee + 500; // Overpayment of 500 pence
            _resubmissionAmountStrategyMock
                .Setup(strategy => strategy.CalculateFeeAsync(request, _cancellationToken))
                .ReturnsAsync(baseFee);

            _paymentsServiceMock
                .Setup(service => service.GetPreviousPaymentsByReferenceAsync(request.ReferenceNumber, _cancellationToken))
                .ReturnsAsync(overpayment);

            // Act
            var result = await _resubmissionService.GetResubmissionFeeAsync(request, _cancellationToken);

            // Assert
            result.TotalResubmissionFee.Should().Be(baseFee);
            result.PreviousPayments.Should().Be(overpayment);
            result.OutstandingPayment.Should().Be(baseFee - overpayment); // Should be negative (-500)
        }

        [TestMethod, AutoMoqData]
        public async Task GetResubmissionFeeAsync_WithExcessOverpayment_ShouldReturnNegativeOutstandingPayment(
            [Frozen] ProducerResubmissionFeeRequestDto request,
            [Frozen] int baseFee)
        {
            // Arrange
            int overpayment = baseFee + 5000; // Overpayment of 5000 pence
            _resubmissionAmountStrategyMock
                .Setup(strategy => strategy.CalculateFeeAsync(request, _cancellationToken))
                .ReturnsAsync(baseFee);

            _paymentsServiceMock
                .Setup(service => service.GetPreviousPaymentsByReferenceAsync(request.ReferenceNumber, _cancellationToken))
                .ReturnsAsync(overpayment);

            // Act
            var result = await _resubmissionService.GetResubmissionFeeAsync(request, _cancellationToken);

            // Assert
            result.TotalResubmissionFee.Should().Be(baseFee);
            result.PreviousPayments.Should().Be(overpayment);
            result.OutstandingPayment.Should().Be(baseFee - overpayment); // Should be negative due to excess overpayment
        }
    }
}