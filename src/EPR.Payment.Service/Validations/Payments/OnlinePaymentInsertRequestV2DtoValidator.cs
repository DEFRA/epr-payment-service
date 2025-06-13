using EPR.Payment.Service.Common.Constants.RegistrationFees;
using EPR.Payment.Service.Common.Dtos.Request.Payments;
using FluentValidation;

namespace EPR.Payment.Service.Validations.Payments
{
    public class OnlinePaymentInsertRequestV2DtoValidator : OnlinePaymentInsertRequestDtoCommonValidator<OnlinePaymentInsertRequestV2Dto>
    {
        public OnlinePaymentInsertRequestV2DtoValidator() : base(true)
        {
            RuleFor(x => x.RequestorType)
               .NotEmpty()
               .WithMessage(ValidationMessages.OnlineRequestorTypeRequired);
        }
    }
}
