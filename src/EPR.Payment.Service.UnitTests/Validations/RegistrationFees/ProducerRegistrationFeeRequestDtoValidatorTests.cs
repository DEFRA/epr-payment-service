using EPR.Payment.Service.Common.Constants.RegistrationFees;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.Producer;
using EPR.Payment.Service.Validations.RegistrationFees;
using FluentValidation.TestHelper;

namespace EPR.Payment.Service.UnitTests.Validations.RegistrationFees
{
    [TestClass]
    public class ProducerRegistrationFeeRequestDtoValidatorTests
    {
        private ProducerRegistrationFeesRequestDtoValidator _validator = null!;

        [TestInitialize]
        public void Setup()
        {
            _validator = new ProducerRegistrationFeesRequestDtoValidator();
        }

        [TestMethod]
        public void Validate_InvalidProducerType_ShouldHaveError()
        {
            // Arrange
            var request = new ProducerRegistrationFeesRequestDto
            {
                ProducerType = "X",
                NumberOfSubsidiaries = 10,
                Regulator = RegulatorConstants.GBENG,
                IsOnlineMarketplace = false
            };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.ProducerType)
                  .WithErrorMessage(ValidationMessages.ProducerTypeInvalid + "LARGE, SMALL");
        }

        [TestMethod]
        public void Validate_UpperCaseProducerTypeLarge_ShouldNotHaveError()
        {
            // Arrange
            var request = new ProducerRegistrationFeesRequestDto
            {
                ProducerType = "LARGE",
                NumberOfSubsidiaries = 10,
                Regulator = RegulatorConstants.GBENG,
                IsOnlineMarketplace = false
            };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.ProducerType);
        }

        [TestMethod]
        public void Validate_LowerCaseProducerTypeLarge_ShouldNotHaveError()
        {
            // Arrange
            var request = new ProducerRegistrationFeesRequestDto
            {
                ProducerType = "large",  // Lowercase
                NumberOfSubsidiaries = 10,
                Regulator = RegulatorConstants.GBENG,
                IsOnlineMarketplace = false
            };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.ProducerType);
        }

        [TestMethod]
        public void Validate_UpperCaseProducerTypeSmall_ShouldNotHaveError()
        {
            // Arrange
            var request = new ProducerRegistrationFeesRequestDto
            {
                ProducerType = "SMALL",
                NumberOfSubsidiaries = 10,
                Regulator = RegulatorConstants.GBENG,
                IsOnlineMarketplace = false
            };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.ProducerType);
        }

        [TestMethod]
        public void Validate_LowerCaseProducerTypeSmall_ShouldNotHaveError()
        {
            // Arrange
            var request = new ProducerRegistrationFeesRequestDto
            {
                ProducerType = "small",  // Lowercase
                NumberOfSubsidiaries = 10,
                Regulator = RegulatorConstants.GBENG,
                IsOnlineMarketplace = false
            };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.ProducerType);
        }

        [TestMethod]
        public void Validate_NumberOfSubsidiariesOutOfRange_ShouldHaveError()
        {
            // Arrange
            var request = new ProducerRegistrationFeesRequestDto
            {
                ProducerType = "LARGE",
                NumberOfSubsidiaries = 101,
                Regulator = RegulatorConstants.GBENG,
                IsOnlineMarketplace = false
            };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.NumberOfSubsidiaries)
                  .WithErrorMessage(ValidationMessages.NumberOfSubsidiariesRange);
        }

        [TestMethod]
        public void Validate_EmptyRegulator_ShouldHaveError()
        {
            // Arrange
            var request = new ProducerRegistrationFeesRequestDto
            {
                ProducerType = "LARGE",
                NumberOfSubsidiaries = 10,
                Regulator = string.Empty,
                IsOnlineMarketplace = false
            };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Regulator)
                  .WithErrorMessage(ValidationMessages.RegulatorRequired);
        }

        [TestMethod]
        public void Validate_EmptyProducerTypeAndZeroSubsidiaries_ShouldHaveError()
        {
            // Arrange
            var request = new ProducerRegistrationFeesRequestDto
            {
                ProducerType = string.Empty, // No base fee required
                NumberOfSubsidiaries = 0,
                Regulator = RegulatorConstants.GBENG,
                IsOnlineMarketplace = false
            };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.NumberOfSubsidiaries)
                  .WithErrorMessage(ValidationMessages.NumberOfSubsidiariesRequiredWhenProducerTypeEmpty);
        }

        [TestMethod]
        public void Validate_ValidRequestWithProducerType_ShouldNotHaveError()
        {
            // Arrange
            var request = new ProducerRegistrationFeesRequestDto
            {
                ProducerType = "LARGE",
                NumberOfSubsidiaries = 10,
                Regulator = RegulatorConstants.GBENG,
                IsOnlineMarketplace = false
            };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.ProducerType);
            result.ShouldNotHaveValidationErrorFor(x => x.NumberOfSubsidiaries);
            result.ShouldNotHaveValidationErrorFor(x => x.Regulator);
        }

        [TestMethod]
        public void Validate_EmptyProducerTypeAndGreaterThanZeroSubsidiaries_ShouldNotHaveError()
        {
            // Arrange
            var request = new ProducerRegistrationFeesRequestDto
            {
                ProducerType = string.Empty, // No base fee required
                NumberOfSubsidiaries = 10,
                Regulator = RegulatorConstants.GBENG,
                IsOnlineMarketplace = false
            };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.NumberOfSubsidiaries);
            result.ShouldNotHaveValidationErrorFor(x => x.Regulator);
        }

        [TestMethod]
        public void Validate_ValidRegulator_ShouldNotHaveError()
        {
            // Arrange
            var validRegulators = new[] { RegulatorConstants.GBENG, RegulatorConstants.GBSCT, RegulatorConstants.GBWLS, RegulatorConstants.GBNIR };

            foreach (var regulator in validRegulators)
            {
                var request = new ProducerRegistrationFeesRequestDto
                {
                    ProducerType = "LARGE",
                    NumberOfSubsidiaries = 10,
                    Regulator = regulator,
                    IsOnlineMarketplace = false
                };

                // Act
                var result = _validator.TestValidate(request);

                // Assert
                result.ShouldNotHaveValidationErrorFor(x => x.Regulator);
            }
        }
    }
}
