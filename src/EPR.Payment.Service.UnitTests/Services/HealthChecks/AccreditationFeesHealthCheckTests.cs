using EPR.Payment.Service.HealthCheck;
using EPR.Payment.Service.Services.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Moq;

namespace EPR.Payment.Service.UnitTests.Services.HealthChecks
{
    [TestClass]
    public class AccreditationFeesHealthCheckTests
    {
        private Mock<IAccreditationFeesService> _feesServiceMock = null!;
        private AccreditationFeesHealthCheck _accreditationFeesHealthCheck = null!;

        [TestInitialize]
        public void TestInitialize()
        {
            _feesServiceMock = new Mock<IAccreditationFeesService>();
            _accreditationFeesHealthCheck = new AccreditationFeesHealthCheck(_feesServiceMock.Object);
        }

        [TestMethod]
        public async Task AccreditationFeesHealthCheck_FeesPresent_ReturnsHealthy()
        {
            //Arrange
            _feesServiceMock.Setup(x => x.GetFeesCount()).ReturnsAsync(3);

            //Act
            var result = await _accreditationFeesHealthCheck.CheckHealthAsync(new HealthCheckContext(), new CancellationToken());

            //Assert
            result.Status.Should().Be(HealthStatus.Healthy);
        }

        [TestMethod]
        public async Task AccreditationFeesHealthCheck_FeesAbsent_ReturnsHealthy()
        {
            //Arrange
            _feesServiceMock.Setup(x => x.GetFeesCount()).ReturnsAsync(0);

            //Act
            var result = await _accreditationFeesHealthCheck.CheckHealthAsync(new HealthCheckContext(), new CancellationToken());

            //Assert
            result.Status.Should().Be(HealthStatus.Unhealthy);
        }
    }
}