using EPR.Payment.Service.Common.Constants.Payments;
using EPR.Payment.Service.Common.Constants.RegistrationFees;
using EPR.Payment.Service.Common.Dtos.Request.Payments;
using FluentValidation;

namespace EPR.Payment.Service.Validations.Payments
{
    public class OfflinePaymentInsertRequestV2DtoValidator : AbstractValidator<OfflinePaymentInsertRequestV2Dto>
    {
        public OfflinePaymentInsertRequestV2DtoValidator()
        {
            RuleFor(x => x.UserId)
               .NotNull()
               .WithMessage(ValidationMessages.UserIdRequired);

            RuleFor(x => x.Reference)
                .NotEmpty()
                .WithMessage(ValidationMessages.OfflineReferenceRequired);

            RuleFor(x => x.Description)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage(ValidationMessages.DescriptionRequired)
                .Must(text => text == OfflinePayDescConstants.RegistrationFee || text == OfflinePayDescConstants.PackagingResubmissionFee)
                .WithMessage(ValidationMessages.InvalidDescription);

            RuleFor(x => x.Regulator)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage(ValidationMessages.OfflineRegulatorRequired)
                .Must(text => text == RegulatorConstants.GBENG || text == RegulatorConstants.GBSCT || text == RegulatorConstants.GBWLS || text == RegulatorConstants.GBNIR)
                .WithMessage(ValidationMessages.InvalidRegulatorOffline);

            RuleFor(x => x.PaymentMethod)
               .NotNull()
               .WithMessage(ValidationMessages.OfflinePaymentMethodRequired);
        }
    }
}
