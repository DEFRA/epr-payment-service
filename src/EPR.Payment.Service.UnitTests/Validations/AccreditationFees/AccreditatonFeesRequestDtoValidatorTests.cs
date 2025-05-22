using EPR.Payment.Service.Common.Constants.RegistrationFees;
using EPR.Payment.Service.Common.Dtos.Request.AccreditationFees;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.Producer;
using EPR.Payment.Service.Validations.AccreditationFees;
using FluentValidation.TestHelper;
using EPR.Payment.Service.Common.Enums;

namespace EPR.Payment.Service.UnitTests.Validations.RegistrationFees
{
    [TestClass]
    public class AccreditatonFeesRequestDtoValidatorTests
    {
        private AccreditationFeesRequestDtoValidator _validator = null!;

        [TestInitialize]
        public void Setup()
        {
            _validator = new AccreditationFeesRequestDtoValidator();
        }

        [TestMethod]
        public void Validate_InvalidRegulator_ShouldHaveError()
        {
            // Arrange
            var request = new AccreditationFeesRequestDto
            {
                Regulator = "X",
                RequestorType = AccreditationFeesRequestorType.Exporters,
                MaterialType = AccreditationFeesMaterialType.Aluminium,
                NumberOfOverseasSites = 0,               
                TonnageBand = TonnageBand.Upto500,
                ApplicationReferenceNumber = "A123",
                SubmissionDate = DateTime.UtcNow
            };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Regulator)
                  .WithErrorMessage(ValidationMessages.RegulatorInvalid);
        }

        [TestMethod]
        public void Validate_EmptyRegulator_ShouldHaveError()
        {
            // Arrange
            var request = new AccreditationFeesRequestDto
            {
                Regulator = string.Empty,
                RequestorType = AccreditationFeesRequestorType.Exporters,
                MaterialType = AccreditationFeesMaterialType.Aluminium,
                NumberOfOverseasSites = 0,
                TonnageBand = TonnageBand.Upto500,
                ApplicationReferenceNumber = "A123",
                SubmissionDate = DateTime.UtcNow
            };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Regulator)
                  .WithErrorMessage(ValidationMessages.RegulatorInvalid);
        }

        [TestMethod]
        public void Validate_InvalidSubmissionDate_ShouldHaveError()
        {
            // Arrange
            var request = new AccreditationFeesRequestDto
            {
                Regulator = RegulatorConstants.GBENG,
                RequestorType = AccreditationFeesRequestorType.Exporters,
                MaterialType = AccreditationFeesMaterialType.Aluminium,
                NumberOfOverseasSites = 0,
                TonnageBand = TonnageBand.Upto500,
                ApplicationReferenceNumber = "A123",
                SubmissionDate = default
            };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.SubmissionDate)
                  .WithErrorMessage(ValidationMessages.InvalidSubmissionDate);
        }

        [TestMethod]
        public void Validate_FutureSubmissionDate_ShouldHaveError()
        {
            // Arrange
            var request = new AccreditationFeesRequestDto
            {
                Regulator = RegulatorConstants.GBENG,
                RequestorType = AccreditationFeesRequestorType.Exporters,
                MaterialType = AccreditationFeesMaterialType.Aluminium,
                NumberOfOverseasSites = 0,
                TonnageBand = TonnageBand.Upto500,
                ApplicationReferenceNumber = "A123",
                SubmissionDate = DateTime.UtcNow.AddDays(10)
            };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.SubmissionDate)
                  .WithErrorMessage(ValidationMessages.FutureSubmissionDate);
        }

        [TestMethod]
        public void Validate_SubmissionDateNotUtc_ShouldHaveError()
        {
            // Arrange
            var request = new AccreditationFeesRequestDto
            {
                Regulator = RegulatorConstants.GBENG,
                RequestorType = AccreditationFeesRequestorType.Exporters,
                MaterialType = AccreditationFeesMaterialType.Aluminium,
                NumberOfOverseasSites = 0,
                TonnageBand = TonnageBand.Upto500,
                ApplicationReferenceNumber = "A123",
                SubmissionDate = DateTime.Now
            };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.SubmissionDate)
                .WithErrorMessage(ValidationMessages.SubmissionDateMustBeUtc);
        }      

        [TestMethod]
        public void Validate_EmptyApplicationReferenceNumber_ShouldHaveError()
        {
            // Arrange
            var request = new AccreditationFeesRequestDto
            {
                Regulator = RegulatorConstants.GBENG,
                RequestorType = AccreditationFeesRequestorType.Exporters,
                MaterialType = AccreditationFeesMaterialType.Aluminium,
                NumberOfOverseasSites = 0,
                TonnageBand = TonnageBand.Upto500,
                ApplicationReferenceNumber = string.Empty,
                SubmissionDate = DateTime.Now
            };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.ApplicationReferenceNumber)
                  .WithErrorMessage(ValidationMessages.ApplicationReferenceNumberRequired);
        }
    }
}
