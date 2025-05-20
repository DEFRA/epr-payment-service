namespace EPR.Payment.Service.Common.Dtos.Request.Payments
{
    public class OfflinePaymentInsertRequestV2Dto : OfflinePaymentInsertRequestDto
    {
        public required string PaymentMethod { get; set; }
    }
}
