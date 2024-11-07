using EPR.Payment.Service.Common.Constants.RegistrationFees;
using EPR.Payment.Service.Common.Dtos.Request.ResubmissionFees.Producer;
using EPR.Payment.Service.Common.UnitTests.TestHelpers;
using EPR.Payment.Service.Validations.ResubmissionFees.Producer;
using FluentValidation.TestHelper;

namespace EPR.Payment.Service.UnitTests.Validations.ResubmissionFees
{
    [TestClass]
    public class ProducerResubmissionFeeRequestDtoValidatorTests
    {
        private ProducerResubmissionFeeRequestDtoValidator _validator = null!;

        [TestInitialize]
        public void TestInitialize()
        {
            _validator = new ProducerResubmissionFeeRequestDtoValidator();
        }

        [TestMethod]
        public void Validate_RegulatorIsEmpty_ShouldHaveValidationError()
        {
            // Arrange
            var dto = new ProducerResubmissionFeeRequestDto
            {
                Regulator = "",
                ResubmissionDate = DateTime.UtcNow,
                ReferenceNumber = "Ref123"
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Regulator)
                  .WithErrorMessage(ValidationMessages.RegulatorRequired);
        }

        [TestMethod]
        public void Validate_RegulatorIsInvalid_ShouldHaveValidationError()
        {
            // Arrange
            var dto = new ProducerResubmissionFeeRequestDto
            {
                Regulator = "InvalidRegulator",
                ResubmissionDate = DateTime.UtcNow,
                ReferenceNumber = "Ref123"
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Regulator)
                  .WithErrorMessage(ValidationMessages.RegulatorInvalid);
        }

        [TestMethod]
        public void Validate_ResubmissionDateIsEmpty_ShouldHaveValidationError()
        {
            // Arrange
            var dto = new ProducerResubmissionFeeRequestDto
            {
                Regulator = "GB-ENG",
                ReferenceNumber = "Ref123"
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.ResubmissionDate)
                  .WithErrorMessage(ValidationMessages.ResubmissionDateRequired);
        }

        [TestMethod]
        public void Validate_ResubmissionDateIsInTheFuture_ShouldHaveValidationError()
        {
            // Arrange
            var dto = new ProducerResubmissionFeeRequestDto
            {
                Regulator = "GB-ENG",
                ResubmissionDate = DateTime.UtcNow.AddDays(1),
                ReferenceNumber = "Ref123"
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.ResubmissionDate)
                  .WithErrorMessage(ValidationMessages.ResubmissionDateInvalid);
        }

        [TestMethod]
        public void Validate_ResubmissionDateIsNotUtc_ShouldHaveValidationError()
        {
            // Arrange
            var dto = new ProducerResubmissionFeeRequestDto
            {
                Regulator = "GB-ENG",
                ResubmissionDate = DateTime.Now, // Local date, not UTC
                ReferenceNumber = "Ref123"
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.ResubmissionDate)
                  .WithErrorMessage(ValidationMessages.ResubmissionDateMustBeUtc);
        }

        [TestMethod]
        public void Validate_ReferenceNumberIsEmpty_ShouldHaveValidationError()
        {
            // Arrange
            var dto = new ProducerResubmissionFeeRequestDto
            {
                Regulator = "GB-ENG",
                ResubmissionDate = DateTime.UtcNow,
                ReferenceNumber = ""
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.ReferenceNumber)
                  .WithErrorMessage(ValidationMessages.ReferenceNumberRequired);
        }

        [TestMethod, AutoMoqData]
        public void Validate_AllFieldsAreValid_ShouldNotHaveAnyValidationErrors()
        {
            // Arrange
            var dto = new ProducerResubmissionFeeRequestDto
            {
                Regulator = "GB-ENG",
                ResubmissionDate = DateTime.UtcNow.AddMinutes(-1), // Ensuring the date is in the past
                ReferenceNumber = "ValidRef123"
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
