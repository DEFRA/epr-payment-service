using EPR.Payment.Service.Common.Constants.RegistrationFees;
using EPR.Payment.Service.Common.Dtos.Request.ResubmissionFees.ComplianceScheme;
using EPR.Payment.Service.Validations.Common;
using FluentValidation;

namespace EPR.Payment.Service.Validations.ResubmissionFees.ComplianceScheme
{
    public class ComplianceSchemeResubmissionFeeRequestDtoValidator : AbstractValidator<ComplianceSchemeResubmissionFeeRequestDto>
    {
        public ComplianceSchemeResubmissionFeeRequestDtoValidator()
        {
            RuleFor(x => x.Regulator)
                .NotEmpty().WithMessage(ValidationMessages.RegulatorRequired)
                .Must(RegulatorValidationHelper.IsValidRegulator).WithMessage(ValidationMessages.RegulatorInvalid);

            RuleFor(x => x.ResubmissionDate)
                .NotEmpty().WithMessage(ValidationMessages.ResubmissionDateRequired)
                .Must(date => date.Kind == DateTimeKind.Utc).WithMessage(ValidationMessages.ResubmissionDateMustBeUtc)
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage(ValidationMessages.ResubmissionDateInvalid);

            RuleFor(x => x.ReferenceNumber)
                .NotEmpty().WithMessage(ValidationMessages.ReferenceNumberRequired);

            RuleFor(x => x.MemberCount)
                .GreaterThan(0).WithMessage(ValidationMessages.MemberCountGreaterThanZero);
        }
    }
}
