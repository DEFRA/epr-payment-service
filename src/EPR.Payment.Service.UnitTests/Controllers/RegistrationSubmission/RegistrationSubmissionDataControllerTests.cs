using AutoFixture.MSTest;
using EPR.Payment.Service.Common.Dtos.Response.RegistrationSubmission;
using EPR.Payment.Service.Common.UnitTests.TestHelpers;
using EPR.Payment.Service.Controllers.RegistrationSubmission;
using EPR.Payment.Service.Services.Interfaces.RegistrationSubmission;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EPR.Payment.Service.UnitTests.Controllers.RegistrationSubmission
{
    [TestClass]
    public class RegistrationSubmissionDataControllerTests
    {
        [TestMethod, AutoMoqData]
        public async Task GetFeeCalculationDetails_Valid_ReturnsOkWithList(
            [Frozen] Mock<IRegistrationFeeCalculationDetailsService> serviceMock,
            [Greedy] RegistrationSubmissionDataController sut,
            Guid submissionId,
            CancellationTokenSource cts)
        {
            var expected = new List<RegistrationFeeCalculationDetailsDto>
            {
                new() { OrganisationId = "ORG-1", OrganisationSize = "Large", NationId = 1 },
            };
            serviceMock.Setup(s => s.GetAsync(submissionId, cts.Token)).ReturnsAsync(expected);

            var result = await sut.GetFeeCalculationDetails(submissionId, cts.Token);

            result.Should().BeOfType<OkObjectResult>().Which.Value.Should().BeEquivalentTo(expected);
        }

        [TestMethod, AutoMoqData]
        public async Task GetFeeCalculationDetails_EmptyList_ReturnsNotFound(
            [Frozen] Mock<IRegistrationFeeCalculationDetailsService> serviceMock,
            [Greedy] RegistrationSubmissionDataController sut,
            Guid submissionId,
            CancellationTokenSource cts)
        {
            serviceMock.Setup(s => s.GetAsync(submissionId, cts.Token))
                .ReturnsAsync(Array.Empty<RegistrationFeeCalculationDetailsDto>());

            var result = await sut.GetFeeCalculationDetails(submissionId, cts.Token);

            result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod, AutoMoqData]
        public async Task GetFeeCalculationDetails_EmptyGuid_ReturnsBadRequest(
            [Frozen] Mock<IRegistrationFeeCalculationDetailsService> serviceMock,
            [Greedy] RegistrationSubmissionDataController sut,
            CancellationTokenSource cts)
        {
            var result = await sut.GetFeeCalculationDetails(Guid.Empty, cts.Token);

            using (new AssertionScope())
            {
                var bad = result.Should().BeOfType<BadRequestObjectResult>().Which;
                bad.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
                serviceMock.Verify(s => s.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
            }
        }

        [TestMethod, AutoMoqData]
        public async Task GetFeeCalculationDetails_ServiceThrows_Returns500(
            [Frozen] Mock<IRegistrationFeeCalculationDetailsService> serviceMock,
            [Greedy] RegistrationSubmissionDataController sut,
            Guid submissionId,
            CancellationTokenSource cts)
        {
            serviceMock.Setup(s => s.GetAsync(submissionId, cts.Token)).ThrowsAsync(new Exception("boom"));

            var result = await sut.GetFeeCalculationDetails(submissionId, cts.Token);

            var status = result.Should().BeOfType<ObjectResult>().Which;
            status.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }
    }
}
