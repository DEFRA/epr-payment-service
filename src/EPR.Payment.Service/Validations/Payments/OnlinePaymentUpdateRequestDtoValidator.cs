using EPR.Payment.Service.Common.Constants.RegistrationFees;
using EPR.Payment.Service.Common.Dtos.Request.Payments;
using FluentValidation;

namespace EPR.Payment.Service.Validations
{
    public class OnlinePaymentUpdateRequestDtoValidator : AbstractValidator<OnlinePaymentUpdateRequestDto>
    {
        public OnlinePaymentUpdateRequestDtoValidator()
        {
            RuleFor(x => x.GovPayPaymentId)
                .NotEmpty()
                .WithMessage(ValidationMessages.InvalidGovPayPaymentId);

            RuleFor(x => x.UpdatedByUserId)
                .NotNull()
                .WithMessage(ValidationMessages.InvalidUpdatedByUserId);

            RuleFor(x => x.UpdatedByOrganisationId)
                .NotNull()
                .WithMessage(ValidationMessages.InvalidOrganisationId);

            RuleFor(x => x.Reference)
                .NotEmpty()
                .WithMessage(ValidationMessages.ReferenceRequired);

            RuleFor(x => x.Status)
                .IsInEnum()
                .WithMessage(ValidationMessages.InvalidStatus);
        }
    }
}
