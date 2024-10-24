using EPR.Payment.Service.Common.Dtos.Request.Payments;
using FluentValidation;

namespace EPR.Payment.Service.Validations.Payments
{
    public class OfflinePaymentStatusInsertRequestDtoValidator : AbstractValidator<OfflinePaymentStatusInsertRequestDto>
    {
        private const string InvalidUserIdErrorMessage = "User ID cannot be null or empty.";
        private const string InvalidReferenceErrorMessage = "Reference cannot be null or empty.";
        private const string InvalidAmountErrorMessage = "Amount For Payment cannot be null or empty.";
        public OfflinePaymentStatusInsertRequestDtoValidator()
        {
            RuleFor(x => x.UserId)
                .NotNull()
                .WithMessage(InvalidUserIdErrorMessage);
            RuleFor(x => x.Reference)
                .NotEmpty()
                .WithMessage(InvalidReferenceErrorMessage);
            RuleFor(x => x.Amount)
                .NotNull()
                .WithMessage(InvalidAmountErrorMessage);
        }
    }
}
