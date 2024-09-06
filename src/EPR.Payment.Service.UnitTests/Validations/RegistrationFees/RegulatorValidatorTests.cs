using EPR.Payment.Service.Validations.RegistrationFees;
using FluentValidation.TestHelper;

namespace EPR.Payment.Service.UnitTests.Validations.RegistrationFees
{
    [TestClass]
    public class RegulatorValidatorTests
    {
        private RegulatorValidator _validator = null!;

        [TestInitialize]
        public void Setup()
        {
            _validator = new RegulatorValidator();
        }

        [TestMethod]
        public void Validate_EmptyRegulator_ShouldHaveError()
        {
            // Arrange
            var regulator = string.Empty;

            // Act
            var result = _validator.TestValidate(regulator);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x)
                  .WithErrorMessage("Regulator is required.");
        }

        [TestMethod]
        public void Validate_NullRegulator_ShouldHaveError()
        {
            // Arrange
            string? regulator = null;

            // Act
            var result = _validator.TestValidate(regulator ?? string.Empty);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x)
                  .WithErrorMessage("Regulator is required.");
        }

        [TestMethod]
        public void Validate_InvalidRegulator_ShouldHaveError()
        {
            // Arrange
            var regulator = "INVALID";

            // Act
            var result = _validator.TestValidate(regulator);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x)
                  .WithErrorMessage("Invalid regulator parameter.");
        }

        [TestMethod]
        public void Validate_ValidRegulator_ShouldNotHaveError()
        {
            // Arrange
            var validRegulators = new[] { "GB-ENG", "GB-SCT", "GB-WLS", "GB-NIR" };

            foreach (var regulator in validRegulators)
            {
                // Act
                var result = _validator.TestValidate(regulator);

                // Assert
                result.ShouldNotHaveValidationErrorFor(x => x);
            }
        }

        [TestMethod]
        public void Validate_LowercaseRegulator_ShouldHaveError()
        {
            // Arrange
            var regulator = "gb-eng"; // Lowercase

            // Act
            var result = _validator.TestValidate(regulator);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x)
                  .WithErrorMessage("Regulator must be in uppercase.");
        }
    }
}
