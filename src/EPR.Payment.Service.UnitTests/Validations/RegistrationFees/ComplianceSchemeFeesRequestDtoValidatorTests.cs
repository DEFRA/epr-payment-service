using EPR.Payment.Service.Common.Constants.RegistrationFees;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Validations.RegistrationFees.ComplianceScheme;
using FluentAssertions;
using FluentValidation.TestHelper;

namespace EPR.Payment.Service.UnitTests.Validations.RegistrationFees
{
    [TestClass]
    public class ComplianceSchemeFeesRequestDtoValidatorTests
    {
        private ComplianceSchemeFeesRequestDtoValidator _validator = null!;

        [TestInitialize]
        public void TestInitialize()
        {
            _validator = new ComplianceSchemeFeesRequestDtoValidator();
        }

        [TestMethod]
        public void Validator_Should_Fail_When_Regulator_Is_Empty()
        {
            // Arrange
            var dto = new ComplianceSchemeFeesRequestDto
            {
                Regulator = "",
                ApplicationReferenceNumber = "Ref123",
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
            var dto = new ComplianceSchemeFeesRequestDto
            {
                Regulator = "ValidRegulator",
                ApplicationReferenceNumber = "",
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
            var dto = new ComplianceSchemeFeesRequestDto
            {
                Regulator = "ValidRegulator",
                ApplicationReferenceNumber = "Ref123",
                ComplianceSchemeMembers = new List<ComplianceSchemeMemberDto>
                {
                    new ComplianceSchemeMemberDto { MemberId = 1, MemberType = "Large", NumberOfSubsidiaries = 2, NoOfSubsidiariesOnlineMarketplace = 1 },
                    new ComplianceSchemeMemberDto { MemberId = 0, MemberType = "Small", NumberOfSubsidiaries = -1, NoOfSubsidiariesOnlineMarketplace = 1 }
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
    }
}
