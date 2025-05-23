using EPR.Payment.Service.Common.Constants.RegistrationFees;
using EPR.Payment.Service.Common.Dtos.Request.Payments;
using FluentValidation;

namespace EPR.Payment.Service.Validations.Payments
{
    public class OfflinePaymentInsertRequestV2DtoValidator : OfflinePaymentInsertRequestBaseDtoValidator<OfflinePaymentInsertRequestV2Dto>
    {
        public OfflinePaymentInsertRequestV2DtoValidator() : base (true)
        {
            RuleFor(x => x.PaymentMethod)
               .NotNull()
               .WithMessage(ValidationMessages.OfflinePaymentMethodRequired);
        }
    }
}
