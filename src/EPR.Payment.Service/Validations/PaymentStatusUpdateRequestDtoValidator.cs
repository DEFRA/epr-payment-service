using EPR.Payment.Service.Common.Dtos.Request;
using FluentValidation;

namespace EPR.Payment.Service.Validations
{
    public class PaymentStatusUpdateRequestDtoValidator : AbstractValidator<PaymentStatusUpdateRequestDto>
    {
        private const string InvalidGovPayPaymentIdErrorMessage = "Gov Pay Payment ID cannot be null or empty.";
        private const string InvalidUserIdErrorMessage = "Updated By User ID cannot be null or empty.";
        private const string InvalidOrganisationIdErrorMessage = "Updated By Organisation ID cannot be null or empty.";
        private const string InvalidReferenceErrorMessage = "Reference cannot be null or empty.";
        private const string InvalidStatusErrorMessage = "Status cannot be null or empty.";
        private const string InvalidErrorCodeErrorMessage = "Error Code must be one of the following: 'A', 'B', 'C' or it can be null or empty.";
        public PaymentStatusUpdateRequestDtoValidator()
        {
            RuleFor(x => x.GovPayPaymentId)
                .NotEmpty()
                .WithMessage(string.Format(InvalidGovPayPaymentIdErrorMessage, nameof(PaymentStatusUpdateRequestDto.GovPayPaymentId)));
            RuleFor(x => x.UpdatedByUserId)
                .NotNull()
                .WithMessage(string.Format(InvalidUserIdErrorMessage, nameof(PaymentStatusUpdateRequestDto.UpdatedByUserId)));
            RuleFor(x => x.UpdatedByOrganisationId)
                .NotNull()
                .WithMessage(string.Format(InvalidOrganisationIdErrorMessage, nameof(PaymentStatusUpdateRequestDto.UpdatedByOrganisationId)));
            RuleFor(x => x.Reference)
                .NotEmpty()
                .WithMessage(string.Format(InvalidReferenceErrorMessage, nameof(PaymentStatusUpdateRequestDto.Reference)));
            RuleFor(x => x.Status)
                .IsInEnum()
                .WithMessage(string.Format(InvalidStatusErrorMessage, nameof(PaymentStatusUpdateRequestDto.Status)));
            RuleFor(x => x.ErrorCode)
                .Must(value => string.IsNullOrEmpty(value) || "ABC".Contains(value))
                .WithMessage(string.Format(InvalidErrorCodeErrorMessage, nameof(PaymentStatusUpdateRequestDto.ErrorCode)));
        }
    }
}
