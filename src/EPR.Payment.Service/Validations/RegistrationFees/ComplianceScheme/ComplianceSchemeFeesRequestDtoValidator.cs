using EPR.Payment.Service.Common.Constants.RegistrationFees;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Validations.Common;
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

            RuleFor(x => x.SubmissionDate)
                .Must(date => date != default(DateTime))
                .WithMessage(ValidationMessages.InvalidSubmissionDate)
                .Must(date => date <= DateTime.Now)
                .WithMessage(ValidationMessages.FutureSubmissionDate);

            RuleForEach(x => x.ComplianceSchemeMembers)
            .SetValidator(new ComplianceSchemeMemberDtoValidator())
            .WithMessage(ValidationMessages.InvalidComplianceSchemeMember);
        }
    }
}
