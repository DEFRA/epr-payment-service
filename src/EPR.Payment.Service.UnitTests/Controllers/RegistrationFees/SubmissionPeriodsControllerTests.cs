using EPR.Payment.Service.Common.Dtos.Response.RegistrationFees;
using EPR.Payment.Service.Controllers.RegistrationFees;
using EPR.Payment.Service.Services.Interfaces.RegistrationFees;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EPR.Payment.Service.UnitTests.Controllers.RegistrationFees
{
    [TestClass]
    public class SubmissionPeriodsControllerTests
    {
        private Mock<ISubmissionPeriodsService> _serviceMock = null!;
        private SubmissionPeriodsController _sut = null!;

        [TestInitialize]
        public void Init()
        {
            _serviceMock = new Mock<ISubmissionPeriodsService>();
            _sut = new SubmissionPeriodsController(_serviceMock.Object);
        }

        [TestMethod]
        public void Constructor_NullService_Throws()
        {
            Action act = () => new SubmissionPeriodsController(null!);
            act.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public async Task GetSubmissionPeriods_ReturnsOkWithPayload()
        {
            var rows = new List<SubmissionPeriodResponseDto>
            {
                new() { Id = 1, WindowType = "Cso", RegistrationYear = 2025 },
                new() { Id = 3, WindowType = "CsoLargeProducer", RegistrationYear = 2026 },
            };
            _serviceMock
                .Setup(s => s.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(rows);

            var result = await _sut.GetSubmissionPeriods(CancellationToken.None);

            using (new AssertionScope())
            {
                var ok = result.Should().BeOfType<OkObjectResult>().Which;
                ok.Value.Should().BeEquivalentTo(rows);
            }
        }
    }
}
