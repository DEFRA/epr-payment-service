using EPR.Payment.Service.Common.Constants.RegistrationFees;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.ComplianceScheme;
using FluentValidation;

namespace EPR.Payment.Service.Validations.RegistrationFees.ComplianceScheme
{
    public class ComplianceSchemeFeesRequestDtoValidator : AbstractValidator<ComplianceSchemeFeesRequestDto>
    {
        public ComplianceSchemeFeesRequestDtoValidator()
        {
            RuleFor(x => x.Regulator)
                    .NotEmpty().WithMessage(ValidationMessages.RegulatorRequired)
                    .Must(RegulatorValidationHelper.IsValidRegulator).WithMessage(ValidationMessages.RegulatorInvalid);

            RuleFor(x => x.ApplicationReferenceNumber)
                    .NotEmpty().WithMessage(ValidationMessages.ApplicationReferenceNumberRequired);

            RuleForEach(x => x.ComplianceSchemeMembers)
            .SetValidator(new ComplianceSchemeMemberDtoValidator())
            .WithMessage(ValidationMessages.InvalidComplianceSchemeMember);
        }
    }
}
