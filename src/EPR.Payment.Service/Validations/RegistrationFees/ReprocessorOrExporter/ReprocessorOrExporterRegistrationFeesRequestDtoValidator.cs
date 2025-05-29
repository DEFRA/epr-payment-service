using EPR.Payment.Service.Common.Constants.RegistrationFees;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.ReprocessorOrExporter;
using EPR.Payment.Service.Validations.Common;
using FluentValidation;

namespace EPR.Payment.Service.Validations.RegistrationFees.ReprocessorOrExporter
{
    public class ReprocessorOrExporterRegistrationFeesRequestDtoValidator : AbstractValidator<ReprocessorOrExporterRegistrationFeesRequestDto>
    {
        public ReprocessorOrExporterRegistrationFeesRequestDtoValidator()
        {
            RuleFor(x => x.RequestorType)
                .NotEmpty().WithMessage(ValidationMessages.EmptyRequestorType);

            RuleFor(x => x.MaterialType)
                .NotEmpty().WithMessage(ValidationMessages.EmptyMaterialType);

            RuleFor(x => x.Regulator)
                .NotEmpty().WithMessage(ValidationMessages.RegulatorRequired)
                .Must(RegulatorValidationHelper.IsValidRegulator).WithMessage(ValidationMessages.RegulatorInvalid);

            RuleFor(x => x.SubmissionDate)
                 .Cascade(CascadeMode.Stop)
                 .MustBeValidSubmissionDate();
        }
    }
}
