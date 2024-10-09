using EPR.Payment.Service.Common.Constants.RegistrationFees;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Validations.RegistrationFees.ComplianceScheme;
using FluentValidation.TestHelper;

namespace EPR.Payment.Service.UnitTests.Validations.RegistrationFees
{
    [TestClass]
    public class ComplianceSchemeMemberDtoValidatorTests
    {
        private ComplianceSchemeMemberDtoValidator _validator = null!;

        [TestInitialize]
        public void TestInitialize()
        {
            _validator = new ComplianceSchemeMemberDtoValidator();
        }

        [TestMethod]
        public void Validator_Should_Fail_When_MemberId_Is_Zero()
        {
            // Arrange
            var dto = new ComplianceSchemeMemberDto
            {
                MemberId = 0,
                MemberType = "Large",
                NumberOfSubsidiaries = 1,
                NoOfSubsidiariesOnlineMarketplace = 0
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.MemberId)
                  .WithErrorMessage(ValidationMessages.InvalidMemberId);
        }

        [TestMethod]
        public void Validator_Should_Fail_When_MemberType_Is_Empty()
        {
            // Arrange
            var dto = new ComplianceSchemeMemberDto
            {
                MemberId = 1,
                MemberType = "",
                NumberOfSubsidiaries = 1,
                NoOfSubsidiariesOnlineMarketplace = 0
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.MemberType)
                  .WithErrorMessage(ValidationMessages.MemberTypeRequired);
        }

        [TestMethod]
        public void Validator_Should_Fail_When_MemberType_Is_Invalid()
        {
            // Arrange
            var dto = new ComplianceSchemeMemberDto
            {
                MemberId = 1,
                MemberType = "InvalidType",
                NumberOfSubsidiaries = 1,
                NoOfSubsidiariesOnlineMarketplace = 0
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.MemberType)
                  .WithErrorMessage(ValidationMessages.InvalidMemberType);
        }

        [TestMethod]
        public void Validator_Should_Fail_When_NumberOfSubsidiaries_Is_Negative()
        {
            // Arrange
            var dto = new ComplianceSchemeMemberDto
            {
                MemberId = 1,
                MemberType = "Large",
                NumberOfSubsidiaries = -1,
                NoOfSubsidiariesOnlineMarketplace = 0
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.NumberOfSubsidiaries)
                  .WithErrorMessage(ValidationMessages.NumberOfSubsidiariesRange);
        }
    }
}
