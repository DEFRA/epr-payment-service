﻿using EPR.Payment.Service.Common.Constants.RegistrationFees;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees;
using FluentValidation;

namespace EPR.Payment.Service.Validations.RegistrationFees
{
    public class ProducerRegistrationFeesRequestDtoValidator : AbstractValidator<ProducerRegistrationFeesRequestDto>
    {
        public ProducerRegistrationFeesRequestDtoValidator()
        {
            var validProducerTypes = new List<string> { "LARGE", "SMALL" };
            RuleFor(x => x.ProducerType)
                .Must(pt => string.IsNullOrEmpty(pt) || validProducerTypes.Contains(pt.ToUpper()))
                .WithMessage(ValidationMessages.ProducerTypeInvalid + string.Join(", ", validProducerTypes));

            RuleFor(x => x.NumberOfSubsidiaries)
                .GreaterThanOrEqualTo(0).WithMessage(ValidationMessages.NumberOfSubsidiariesRange)
                .LessThanOrEqualTo(100).WithMessage(ValidationMessages.NumberOfSubsidiariesRange);

            RuleFor(x => x.NumberOfSubsidiaries)
            .GreaterThan(0)
                .When(x => string.IsNullOrEmpty(x.ProducerType))
                .WithMessage(ValidationMessages.NumberOfSubsidiariesRequiredWhenProducerTypeEmpty);
            RuleFor(x => x.Regulator)
                .NotEmpty().WithMessage(ValidationMessages.RegulatorRequired)
                .Must(IsValidRegulator).WithMessage(ValidationMessages.RegulatorInvalid);
        }

        private bool IsValidRegulator(string regulator)
        {
            var validRegulators = new List<string>
            {
                RegulatorConstants.GBENG,
                RegulatorConstants.GBSCT,
                RegulatorConstants.GBWLS,
                RegulatorConstants.GBNIR
            };

            return validRegulators.Contains(regulator);
        }
    }
}
