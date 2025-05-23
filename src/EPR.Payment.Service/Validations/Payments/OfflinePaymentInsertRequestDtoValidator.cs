using EPR.Payment.Service.Common.Dtos.Request.Payments;

namespace EPR.Payment.Service.Validations.Payments
{
    public class OfflinePaymentInsertRequestDtoValidator : OfflinePaymentInsertRequestDtoCommonValidator<OfflinePaymentInsertRequestDto>
    {
        public OfflinePaymentInsertRequestDtoValidator() : base(false)
        {
        }
    }
}
