using EPR.Payment.Service.HealthCheck;
using EPR.Payment.Service.Services.Interfaces;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Moq;
using FluentAssertions;

namespace EPR.Payment.Service.UnitTests.Services.HealthChecks
{
    [TestClass]
    public class PaymentStatusHealthCheckTests
    {
        private Mock<IPaymentsService> _paymentsServiceMock = null!;
        private PaymentStatusHealthCheck _paymentStatusHealthCheck = null!;

        [TestInitialize]
        public void TestInitialize()
        {
            _paymentsServiceMock = new Mock<IPaymentsService>();
            _paymentStatusHealthCheck = new PaymentStatusHealthCheck(_paymentsServiceMock.Object);
        }

        [TestMethod]
        public async Task PaymentStatusHealthCheck_Present_ReturnsHealthy()
        {
            //Arrange
            _paymentsServiceMock.Setup(x => x.GetPaymentStatusCount()).ReturnsAsync(6);

            //Act
            var result = await _paymentStatusHealthCheck.CheckHealthAsync(new HealthCheckContext(), new CancellationToken());

            //Assert
            result.Status.Should().Be(HealthStatus.Healthy);
        }

        [TestMethod]
        public async Task PaymentStatusHealthCheck_Absent_ReturnsHealthy()
        {
            //Arrange
            _paymentsServiceMock.Setup(x => x.GetPaymentStatusCount()).ReturnsAsync(0);

            //Act
            var result = await _paymentStatusHealthCheck.CheckHealthAsync(new HealthCheckContext(), new CancellationToken());

            //Assert
            result.Status.Should().Be(HealthStatus.Unhealthy);
        }
    }
}
