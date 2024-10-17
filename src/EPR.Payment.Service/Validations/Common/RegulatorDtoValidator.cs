using EPR.Payment.Service.Common.Constants.RegistrationFees;
using EPR.Payment.Service.Common.Dtos.Request.Common;
using FluentValidation;

namespace EPR.Payment.Service.Validations.Common
{
    public class RegulatorDtoValidator : AbstractValidator<RegulatorDto>
    {
        public RegulatorDtoValidator()
        {
            RuleFor(x => x.Regulator)
                .NotEmpty().WithMessage(ValidationMessages.RegulatorRequired)
                .Must(RegulatorValidationHelper.IsValidRegulator).WithMessage(ValidationMessages.RegulatorInvalid);
        }
    }
}
