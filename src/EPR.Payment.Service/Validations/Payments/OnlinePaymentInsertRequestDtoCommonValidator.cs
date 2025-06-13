using EPR.Payment.Service.Common.Constants.Payments;
using EPR.Payment.Service.Common.Constants.RegistrationFees;
using EPR.Payment.Service.Common.Dtos.Request.Payments;
using FluentValidation;

namespace EPR.Payment.Service.Validations.Payments
{
    public class OnlinePaymentInsertRequestDtoCommonValidator<T> : AbstractValidator<T>
        where T : OnlinePaymentInsertRequestDto
    {
        public OnlinePaymentInsertRequestDtoCommonValidator(bool isAccerdiationFee = false)
        {
            RuleFor(x => x.UserId)
                .NotNull()
                .WithMessage(ValidationMessages.UserIdRequired);

            RuleFor(x => x.OrganisationId)
                .NotNull()
                .WithMessage(ValidationMessages.OrganisationIdRequired);

            RuleFor(x => x.Reference)
                .NotEmpty()
                .WithMessage(ValidationMessages.ReferenceRequired);

            if (isAccerdiationFee)
            {
                RuleFor(x => x.ReasonForPayment)
                    .Cascade(CascadeMode.Stop)
                    .NotEmpty()
                    .WithMessage(ValidationMessages.ReasonForPaymentRequired)
                    .Must(text => text == ReasonForPaymentConstants.RegistrationFee || text == ReasonForPaymentConstants.PackagingResubmissionFee || text == ReasonForPaymentConstants.AccreditationFee)
                    .WithMessage(ValidationMessages.InvalidReasonForPaymentV2);
            }
            else
            {
                RuleFor(x => x.ReasonForPayment)
                    .Cascade(CascadeMode.Stop)
                    .NotEmpty()
                    .WithMessage(ValidationMessages.ReasonForPaymentRequired)
                    .Must(text => text == ReasonForPaymentConstants.RegistrationFee || text == ReasonForPaymentConstants.PackagingResubmissionFee)
                    .WithMessage(ValidationMessages.InvalidReasonForPayment);
            }

            RuleFor(x => x.Amount)
                .NotNull()
                .GreaterThan(0)
                .WithMessage(ValidationMessages.AmountRequiredAndGreaterThanZero);

            RuleFor(x => x.Regulator)
                .NotEmpty()
                .WithMessage(ValidationMessages.RegulatorRequired);

            RuleFor(x => x.Regulator)
                .Must(text => text == RegulatorConstants.GBENG || text == RegulatorConstants.GBSCT || text == RegulatorConstants.GBWLS || text == RegulatorConstants.GBNIR)
                .WithMessage(ValidationMessages.RegulatorInvalid);

            RuleFor(x => x.Regulator)
                .Must(text => text == RegulatorConstants.GBENG)
                .WithMessage(ValidationMessages.RegulatorNotENG)
                .When(x => string.Equals(x.Regulator, RegulatorConstants.GBSCT, StringComparison.Ordinal)
                       || string.Equals(x.Regulator, RegulatorConstants.GBWLS, StringComparison.Ordinal)
                       || string.Equals(x.Regulator, RegulatorConstants.GBNIR, StringComparison.Ordinal));

            RuleFor(x => x.Status)
                .IsInEnum()
                .WithMessage(ValidationMessages.InvalidStatus);
        }
    }
}
