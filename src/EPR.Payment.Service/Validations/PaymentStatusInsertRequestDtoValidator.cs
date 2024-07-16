﻿using EPR.Payment.Service.Common.Dtos.Request;
using FluentValidation;

namespace EPR.Payment.Service.Validations
{
    public class PaymentStatusInsertRequestDtoValidator : AbstractValidator<PaymentStatusInsertRequestDto>
    {
        private const string InvalidUserIdErrorMessage = "User ID cannot be null or empty.";
        private const string InvalidOrganisationIdErrorMessage = "Organisation ID cannot be null or empty.";
        private const string InvalidReferenceErrorMessage = "Reference cannot be null or empty.";
        private const string InvalidReasonForPaymentErrorMessage = "Reason For Payment cannot be null or empty."; 
        private const string InvalidAmountErrorMessage = "Amount For Payment cannot be null or empty.";
        private const string LessThanAmountErrorMessage = "Amount must be less than or equal to 100000.";
        private const string InvalidStatusErrorMessage = "Status For Payment must be a valid status type.";
        public PaymentStatusInsertRequestDtoValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage(string.Format(InvalidUserIdErrorMessage, nameof(PaymentStatusInsertRequestDto.UserId)));
            RuleFor(x => x.OrganisationId)
                .NotEmpty()
                .WithMessage(string.Format(InvalidOrganisationIdErrorMessage, nameof(PaymentStatusInsertRequestDto.OrganisationId)));
            RuleFor(x => x.Reference)
                .NotEmpty()
                .WithMessage(string.Format(InvalidReferenceErrorMessage, nameof(PaymentStatusInsertRequestDto.Reference)));
            RuleFor(x => x.ReasonForPayment)
                .NotEmpty()
                .WithMessage(string.Format(InvalidReasonForPaymentErrorMessage, nameof(PaymentStatusInsertRequestDto.ReasonForPayment)));
            RuleFor(x => x.Amount)
                .NotEmpty()
                .WithMessage(string.Format(InvalidAmountErrorMessage, nameof(PaymentStatusInsertRequestDto.Amount)));
            RuleFor(x => x.Amount)
                .LessThanOrEqualTo(100000)
                .WithMessage(string.Format(LessThanAmountErrorMessage, nameof(PaymentStatusInsertRequestDto.Amount)));
            RuleFor(x => x.Status)
                .IsInEnum()
                .WithMessage(string.Format(InvalidStatusErrorMessage, nameof(PaymentStatusInsertRequestDto.Status)));
        }
    }
}
