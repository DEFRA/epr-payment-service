using EPR.Payment.Service.Common.Dtos.Request.Payments;
using FluentValidation;

namespace EPR.Payment.Service.Validations
{
    public class OnlinePaymentStatusUpdateRequestDtoValidator : AbstractValidator<OnlinePaymentStatusUpdateRequestDto>
    {
        private const string InvalidGovPayPaymentIdErrorMessage = "Gov Pay Payment ID cannot be null or empty.";
        private const string InvalidUserIdErrorMessage = "Updated By User ID cannot be null or empty.";
        private const string InvalidOrganisationIdErrorMessage = "Updated By Organisation ID cannot be null or empty.";
        private const string InvalidReferenceErrorMessage = "Reference cannot be null or empty.";
        private const string InvalidStatusErrorMessage = "Status cannot be null or empty.";
        public OnlinePaymentStatusUpdateRequestDtoValidator()
        {
            RuleFor(x => x.GovPayPaymentId)
                .NotEmpty()
                .WithMessage(string.Format(InvalidGovPayPaymentIdErrorMessage, nameof(OnlinePaymentStatusUpdateRequestDto.GovPayPaymentId)));
            RuleFor(x => x.UpdatedByUserId)
                .NotNull()
                .WithMessage(string.Format(InvalidUserIdErrorMessage, nameof(OnlinePaymentStatusUpdateRequestDto.UpdatedByUserId)));
            RuleFor(x => x.UpdatedByOrganisationId)
                .NotNull()
                .WithMessage(string.Format(InvalidOrganisationIdErrorMessage, nameof(OnlinePaymentStatusUpdateRequestDto.UpdatedByOrganisationId)));
            RuleFor(x => x.Reference)
                .NotEmpty()
                .WithMessage(string.Format(InvalidReferenceErrorMessage, nameof(OnlinePaymentStatusUpdateRequestDto.Reference)));
            RuleFor(x => x.Status)
                .IsInEnum()
                .WithMessage(string.Format(InvalidStatusErrorMessage, nameof(OnlinePaymentStatusUpdateRequestDto.Status)));
        }
    }
}
