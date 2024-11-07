﻿using EPR.Payment.Service.Common.Constants.RegistrationFees;
using EPR.Payment.Service.Common.Dtos.Request.ResubmissionFees.Producer;
using EPR.Payment.Service.Validations.Common;
using FluentValidation;

namespace EPR.Payment.Service.Validations.ResubmissionFees.Producer
{
    public class ProducerResubmissionFeeRequestDtoValidator : AbstractValidator<ProducerResubmissionFeeRequestDto>
    {
        public ProducerResubmissionFeeRequestDtoValidator()
        {
            RuleFor(x => x.Regulator)
                .NotEmpty().WithMessage(ValidationMessages.RegulatorRequired)
                .Must(RegulatorValidationHelper.IsValidRegulator).WithMessage(ValidationMessages.RegulatorInvalid);

            RuleFor(x => x.ResubmissionDate)
                 .Cascade(CascadeMode.Stop)
                 .NotEmpty().WithMessage(ValidationMessages.ResubmissionDateRequired)
                 .Must(BeInUtc).WithMessage(ValidationMessages.ResubmissionDateMustBeUtc)
                 .LessThanOrEqualTo(DateTime.UtcNow).WithMessage(ValidationMessages.FutureResubmissionDate);

            RuleFor(x => x.ReferenceNumber)
                .NotEmpty().WithMessage(ValidationMessages.ReferenceNumberRequired);
        }
        private static bool BeInUtc(DateTime dateTime)
        {
            return dateTime.Kind == DateTimeKind.Utc;
        }
    }
}