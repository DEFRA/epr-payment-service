using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.Producer;
using FluentValidation;

namespace EPR.Payment.Service.Validations.RegistrationFees
{
    public class RegulatorDtoValidator : AbstractValidator<RegulatorDto>
    {
        public RegulatorDtoValidator()
        {
            RuleFor(x => x.Regulator)
                .NotEmpty().WithMessage("Regulator is required.")
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("Regulator is required.")
                .Must(x => x?.ToUpper() == x).WithMessage("Regulator must be in uppercase.")
                .Must(RegulatorValidationHelper.IsValidRegulator).WithMessage("Invalid regulator parameter.");
        }
    }
}
