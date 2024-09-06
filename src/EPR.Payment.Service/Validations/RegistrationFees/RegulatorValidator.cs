using FluentValidation;

namespace EPR.Payment.Service.Validations.RegistrationFees
{
    public class RegulatorValidator : AbstractValidator<string>
    {
        public RegulatorValidator()
        {
            RuleFor(x => x)
                .NotEmpty().WithMessage("Regulator is required.")
                .Must(x => x.ToUpper() == x).WithMessage("Regulator must be in uppercase.")
                .Must(RegulatorValidationHelper.IsValidRegulator).WithMessage("Invalid regulator parameter.");
        }
    }
}
