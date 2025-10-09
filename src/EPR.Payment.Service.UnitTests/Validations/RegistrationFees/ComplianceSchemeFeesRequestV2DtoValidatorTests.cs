using EPR.Payment.Service.Common.Constants.RegistrationFees;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Validations.RegistrationFees.ComplianceScheme;
using FluentAssertions;
using FluentValidation.TestHelper;

namespace EPR.Payment.Service.UnitTests.Validations.RegistrationFees
{
    [TestClass]
    public class ComplianceSchemeFeesRequestV2DtoValidatorTests
    {
        private ComplianceSchemeFeesRequestV2DtoValidator _validator = null!;

        [TestInitialize]
        public void TestInitialize()
        {
            _validator = new ComplianceSchemeFeesRequestV2DtoValidator();
        }

        [TestMethod]
        public void Validator_Should_Fail_When_Regulator_Is_Empty()
        {
            // Arrange
            var dto = new ComplianceSchemeFeesRequestV2Dto
            {
                Regulator = "",
                ApplicationReferenceNumber = "Ref123",
                SubmissionDate = DateTime.UtcNow,
                FileId = Guid.NewGuid(),
                ExternalId = Guid.NewGuid(),
                InvoicePeriod = new DateTimeOffset(),
                PayerId = 1,
                PayerTypeId = 1,
                ComplianceSchemeMembers = new List<ComplianceSchemeMemberDto>()
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Regulator)
                  .WithErrorMessage(ValidationMessages.RegulatorRequired);
        }

        [TestMethod]
        public void Validator_Should_Fail_When_ApplicationReferenceNumber_Is_Empty()
        {
            // Arrange
            var dto = new ComplianceSchemeFeesRequestV2Dto
            {
                Regulator = RegulatorConstants.GBENG,
                ApplicationReferenceNumber = "",
                SubmissionDate = DateTime.UtcNow,
                FileId = Guid.NewGuid(),
                ExternalId = Guid.NewGuid(),
                InvoicePeriod = new DateTimeOffset(),
                PayerId = 1,
                PayerTypeId = 1,
                ComplianceSchemeMembers = new List<ComplianceSchemeMemberDto>()
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.ApplicationReferenceNumber)
                  .WithErrorMessage(ValidationMessages.ApplicationReferenceNumberRequired);
        }

