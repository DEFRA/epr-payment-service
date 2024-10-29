using EPR.Payment.Service.HealthCheck;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Moq;
using FluentAssertions;
using EPR.Payment.Service.Services.Interfaces.Payments;

namespace EPR.Payment.Service.UnitTests.Services.HealthChecks
{
    [TestClass]
    public class PaymentStatusHealthCheckTests
    {
        private Mock<IOnlinePaymentsService> _paymentsServiceMock = null!;
        private PaymentStatusHealthCheck _paymentStatusHealthCheck = null!;

        [TestInitialize]
        public void TestInitialize()
        {
            _paymentsServiceMock = new Mock<IOnlinePaymentsService>();
            _paymentStatusHealthCheck = new PaymentStatusHealthCheck(_paymentsServiceMock.Object);
        }

        [TestMethod]
        public async Task PaymentStatusHealthCheck_Present_ReturnsHealthy()
        {
            //Arrange
            var cancellationToken = new CancellationToken();
            _paymentsServiceMock.Setup(x => x.GetOnlinePaymentStatusCountAsync(cancellationToken)).ReturnsAsync(6);

            //Act
            var result = await _paymentStatusHealthCheck.CheckHealthAsync(new HealthCheckContext(), new CancellationToken());

            //Assert
            result.Status.Should().Be(HealthStatus.Healthy);
        }

        [TestMethod]
        public async Task PaymentStatusHealthCheck_Absent_ReturnsHealthy()
        {
            //Arrange
            var cancellationToken = new CancellationToken();
            _paymentsServiceMock.Setup(x => x.GetOnlinePaymentStatusCountAsync(cancellationToken)).ReturnsAsync(0);

            //Act
            var result = await _paymentStatusHealthCheck.CheckHealthAsync(new HealthCheckContext(), new CancellationToken());

            //Assert
            result.Status.Should().Be(HealthStatus.Unhealthy);
        }
    }
}
