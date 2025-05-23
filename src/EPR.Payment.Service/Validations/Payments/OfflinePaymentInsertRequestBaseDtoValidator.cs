using EPR.Payment.Service.Common.Constants.Payments;
using EPR.Payment.Service.Common.Constants.RegistrationFees;
using EPR.Payment.Service.Common.Dtos.Request.Payments;
using FluentValidation;

namespace EPR.Payment.Service.Validations.Payments
{
    public abstract class OfflinePaymentInsertRequestBaseDtoValidator<T> : AbstractValidator<T> where T : OfflinePaymentInsertRequestBaseDto 
    {
        protected OfflinePaymentInsertRequestBaseDtoValidator(bool isAccerdiationFee = false)
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
                    .Must(text => text == OfflinePayDescConstants.RegistrationFee || text == OfflinePayDescConstants.PackagingResubmissionFee || text == OfflinePayDescConstants.AccreditationFee)
                    .WithMessage(ValidationMessages.InvalidDescription);
            }
            else
            {
                RuleFor(x => x.Description)
                    .Cascade(CascadeMode.Stop)
                    .NotEmpty()
                    .WithMessage(ValidationMessages.DescriptionRequired)
                    .Must(text => text == OfflinePayDescConstants.RegistrationFee || text == OfflinePayDescConstants.PackagingResubmissionFee)
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