        [TestMethod]
        public void Validator_Should_Validate_ComplianceSchemeMember()
        {
            // Arrange
            var dto = new ComplianceSchemeFeesRequestV2Dto
            {
                Regulator = RegulatorConstants.GBENG,
                ApplicationReferenceNumber = "Ref123",
                SubmissionDate = DateTime.UtcNow,
                FileId = Guid.NewGuid(),
                ExternalId = Guid.NewGuid(),
                InvoicePeriod = new DateTimeOffset(),
                PayerId = 1,
                PayerTypeId = 1,
                ComplianceSchemeMembers = new List<ComplianceSchemeMemberDto>
                {
                    new ComplianceSchemeMemberDto { MemberId = "123", MemberType = "Large", NumberOfSubsidiaries = 2, NoOfSubsidiariesOnlineMarketplace = 1 },
                    new ComplianceSchemeMemberDto { MemberId = string.Empty, MemberType = "Small", NumberOfSubsidiaries = -1, NoOfSubsidiariesOnlineMarketplace = 1 }
                }
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.Errors.Should().ContainSingle(e => e.PropertyName == "ComplianceSchemeMembers[1].MemberId" && 
                                   e.ErrorMessage == ValidationMessages.InvalidMemberId);

            result.Errors.Should().ContainSingle(e => e.PropertyName == "ComplianceSchemeMembers[1].NumberOfSubsidiaries" && 
                                   e.ErrorMessage == ValidationMessages.NumberOfSubsidiariesRange);
        }

        [TestMethod]
        public void Validator_Should_Fail_When_SubmissionDate_Is_Not_Valid()
        {
            // Arrange
            var dto = new ComplianceSchemeFeesRequestV2Dto
            {
                Regulator = RegulatorConstants.GBENG,
                ApplicationReferenceNumber = "Ref123",
                SubmissionDate = default(DateTime),
                FileId = Guid.NewGuid(),
                ExternalId = Guid.NewGuid(),
                InvoicePeriod = new DateTimeOffset(),
                PayerId = 1,
                PayerTypeId = 1,
                ComplianceSchemeMembers = new List<ComplianceSchemeMemberDto>()
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.SubmissionDate)
                  .WithErrorMessage(ValidationMessages.InvalidSubmissionDate);
        }

        [TestMethod]
        public void Validator_Should_Fail_When_SubmissionDate_Is_Future_Date()
        {
            // Arrange
            var dto = new ComplianceSchemeFeesRequestV2Dto
            {
                Regulator = RegulatorConstants.GBENG,
                ApplicationReferenceNumber = "Ref123",
                SubmissionDate = DateTime.UtcNow.AddDays(10),
                FileId = Guid.NewGuid(),
                ExternalId = Guid.NewGuid(),
                InvoicePeriod = new DateTimeOffset(),
                PayerId = 1,
                PayerTypeId = 1,
                ComplianceSchemeMembers = new List<ComplianceSchemeMemberDto>()
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.SubmissionDate)
                  .WithErrorMessage(ValidationMessages.FutureSubmissionDate);
        }

        [TestMethod]
        public void Validate_SubmissionDateNotUtc_ShouldHaveError()
        {
            // Arrange
            var dto = new ComplianceSchemeFeesRequestV2Dto
            {
                Regulator = RegulatorConstants.GBENG,
                ApplicationReferenceNumber = "Ref123",
                SubmissionDate = DateTime.Now,
                FileId = Guid.NewGuid(),
                ExternalId = Guid.NewGuid(),
                InvoicePeriod = new DateTimeOffset(),
                PayerId = 1,
                PayerTypeId = 1,
                ComplianceSchemeMembers = new List<ComplianceSchemeMemberDto>()
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.SubmissionDate)
                .WithErrorMessage(ValidationMessages.SubmissionDateMustBeUtc);
        }

        [TestMethod]
        public void Validator_Should_Fail_When_FileId_Is_Empty()
        {
            // Arrange
            var dto = new ComplianceSchemeFeesRequestV2Dto
            {
                Regulator = "",
                ApplicationReferenceNumber = "Ref123",
                SubmissionDate = DateTime.UtcNow,
                FileId = Guid.Empty,
                ExternalId = Guid.NewGuid(),
                InvoicePeriod = new DateTimeOffset(),
                PayerId = 1,
                PayerTypeId = 1,
                ComplianceSchemeMembers = new List<ComplianceSchemeMemberDto>()
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.FileId)
                  .WithErrorMessage(ValidationMessages.FileIdRequired);
        }

        [TestMethod]
        public void Validator_Should_Fail_When_ExternalId_Is_Empty()
        {
            // Arrange
            var dto = new ComplianceSchemeFeesRequestV2Dto
            {
                Regulator = "",
                ApplicationReferenceNumber = "Ref123",
                SubmissionDate = DateTime.UtcNow,
                FileId = Guid.NewGuid(),
                ExternalId = Guid.Empty,
                InvoicePeriod = new DateTimeOffset(),
                PayerId = 1,
                PayerTypeId = 1,
                ComplianceSchemeMembers = new List<ComplianceSchemeMemberDto>()
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.ExternalId)
                  .WithErrorMessage(ValidationMessages.ExternalIdRequired);
        }

        public void Validator_Should_Fail_When_InvoicePeriod_Is_Not_Valid()
        {
            // Arrange
            var dto = new ComplianceSchemeFeesRequestV2Dto
            {
                Regulator = "Abc regulator",
                ApplicationReferenceNumber = "Ref123",
                SubmissionDate = DateTime.UtcNow,
                FileId = Guid.NewGuid(),
                ExternalId = Guid.NewGuid(),
                InvoicePeriod = new DateTimeOffset(),
                PayerId = 1,
                PayerTypeId = 1,
                ComplianceSchemeMembers = new List<ComplianceSchemeMemberDto>()
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.InvoicePeriod)
                  .WithErrorMessage(ValidationMessages.InvoicePeriodRequired);
        }

        [TestMethod]
        public void Validator_Should_Fail_When_PayerTypeId_Is_Not_Valid()
        {
            // Arrange
            var dto = new ComplianceSchemeFeesRequestV2Dto
            {
                Regulator = "",
                ApplicationReferenceNumber = "Ref123",
                SubmissionDate = DateTime.UtcNow,
                FileId = Guid.NewGuid(),
                ExternalId = Guid.NewGuid(),
                InvoicePeriod = new DateTimeOffset(),
                PayerId = 1,
                PayerTypeId = 0,
                ComplianceSchemeMembers = new List<ComplianceSchemeMemberDto>()
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.PayerTypeId)
                  .WithErrorMessage(ValidationMessages.PayerTypeIdRequired);
        }

        [TestMethod]
        public void Validator_Should_Fail_When_PayerId_Is_Not_Valid()
        {
            // Arrange
            var dto = new ComplianceSchemeFeesRequestV2Dto
            {
                Regulator = "",
                ApplicationReferenceNumber = "Ref123",
                SubmissionDate = DateTime.UtcNow,
                FileId = Guid.NewGuid(),
                ExternalId = Guid.NewGuid(),
                InvoicePeriod = new DateTimeOffset(),
                PayerId = 0,
                PayerTypeId = 1,
                ComplianceSchemeMembers = new List<ComplianceSchemeMemberDto>()
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.PayerId)
                  .WithErrorMessage(ValidationMessages.PayerIdRequired);
        }
    }
}
