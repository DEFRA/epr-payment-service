using AutoFixture.MSTest;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationSubmission;
using EPR.Payment.Service.Common.UnitTests.TestHelpers;
using EPR.Payment.Service.Controllers.RegistrationSubmission;
using EPR.Payment.Service.Services.Interfaces.RegistrationSubmission;
using FluentAssertions;
using FluentAssertions.Execution;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EPR.Payment.Service.UnitTests.Controllers.RegistrationSubmission
{
    [TestClass]
    public class RegistrationSubmissionDataControllerTests
    {
        [TestMethod, AutoMoqData]
        public async Task Create_ValidRequest_ReturnsOkWithId(
            [Frozen] Mock<IValidator<CreateRegistrationSubmissionDataRequest>> validatorMock,
            [Frozen] Mock<IRegistrationSubmissionDataHandler> handlerMock,
            [Greedy] RegistrationSubmissionDataController sut,
            CreateRegistrationSubmissionDataRequest request,
            Guid expected,
            CancellationTokenSource cts)
        {
            validatorMock.Setup(v => v.Validate(request)).Returns(new ValidationResult());
            handlerMock.Setup(h => h.HandleAsync(request, cts.Token)).ReturnsAsync(expected);

            var result = await sut.Create(request, cts.Token);

            using (new AssertionScope())
            {
                result.Result.Should().BeOfType<OkObjectResult>().Which.Value.Should().Be(expected);
                handlerMock.Verify(h => h.HandleAsync(request, cts.Token), Times.Once);
            }
        }

        [TestMethod, AutoMoqData]
        public async Task Create_ValidationFails_ReturnsBadRequestWithProblemDetails(
            [Frozen] Mock<IValidator<CreateRegistrationSubmissionDataRequest>> validatorMock,
            [Frozen] Mock<IRegistrationSubmissionDataHandler> handlerMock,
            [Greedy] RegistrationSubmissionDataController sut,
            CreateRegistrationSubmissionDataRequest request,
            CancellationTokenSource cts)
        {
            validatorMock.Setup(v => v.Validate(request)).Returns(new ValidationResult(new[]
            {
                new ValidationFailure("SubmissionId", "SubmissionId is required."),
                new ValidationFailure("FileId", "FileId is required."),
            }));

            var result = await sut.Create(request, cts.Token);

            using (new AssertionScope())
            {
                var bad = result.Result.Should().BeOfType<BadRequestObjectResult>().Which;
                bad.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
                bad.Value.Should().BeOfType<ProblemDetails>().Which.Detail.Should().Contain("SubmissionId is required").And.Contain("FileId is required");
                handlerMock.Verify(h => h.HandleAsync(It.IsAny<CreateRegistrationSubmissionDataRequest>(), It.IsAny<CancellationToken>()), Times.Never);
            }
        }

        [TestMethod, AutoMoqData]
        public async Task Create_HandlerThrowsInvalidOperation_ReturnsBadRequest(
            [Frozen] Mock<IValidator<CreateRegistrationSubmissionDataRequest>> validatorMock,
            [Frozen] Mock<IRegistrationSubmissionDataHandler> handlerMock,
            [Greedy] RegistrationSubmissionDataController sut,
            CreateRegistrationSubmissionDataRequest request,
            CancellationTokenSource cts)
        {
            validatorMock.Setup(v => v.Validate(request)).Returns(new ValidationResult());
            handlerMock.Setup(h => h.HandleAsync(request, cts.Token)).ThrowsAsync(new InvalidOperationException("orphan subsidiary"));

            var result = await sut.Create(request, cts.Token);

            var bad = result.Result.Should().BeOfType<BadRequestObjectResult>().Which;
            bad.Value.Should().BeOfType<ProblemDetails>().Which.Detail.Should().Contain("orphan subsidiary");
        }

        [TestMethod, AutoMoqData]
        public async Task Create_HandlerThrowsUnexpected_Returns500(
            [Frozen] Mock<IValidator<CreateRegistrationSubmissionDataRequest>> validatorMock,
            [Frozen] Mock<IRegistrationSubmissionDataHandler> handlerMock,
            [Greedy] RegistrationSubmissionDataController sut,
            CreateRegistrationSubmissionDataRequest request,
            CancellationTokenSource cts)
        {
            validatorMock.Setup(v => v.Validate(request)).Returns(new ValidationResult());
            handlerMock.Setup(h => h.HandleAsync(request, cts.Token)).ThrowsAsync(new Exception("boom"));

            var result = await sut.Create(request, cts.Token);

            var status = result.Result.Should().BeOfType<ObjectResult>().Which;
            status.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }
    }
}
