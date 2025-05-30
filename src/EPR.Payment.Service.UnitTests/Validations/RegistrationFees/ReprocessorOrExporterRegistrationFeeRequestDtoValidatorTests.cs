using Azure.Core;
using EPR.Payment.Service.Common.Constants.RegistrationFees;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.ReprocessorOrExporter;
using EPR.Payment.Service.Common.Enums;
using EPR.Payment.Service.Validations.RegistrationFees.ReprocessorOrExporter;
using FluentValidation.TestHelper;

namespace EPR.Payment.Service.UnitTests.Validations.RegistrationFees
{
    [TestClass]
    public class ReprocessorOrExporterRegistrationFeeRequestDtoValidatorTests
    {
        private ReprocessorOrExporterRegistrationFeesRequestDtoValidator _validator = null!;

        [TestInitialize]
        public void Setup()
        {
            _validator = new ReprocessorOrExporterRegistrationFeesRequestDtoValidator();
        }

        [TestMethod]
        public void Validate_EmptyRequestorType_ShouldHaveError()
        {
            // Arrange
            var request = new ReprocessorOrExporterRegistrationFeesRequestDto
            {
                MaterialType = MaterialTypes.Aluminium,
                Regulator = RegulatorConstants.GBENG,
                ApplicationReferenceNumber = "A123",
                SubmissionDate = DateTime.UtcNow
            };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.RequestorType)
                  .WithErrorMessage(ValidationMessages.EmptyRequestorType);
        }

        [TestMethod]
        public void Validate_EmptyMaterialType_ShouldHaveError()
        {
            // Arrange
            var request = new ReprocessorOrExporterRegistrationFeesRequestDto
            {
                RequestorType = RequestorTypes.Exporters,
                Regulator = RegulatorConstants.GBENG,
                ApplicationReferenceNumber = "A123",
                SubmissionDate = DateTime.UtcNow
            };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.MaterialType)
                  .WithErrorMessage(ValidationMessages.EmptyMaterialType);
        }

        [TestMethod]
        public void Validate_EmptyRegulator_ShouldHaveError()
        {
            // Arrange
            var request = new ReprocessorOrExporterRegistrationFeesRequestDto
            {
                RequestorType = RequestorTypes.Exporters,
                MaterialType = MaterialTypes.Aluminium,
                Regulator = string.Empty,
                ApplicationReferenceNumber = "A123",
                SubmissionDate = DateTime.UtcNow
            };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Regulator)
                  .WithErrorMessage(ValidationMessages.RegulatorRequired);
        }

        [TestMethod]
        public void Validate_ValidRegulator_ShouldNotHaveError()
        {
            // Arrange
            var validRegulators = new[] { RegulatorConstants.GBENG, RegulatorConstants.GBSCT, RegulatorConstants.GBWLS, RegulatorConstants.GBNIR };

            foreach (var regulator in validRegulators)
            {
                var request = new ReprocessorOrExporterRegistrationFeesRequestDto
                {
                    RequestorType = RequestorTypes.Exporters,
                    MaterialType = MaterialTypes.Aluminium,
                    Regulator = regulator,
                    ApplicationReferenceNumber = "A123",
                    SubmissionDate = DateTime.UtcNow
                };

                // Act
                var result = _validator.TestValidate(request);

                // Assert
                result.ShouldNotHaveValidationErrorFor(x => x.Regulator);
            }
        }

        [TestMethod]
        public void Validate_EmptyApplicationReferenceNumber_ShouldNotHaveError()
        {
            // Arrange
            var request = new ReprocessorOrExporterRegistrationFeesRequestDto
            {
                RequestorType = RequestorTypes.Exporters,
                MaterialType = MaterialTypes.Aluminium,
                Regulator = RegulatorConstants.GBENG,
                SubmissionDate = DateTime.UtcNow
            };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.ApplicationReferenceNumber);
        }

        [TestMethod]
        public void Validate_InvalidSubmissionDate_ShouldHaveError()
        {
            // Arrange
            var request = new ReprocessorOrExporterRegistrationFeesRequestDto
            {
                RequestorType = RequestorTypes.Exporters,
                MaterialType = MaterialTypes.Aluminium,
                Regulator = RegulatorConstants.GBENG,
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
            var request = new ReprocessorOrExporterRegistrationFeesRequestDto
            {
                RequestorType = RequestorTypes.Exporters,
                MaterialType = MaterialTypes.Aluminium,
                Regulator = RegulatorConstants.GBENG,
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
            var request = new ReprocessorOrExporterRegistrationFeesRequestDto
            {
                RequestorType = RequestorTypes.Exporters,
                MaterialType = MaterialTypes.Aluminium,
                Regulator = RegulatorConstants.GBENG,
                ApplicationReferenceNumber = "A123",
                SubmissionDate = DateTime.Now
            };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.SubmissionDate)
                .WithErrorMessage(ValidationMessages.SubmissionDateMustBeUtc);
        }
    }
}
