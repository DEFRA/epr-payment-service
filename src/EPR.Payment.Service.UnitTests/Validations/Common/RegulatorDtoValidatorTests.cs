using EPR.Payment.Service.Common.Dtos.Request.Common;
using EPR.Payment.Service.Validations.Common;
using FluentValidation.TestHelper;

namespace EPR.Payment.Service.UnitTests.Validations.Common
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
            var dto = new RegulatorDto { Regulator = null! };

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
                  .WithErrorMessage("Invalid Regulator.");
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
    }
}
