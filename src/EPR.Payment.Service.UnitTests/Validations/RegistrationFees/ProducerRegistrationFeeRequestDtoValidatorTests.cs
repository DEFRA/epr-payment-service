using EPR.Payment.Service.Common.Constants.RegistrationFees;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.Producer;
using EPR.Payment.Service.Validations.RegistrationFees.Producer;
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
                IsProducerOnlineMarketplace = false,
                ApplicationReferenceNumber = "A123",
                SubmissionDate = DateTime.Now
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
                IsProducerOnlineMarketplace = false,
                ApplicationReferenceNumber = "A123",
                SubmissionDate = DateTime.Now
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
                IsProducerOnlineMarketplace = false,
                ApplicationReferenceNumber = "A123",
                SubmissionDate = DateTime.Now
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
                IsProducerOnlineMarketplace = false,
                ApplicationReferenceNumber = "A123",
                SubmissionDate = DateTime.Now
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
                IsProducerOnlineMarketplace = false,
                ApplicationReferenceNumber = "A123",
                SubmissionDate = DateTime.Now
            };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.ProducerType);
        }

        [TestMethod]
        public void Validate_NumberOfSubsidiariesLessThanZero_ShouldHaveError()
        {
            // Arrange
            var request = new ProducerRegistrationFeesRequestDto
            {
                ProducerType = "LARGE",
                NumberOfSubsidiaries = -5,
                Regulator = RegulatorConstants.GBENG,
                IsProducerOnlineMarketplace = false,
                ApplicationReferenceNumber = "A123",
                SubmissionDate = DateTime.Now
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
                IsProducerOnlineMarketplace = false,
                ApplicationReferenceNumber = "A123",
                SubmissionDate = DateTime.Now
            };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Regulator)
                  .WithErrorMessage(ValidationMessages.RegulatorRequired);
        }

        [TestMethod]
        public void Validate_EmptyProducerType_ShouldHaveError()
        {
            // Arrange
            var validProducerTypes = new List<string> { "LARGE", "SMALL" };
            var request = new ProducerRegistrationFeesRequestDto
            {
                ProducerType = string.Empty,
                NumberOfSubsidiaries = 10,
                Regulator = RegulatorConstants.GBENG,
                IsProducerOnlineMarketplace = false,
                ApplicationReferenceNumber = "A123",
                SubmissionDate = DateTime.Now
            };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.ProducerType)
                  .WithErrorMessage(ValidationMessages.ProducerTypeInvalid + string.Join(", ", validProducerTypes));
        }

        [TestMethod]
        public void Validate_NumberOfOMPSubsidiaries_ShouldBeLessThanOrEqualToNumberOfSubsidiaries()
        {
            // Arrange
            var request = new ProducerRegistrationFeesRequestDto
            {
                ProducerType = "Large",
                NumberOfSubsidiaries = 10,
                NoOfSubsidiariesOnlineMarketplace = 11,
                Regulator = RegulatorConstants.GBENG,
                IsProducerOnlineMarketplace = false,
                ApplicationReferenceNumber = "A123",
                SubmissionDate = DateTime.Now
            };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.NoOfSubsidiariesOnlineMarketplace)
                  .WithErrorMessage(ValidationMessages.NumberOfOMPSubsidiariesLessThanOrEqualToNumberOfSubsidiaries);
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
                IsProducerOnlineMarketplace = false,
                ApplicationReferenceNumber = "A123",
                SubmissionDate = DateTime.Now
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
                IsProducerOnlineMarketplace = false,
                ApplicationReferenceNumber = "A123",
                SubmissionDate = DateTime.Now
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
                    IsProducerOnlineMarketplace = false,
                    ApplicationReferenceNumber = "A123",
                    SubmissionDate = DateTime.Now
                };

                // Act
                var result = _validator.TestValidate(request);

                // Assert
                result.ShouldNotHaveValidationErrorFor(x => x.Regulator);
            }
        }


        [TestMethod]
        public void Validate_EmptyApplicationReferenceNumber_ShouldHaveError()
        {
            // Arrange
            var request = new ProducerRegistrationFeesRequestDto
            {
                ProducerType = "LARGE",
                NumberOfSubsidiaries = 10,
                Regulator = RegulatorConstants.GBENG,
                IsProducerOnlineMarketplace = false,
                ApplicationReferenceNumber = string.Empty,
                SubmissionDate = DateTime.Now
            };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.ApplicationReferenceNumber)
                  .WithErrorMessage(ValidationMessages.ApplicationReferenceNumberRequired);
        }

        [TestMethod]
        public void Validate_NoOfSubsidiariesOnlineMarketplaceLessThanZero_ShouldHaveError()
        {
            // Arrange
            var request = new ProducerRegistrationFeesRequestDto
            {
                ProducerType = "LARGE",
                NumberOfSubsidiaries = 0,
                Regulator = RegulatorConstants.GBENG,
                IsProducerOnlineMarketplace = false,
                NoOfSubsidiariesOnlineMarketplace = -5,
                ApplicationReferenceNumber = "A123",
                SubmissionDate = DateTime.Now
            };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.NoOfSubsidiariesOnlineMarketplace)
                  .WithErrorMessage(ValidationMessages.NoOfSubsidiariesOnlineMarketplaceRange);
        }

        [TestMethod]
        public void Validate_InvalidSubmissionDate_ShouldHaveError()
        {
            // Arrange
            var request = new ProducerRegistrationFeesRequestDto
            {
                ProducerType = "LARGE",
                NumberOfSubsidiaries = 10,
                Regulator = RegulatorConstants.GBENG,
                IsProducerOnlineMarketplace = false,
                IsLateFeeApplicable = false,
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
            var request = new ProducerRegistrationFeesRequestDto
            {
                ProducerType = "LARGE",
                NumberOfSubsidiaries = 10,
                Regulator = RegulatorConstants.GBENG,
                IsProducerOnlineMarketplace = false,
                IsLateFeeApplicable = false,
                ApplicationReferenceNumber = "A123",
                SubmissionDate = DateTime.Now.AddDays(10)
            };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.SubmissionDate)
                  .WithErrorMessage(ValidationMessages.FutureSubmissionDate);
        }
    }
}
