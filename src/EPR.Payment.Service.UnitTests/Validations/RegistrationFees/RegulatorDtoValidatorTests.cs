using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.Producer;
using EPR.Payment.Service.Validations.RegistrationFees;
using FluentValidation.TestHelper;

namespace EPR.Payment.Service.UnitTests.Validations.RegistrationFees
{

    [TestClass]
    public class RegulatorDtoValidatorTests
    {
        private RegulatorDtoValidator _validator = null!;

        [TestInitialize]
        public void Setup()
        {
            _validator = new RegulatorDtoValidator();
        }

        [TestMethod]
        public void Validate_EmptyRegulator_ShouldHaveError()
        {
            // Arrange
            var dto = new RegulatorDto { Regulator = string.Empty };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Regulator)
                  .WithErrorMessage("Regulator is required.");
        }

        [TestMethod]
        public void Validate_NullRegulator_ShouldHaveError()
        {
            // Arrange
            var dto = new RegulatorDto { Regulator = null };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Regulator)
                  .WithErrorMessage("Regulator is required.");
        }

        [TestMethod]
        public void Validate_InvalidRegulator_ShouldHaveError()
        {
            // Arrange
            var dto = new RegulatorDto { Regulator = "INVALID" };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Regulator)
                  .WithErrorMessage("Invalid regulator parameter.");
        }

        [TestMethod]
        public void Validate_ValidRegulator_ShouldNotHaveError()
        {
            // Arrange
            var validRegulators = new[] { "GB-ENG", "GB-SCT", "GB-WLS", "GB-NIR" };

            foreach (var regulator in validRegulators)
            {
                var dto = new RegulatorDto { Regulator = regulator };

                // Act
                var result = _validator.TestValidate(dto);

                // Assert
                result.ShouldNotHaveValidationErrorFor(x => x.Regulator);
            }
        }

        [TestMethod]
        public void Validate_LowercaseRegulator_ShouldHaveError()
        {
            // Arrange
            var dto = new RegulatorDto { Regulator = "gb-eng" }; // Lowercase

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Regulator)
                  .WithErrorMessage("Regulator must be in uppercase.");
        }

        [TestMethod]
        public void Validate_RegulatorWithLeadingTrailingSpaces_ShouldHaveError()
        {
            // Arrange
            var dto = new RegulatorDto { Regulator = " GB-ENG " }; // With spaces

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Regulator)
                  .WithErrorMessage("Invalid regulator parameter.");
        }

        [TestMethod]
        public void Validate_RegulatorTooShort_ShouldHaveError()
        {
            // Arrange
            var dto = new RegulatorDto { Regulator = "GB" }; // Assuming a minimum length constraint

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Regulator)
                  .WithErrorMessage("Invalid regulator parameter.");
        }

        [TestMethod]
        public void Validate_RegulatorTooLong_ShouldHaveError()
        {
            // Arrange
            var dto = new RegulatorDto { Regulator = "GB-ENG-TOOLONG" }; // Assuming a maximum length constraint

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Regulator)
                  .WithErrorMessage("Invalid regulator parameter.");
        }
    }
}
