using EPR.Payment.Service.Common.Dtos.Request.RegistrationSubmission;
using EPR.Payment.Service.Validations.RegistrationSubmission;
using FluentAssertions;

namespace EPR.Payment.Service.UnitTests.Validations.RegistrationSubmission
{
    [TestClass]
    public class CreateRegistrationSubmissionDataRequestValidatorTests
    {
        private readonly CreateRegistrationSubmissionDataRequestValidator _sut = new();

        [TestMethod]
        public void Validate_AllFieldsPopulated_PassesValidation()
        {
            var request = new CreateRegistrationSubmissionDataRequest
            {
                SubmissionId = Guid.NewGuid(),
                FileId = Guid.NewGuid(),
                ComplianceSchemeId = Guid.NewGuid(),
                SubmissionPeriod = "Jan to Jun 2026",
                SubmissionDate = new DateTime(2026, 5, 28, 0, 0, 0, DateTimeKind.Utc),
            };

            var result = _sut.Validate(request);

            result.IsValid.Should().BeTrue();
        }

        [TestMethod]
        public void Validate_MissingFields_FailsForEach()
        {
            var request = new CreateRegistrationSubmissionDataRequest
            {
                SubmissionId = Guid.Empty,
                FileId = Guid.Empty,
                SubmissionPeriod = string.Empty,
                SubmissionDate = default,
            };

            var result = _sut.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Select(e => e.PropertyName).Should().Contain(new[] { "SubmissionId", "FileId", "SubmissionPeriod", "SubmissionDate" });
        }

        [TestMethod]
        public void Validate_ComplianceSchemeIdOptional()
        {
            var request = new CreateRegistrationSubmissionDataRequest
            {
                SubmissionId = Guid.NewGuid(),
                FileId = Guid.NewGuid(),
                ComplianceSchemeId = null,
                SubmissionPeriod = "Jan to Jun 2026",
                SubmissionDate = new DateTime(2026, 5, 28, 0, 0, 0, DateTimeKind.Utc),
            };

            var result = _sut.Validate(request);

            result.IsValid.Should().BeTrue();
        }
    }
}
