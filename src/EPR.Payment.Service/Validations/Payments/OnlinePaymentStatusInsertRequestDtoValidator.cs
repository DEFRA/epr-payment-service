using EPR.Payment.Service.Common.Dtos.Request.Payments;
using FluentValidation;

namespace EPR.Payment.Service.Validations.Payments
{
    public class OnlinePaymentStatusInsertRequestDtoValidator : AbstractValidator<OnlinePaymentStatusInsertRequestDto>
    {
        private const string InvalidUserIdErrorMessage = "User ID cannot be null or empty.";
        private const string InvalidOrganisationIdErrorMessage = "Organisation ID cannot be null or empty.";
        private const string InvalidReferenceErrorMessage = "Reference cannot be null or empty.";
        private const string InvalidReasonForPaymentErrorMessage = "Reason For Payment cannot be null or empty.";
        private const string InvalidAmountErrorMessage = "Amount For Payment cannot be null or empty.";
        private const string InvalidStatusErrorMessage = "Status For Payment must be a valid status type.";
        public OnlinePaymentStatusInsertRequestDtoValidator()
        {
            RuleFor(x => x.UserId)
                .NotNull()
                .WithMessage(InvalidUserIdErrorMessage);
            RuleFor(x => x.OrganisationId)
                .NotNull()
                .WithMessage(InvalidOrganisationIdErrorMessage);
            RuleFor(x => x.Reference)
                .NotEmpty()
                .WithMessage(InvalidReferenceErrorMessage);
            RuleFor(x => x.ReasonForPayment)
                .NotEmpty()
                .WithMessage(InvalidReasonForPaymentErrorMessage);
            RuleFor(x => x.Amount)
                .NotNull()
                .WithMessage(InvalidAmountErrorMessage);
            RuleFor(x => x.Status)
                .IsInEnum()
                .WithMessage(InvalidStatusErrorMessage);
        }
    }
}
