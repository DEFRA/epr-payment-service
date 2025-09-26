using EPR.Payment.Service.Common.Constants.RegistrationFees;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Validations.Common;
using FluentValidation;

namespace EPR.Payment.Service.Validations.RegistrationFees.ComplianceScheme
{
    public class ComplianceSchemeFeesRequestV3DtoValidator : AbstractValidator<ComplianceSchemeFeesRequestV3Dto>
    {
        public ComplianceSchemeFeesRequestV3DtoValidator()
        {
            RuleFor(x => x.Regulator)
                    .NotEmpty().WithMessage(ValidationMessages.RegulatorRequired)
                    .Must(RegulatorValidationHelper.IsValidRegulator).WithMessage(ValidationMessages.RegulatorInvalid);

            RuleFor(x => x.ApplicationReferenceNumber)
                    .NotEmpty().WithMessage(ValidationMessages.ApplicationReferenceNumberRequired);

            RuleFor(x => x.SubmissionDate)
                    .Cascade(CascadeMode.Stop)
                    .MustBeValidSubmissionDate();

            RuleFor(x => x.FileId)
                    .NotEmpty().WithMessage(ValidationMessages.FileIdRequired);

            RuleFor(x => x.ExternalId)
                    .NotEmpty().WithMessage(ValidationMessages.ExternalIdRequired);

            RuleFor(x => x.InvoicePeriod)
                    .NotEmpty().WithMessage(ValidationMessages.InvoicePeriodRequired);

            RuleFor(x => x.PayerTypeId)
                    .LessThan(1).WithMessage(ValidationMessages.PayerTypeIdRequired);

            RuleFor(x => x.PayerId)
                    .LessThan(1).WithMessage(ValidationMessages.PayerIdRequired);

            RuleForEach(x => x.ComplianceSchemeMembers)
                    .SetValidator(new ComplianceSchemeMemberDtoValidator())
                    .WithMessage(ValidationMessages.InvalidComplianceSchemeMember);
        }
    }
}
