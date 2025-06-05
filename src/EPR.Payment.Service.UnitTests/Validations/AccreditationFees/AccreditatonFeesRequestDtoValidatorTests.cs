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
        private ReprocessorOrExporterAccreditationFeesRequestDtoValidator _validator = null!;

        [TestInitialize]
        public void Setup()
        {
            _validator = new ReprocessorOrExporterAccreditationFeesRequestDtoValidator();
        }

        #region Regulator Tests 

        [TestMethod]
        public void Validate_InvalidRegulator_ShouldHaveError()
        {
            // Arrange
            var request = new ReprocessorOrExporterAccreditationFeesRequestDto
            {
                Regulator = "X",
                RequestorType = RequestorTypes.Exporters,
                MaterialType = MaterialTypes.Aluminium,
                NumberOfOverseasSites = 0,               
                TonnageBand = TonnageBands.Upto500,
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
            var request = new ReprocessorOrExporterAccreditationFeesRequestDto
            {
                Regulator = string.Empty,
                RequestorType = RequestorTypes.Exporters,
                MaterialType = MaterialTypes.Aluminium,
                NumberOfOverseasSites = 0,
                TonnageBand = TonnageBands.Upto500,
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
        public void Validate_ValidRegulator_ShouldNotHaveError()
        {
            // Arrange
            var validRegulators = new[] { RegulatorConstants.GBENG, RegulatorConstants.GBSCT, RegulatorConstants.GBWLS, RegulatorConstants.GBNIR };

            foreach (var regulator in validRegulators)
            {
                // Arrange
                var request = new ReprocessorOrExporterAccreditationFeesRequestDto
                {
                    Regulator = regulator,
                    RequestorType = RequestorTypes.Exporters,
                    MaterialType = MaterialTypes.Aluminium,
                    NumberOfOverseasSites = 0,
                    TonnageBand = TonnageBands.Upto500,
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
            var request = new ReprocessorOrExporterAccreditationFeesRequestDto
            {
                Regulator = RegulatorConstants.GBENG,
                RequestorType = RequestorTypes.Exporters,
                MaterialType = MaterialTypes.Aluminium,
                NumberOfOverseasSites = 0,
                TonnageBand = TonnageBands.Upto500,
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
            var request = new ReprocessorOrExporterAccreditationFeesRequestDto
            {
                Regulator = RegulatorConstants.GBENG,
                RequestorType = RequestorTypes.Exporters,
                MaterialType = MaterialTypes.Aluminium,
                NumberOfOverseasSites = 0,
                TonnageBand = TonnageBands.Upto500,
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
            var request = new ReprocessorOrExporterAccreditationFeesRequestDto
            {
                Regulator = RegulatorConstants.GBENG,
                RequestorType = RequestorTypes.Exporters,
                MaterialType = MaterialTypes.Aluminium,
                NumberOfOverseasSites = 0,
                TonnageBand = TonnageBands.Upto500,
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

        #region Number of Overseas site Tests 

        [TestMethod]
        public void Validate_InValidOverseasSiteForReprocessor_ShouldHaveError()
        {
            // Arrange
            var request = new ReprocessorOrExporterAccreditationFeesRequestDto
            {
                Regulator = RegulatorConstants.GBENG,
                RequestorType = RequestorTypes.Reprocessors,
                MaterialType = MaterialTypes.Aluminium,
                NumberOfOverseasSites = 10,
                TonnageBand = TonnageBands.Upto500,
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
        public void Validate_InValidOverseasSiteIsZeroForExporter_ShouldHaveError()
        {
            // Arrange
            var request = new ReprocessorOrExporterAccreditationFeesRequestDto
            {
                Regulator = RegulatorConstants.GBENG,
                RequestorType = RequestorTypes.Exporters,
                MaterialType = MaterialTypes.Aluminium,
                NumberOfOverseasSites = 0,
                TonnageBand = TonnageBands.Upto500,
                ApplicationReferenceNumber = "A123",
                SubmissionDate = DateTime.UtcNow.AddMinutes(-1)
            };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.NumberOfOverseasSites);                  
        }

        [TestMethod]
        public void Validate_InValidOverseasSiteIsTooBigNumberForExporter_ShouldHaveError()
        {
            // Arrange
            var request = new ReprocessorOrExporterAccreditationFeesRequestDto
            {
                Regulator = RegulatorConstants.GBENG,
                RequestorType = RequestorTypes.Exporters,
                MaterialType = MaterialTypes.Aluminium,
                NumberOfOverseasSites = 471,
                TonnageBand = TonnageBands.Upto500,
                ApplicationReferenceNumber = "A123",
                SubmissionDate = DateTime.UtcNow.AddMinutes(-1)
            };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.NumberOfOverseasSites);                  
        }

        #endregion

        #region Requestor Type Tests 

        [TestMethod]
        public void Validate_EmptyRequestorType_ShouldHaveError()
        {
            // Arrange
            var request = new ReprocessorOrExporterAccreditationFeesRequestDto
            {
                Regulator = RegulatorConstants.GBENG,
                MaterialType = MaterialTypes.Aluminium,
                NumberOfOverseasSites = 0,
                TonnageBand = TonnageBands.Upto500,
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
            var validRequestorTypes = new[] { RequestorTypes.Exporters, RequestorTypes.Reprocessors };

            foreach (var requestorType in validRequestorTypes)
            {
                // Arrange
                var request = new ReprocessorOrExporterAccreditationFeesRequestDto
                {
                    Regulator = RegulatorConstants.GBENG,
                    RequestorType = requestorType,
                    MaterialType = MaterialTypes.Aluminium,
                    NumberOfOverseasSites = 0,
                    TonnageBand = TonnageBands.Upto500,
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
            var request = new ReprocessorOrExporterAccreditationFeesRequestDto
            {
                Regulator = RegulatorConstants.GBENG,
                RequestorType = RequestorTypes.Exporters,              
                NumberOfOverseasSites = 10,
                TonnageBand = TonnageBands.Upto500,
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
            var validMaterialTypes = new[] { MaterialTypes.Aluminium,
                MaterialTypes.Plastic,
                MaterialTypes.Glass,
                MaterialTypes.PaperOrBoardOrFibreBasedCompositeMaterial,
                MaterialTypes.Wood };

            foreach (var materialType in validMaterialTypes)
            {
                // Arrange
                var request = new ReprocessorOrExporterAccreditationFeesRequestDto
                {
                    Regulator = RegulatorConstants.GBENG,
                    RequestorType = RequestorTypes.Exporters,
                    MaterialType = materialType,
                    NumberOfOverseasSites = 10,
                    TonnageBand = TonnageBands.Upto500,
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
            var request = new ReprocessorOrExporterAccreditationFeesRequestDto
            {
                Regulator = RegulatorConstants.GBENG,
                RequestorType = RequestorTypes.Exporters,
                MaterialType = MaterialTypes.Aluminium,
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
            var validTonnageBands = new[] { TonnageBands.Upto500,
                TonnageBands.Over500To5000,
                TonnageBands.Over5000To10000,
                TonnageBands.Over10000,
                };

            foreach (var tonnageBand in validTonnageBands)
            {
                // Arrange
                var request = new ReprocessorOrExporterAccreditationFeesRequestDto
                {
                    Regulator = RegulatorConstants.GBENG,
                    RequestorType = RequestorTypes.Exporters,
                    MaterialType = MaterialTypes.Plastic,
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
            var accreditationFeesRequestDtoList = new List<ReprocessorOrExporterAccreditationFeesRequestDto>
            {
                new ()
                {
                    Regulator = RegulatorConstants.GBENG,
                    RequestorType = RequestorTypes.Reprocessors,
                    MaterialType = MaterialTypes.Aluminium,
                    NumberOfOverseasSites = 0,
                    TonnageBand = TonnageBands.Upto500,
                    ApplicationReferenceNumber = "A123",
                    SubmissionDate = DateTime.UtcNow.AddMinutes(-1)
                },

                new ()
                {
                    Regulator = RegulatorConstants.GBENG,
                    RequestorType = RequestorTypes.Exporters,
                    MaterialType = MaterialTypes.Aluminium,
                    NumberOfOverseasSites = 440,
                    TonnageBand = TonnageBands.Upto500,
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
