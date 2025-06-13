using EPR.Payment.Service.Common.Constants.Payments;
using EPR.Payment.Service.Common.Constants.RegistrationFees;
using EPR.Payment.Service.Common.Dtos.Request.Payments;
using FluentValidation;

namespace EPR.Payment.Service.Validations.Payments
{
    public abstract class OfflinePaymentInsertRequestDtoCommonValidator<T> : AbstractValidator<T> where T : OfflinePaymentInsertRequestDto
    {
        protected OfflinePaymentInsertRequestDtoCommonValidator(bool isAccerdiationFee = false)
        {
            RuleFor(x => x.UserId)
               .NotNull()
               .WithMessage(ValidationMessages.UserIdRequired);

            RuleFor(x => x.Reference)
                .NotEmpty()
                .WithMessage(ValidationMessages.OfflineReferenceRequired);

            if (isAccerdiationFee)
            {
                RuleFor(x => x.Description)
                    .Cascade(CascadeMode.Stop)
                    .NotEmpty()
                    .WithMessage(ValidationMessages.DescriptionRequired)
                    .Must(text => text == ReasonForPaymentConstants.RegistrationFee || text == ReasonForPaymentConstants.PackagingResubmissionFee || text == ReasonForPaymentConstants.AccreditationFee)
                    .WithMessage(ValidationMessages.InvalidDescriptionV2);
            }
            else
            {
                RuleFor(x => x.Description)
                    .Cascade(CascadeMode.Stop)
                    .NotEmpty()
                    .WithMessage(ValidationMessages.DescriptionRequired)
                    .Must(text => text == ReasonForPaymentConstants.RegistrationFee || text == ReasonForPaymentConstants.PackagingResubmissionFee)
                    .WithMessage(ValidationMessages.InvalidDescription);
            }

            RuleFor(x => x.Regulator)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage(ValidationMessages.OfflineRegulatorRequired)
                .Must(text => text == RegulatorConstants.GBENG || text == RegulatorConstants.GBSCT || text == RegulatorConstants.GBWLS || text == RegulatorConstants.GBNIR)
                .WithMessage(ValidationMessages.InvalidRegulatorOffline);
        }
    }
}
