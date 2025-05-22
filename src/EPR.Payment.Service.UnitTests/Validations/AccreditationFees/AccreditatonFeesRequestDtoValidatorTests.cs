using EPR.Payment.Service.Common.Constants.RegistrationFees;
using EPR.Payment.Service.Common.Dtos.Request.AccreditationFees;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.Producer;
using EPR.Payment.Service.Validations.AccreditationFees;
using FluentValidation.TestHelper;
using EPR.Payment.Service.Common.Enums;
using Azure.Core;

namespace EPR.Payment.Service.UnitTests.Validations.AccreditationFees
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

        #region Regulator Tests 

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
                  .WithErrorMessage(ValidationMessages.RegulatorRequired);
        }

        [TestMethod]
        public void Validate_ValidRegulator_ShouldNotHaveError()
        {
            // Arrange
            var validRegulators = new[] { RegulatorConstants.GBENG, RegulatorConstants.GBSCT, RegulatorConstants.GBWLS, RegulatorConstants.GBNIR };

            foreach (var regulator in validRegulators)
            {
                // Arrange
                var request = new AccreditationFeesRequestDto
                {
                    Regulator = regulator,
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
                result.ShouldNotHaveValidationErrorFor(x => x.Regulator);
            }
        }

        #endregion

        #region Submission Date Tests 

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

        #endregion

        #region Application Reference Number Tests 

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
                SubmissionDate = DateTime.UtcNow.AddMinutes(-1)
            };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.ApplicationReferenceNumber)
                  .WithErrorMessage(ValidationMessages.ApplicationReferenceNumberRequired);
        }

        #endregion
               
        #region Number of Overseas site Tests 

        [TestMethod]
        public void Validate_InValidOverseasSiteForReprocessor_ShouldHaveError()
        {
            // Arrange
            var request = new AccreditationFeesRequestDto
            {
                Regulator = RegulatorConstants.GBENG,
                RequestorType = AccreditationFeesRequestorType.Reprocessors,
                MaterialType = AccreditationFeesMaterialType.Aluminium,
                NumberOfOverseasSites = 10,
                TonnageBand = TonnageBand.Upto500,
                ApplicationReferenceNumber = "A123",
                SubmissionDate = DateTime.UtcNow.AddMinutes(-1)
            };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.NumberOfOverseasSites)
                  .WithErrorMessage(ValidationMessages.InvalidNumberOfOverseasSiteForReprocessor);
        }

        [TestMethod]
        public void Validate_InValidOverseasSiteForExporter_ShouldHaveError()
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
                SubmissionDate = DateTime.UtcNow.AddMinutes(-1)
            };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.NumberOfOverseasSites)
                  .WithErrorMessage(ValidationMessages.InvalidNumberOfOverseasSiteForExporter);
        }

        #endregion

        #region Requestor Type Tests 

        [TestMethod]
        public void Validate_EmptyRequestorType_ShouldHaveError()
        {
            // Arrange
            var request = new AccreditationFeesRequestDto
            {
                Regulator = RegulatorConstants.GBENG,
                MaterialType = AccreditationFeesMaterialType.Aluminium,
                NumberOfOverseasSites = 0,
                TonnageBand = TonnageBand.Upto500,
                ApplicationReferenceNumber = "A123",
                SubmissionDate = DateTime.UtcNow.AddMinutes(-1)
            };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.RequestorType);
        }

        [TestMethod]
        public void Validate_ValidRequestorType_ShouldNotHaveError()
        {
            // Arrange
            var validRequestorTypes = new[] { AccreditationFeesRequestorType.Exporters, AccreditationFeesRequestorType.Reprocessors };

            foreach (var requestorType in validRequestorTypes)
            {
                // Arrange
                var request = new AccreditationFeesRequestDto
                {
                    Regulator = RegulatorConstants.GBENG,
                    RequestorType = requestorType,
                    MaterialType = AccreditationFeesMaterialType.Aluminium,
                    NumberOfOverseasSites = 0,
                    TonnageBand = TonnageBand.Upto500,
                    ApplicationReferenceNumber = "A123",
                    SubmissionDate = DateTime.UtcNow
                };

                // Act
                var result = _validator.TestValidate(request);

                // Assert
                result.ShouldNotHaveValidationErrorFor(x => x.RequestorType);
            }
        }

        #endregion

        #region Material Type Tests 

        [TestMethod]
        public void Validate_EmptyMaterialType_ShouldHaveError()
        {
            // Arrange
            var request = new AccreditationFeesRequestDto
            {
                Regulator = RegulatorConstants.GBENG,
                RequestorType = AccreditationFeesRequestorType.Exporters,              
                NumberOfOverseasSites = 10,
                TonnageBand = TonnageBand.Upto500,
                ApplicationReferenceNumber = "A123",
                SubmissionDate = DateTime.UtcNow.AddMinutes(-1)
            };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.MaterialType);
        }

        [TestMethod]
        public void Validate_ValidMaterialType_ShouldNotHaveError()
        {
            // Arrange
            var validMaterialTypes = new[] { AccreditationFeesMaterialType.Aluminium,
                AccreditationFeesMaterialType.Plastic,
                AccreditationFeesMaterialType.Glass,
                AccreditationFeesMaterialType.PaperOrBoardOrFibreBasedCompositeMaterial,
                AccreditationFeesMaterialType.Wood };

            foreach (var materialType in validMaterialTypes)
            {
                // Arrange
                var request = new AccreditationFeesRequestDto
                {
                    Regulator = RegulatorConstants.GBENG,
                    RequestorType = AccreditationFeesRequestorType.Exporters,
                    MaterialType = materialType,
                    NumberOfOverseasSites = 10,
                    TonnageBand = TonnageBand.Upto500,
                    ApplicationReferenceNumber = "A123",
                    SubmissionDate = DateTime.UtcNow
                };

                // Act
                var result = _validator.TestValidate(request);

                // Assert
                result.ShouldNotHaveValidationErrorFor(x => x.MaterialType);
            }
        }

        #endregion

        #region Tonnage Band Tests 

        [TestMethod]
        public void Validate_EmptyTonnageBand_ShouldHaveError()
        {
            // Arrange
            var request = new AccreditationFeesRequestDto
            {
                Regulator = RegulatorConstants.GBENG,
                RequestorType = AccreditationFeesRequestorType.Exporters,
                MaterialType = AccreditationFeesMaterialType.Aluminium,
                NumberOfOverseasSites = 10,
                ApplicationReferenceNumber = "A123",
                SubmissionDate = DateTime.UtcNow.AddMinutes(-1)
            };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.TonnageBand);
        }

        [TestMethod]
        public void Validate_ValidTonnageBand_ShouldNotHaveError()
        {
            // Arrange
            var validTonnageBands = new[] { TonnageBand.Upto500,
                TonnageBand.Over500To5000,
                TonnageBand.Over5000To10000,
                TonnageBand.Over10000,
                };

            foreach (var tonnageBand in validTonnageBands)
            {
                // Arrange
                var request = new AccreditationFeesRequestDto
                {
                    Regulator = RegulatorConstants.GBENG,
                    RequestorType = AccreditationFeesRequestorType.Exporters,
                    MaterialType = AccreditationFeesMaterialType.Plastic,
                    NumberOfOverseasSites = 10,
                    TonnageBand = tonnageBand,
                    ApplicationReferenceNumber = "A123",
                    SubmissionDate = DateTime.UtcNow
                };

                // Act
                var result = _validator.TestValidate(request);

                // Assert
                result.ShouldNotHaveValidationErrorFor(x => x.TonnageBand);
            }
        }

        #endregion

        #region Valid Data

        [TestMethod]
        public void Validate_ValidData_ShouldNotHaveError()
        {
            // Arrange
            var accreditationFeesRequestDtoList = new List<AccreditationFeesRequestDto>
            {
                new ()
                {
                    Regulator = RegulatorConstants.GBENG,
                    RequestorType = AccreditationFeesRequestorType.Reprocessors,
                    MaterialType = AccreditationFeesMaterialType.Aluminium,
                    NumberOfOverseasSites = 0,
                    TonnageBand = TonnageBand.Upto500,
                    ApplicationReferenceNumber = "A123",
                    SubmissionDate = DateTime.UtcNow.AddMinutes(-1)
                },

                new ()
                {
                    Regulator = RegulatorConstants.GBENG,
                    RequestorType = AccreditationFeesRequestorType.Exporters,
                    MaterialType = AccreditationFeesMaterialType.Aluminium,
                    NumberOfOverseasSites = 10,
                    TonnageBand = TonnageBand.Upto500,
                    ApplicationReferenceNumber = "A123",
                    SubmissionDate = DateTime.UtcNow.AddMinutes(-1)
                }
            };

            foreach (var accreditationFeesRequestDto in accreditationFeesRequestDtoList)
            {
                // Act
                var result = _validator.TestValidate(accreditationFeesRequestDto);

                // Assert
                Assert.IsTrue(result.IsValid);
            }                
        }

        #endregion

    }
}
