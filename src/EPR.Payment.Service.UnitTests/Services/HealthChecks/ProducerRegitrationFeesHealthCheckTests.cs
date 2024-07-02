using EPR.Payment.Service.HealthCheck;
using EPR.Payment.Service.Services.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Moq;

namespace EPR.Payment.Service.UnitTests.Services.HealthChecks
{
    [TestClass]
    public class ProducerRegitrationFeesHealthCheckTests
    {
        private Mock<IProducerFeesService> _feesServiceMock = null!;
        private ProducerRegitrationFeesHealthCheck _FeesHealthCheck = null!;

        [TestInitialize]
        public void TestInitialize()
        {
            _feesServiceMock = new Mock<IProducerFeesService>();
            _FeesHealthCheck = new ProducerRegitrationFeesHealthCheck(_feesServiceMock.Object);
        }

        [TestMethod]
        public async Task FeesHealthCheck_FeesPresent_ReturnsHealthy()
        {
            //Arrange
            _feesServiceMock.Setup(x => x.GetProducerRegitrationFeesCount()).ReturnsAsync(3);

            //Act
            var result = await _FeesHealthCheck.CheckHealthAsync(new HealthCheckContext(), new CancellationToken());

            //Assert
            result.Status.Should().Be(HealthStatus.Healthy);
        }

        [TestMethod]
        public async Task FeesHealthCheck_FeesAbsent_ReturnsHealthy()
        {
            //Arrange
            _feesServiceMock.Setup(x => x.GetProducerRegitrationFeesCount()).ReturnsAsync(0);

            //Act
            var result = await _FeesHealthCheck.CheckHealthAsync(new HealthCheckContext(), new CancellationToken());

            //Assert
            result.Status.Should().Be(HealthStatus.Unhealthy);
        }
    }
}