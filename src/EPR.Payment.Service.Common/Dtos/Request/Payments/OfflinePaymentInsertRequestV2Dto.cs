using System.Text.Json.Serialization;
using EPR.Payment.Service.Common.Dtos.Enums;

namespace EPR.Payment.Service.Common.Dtos.Request.Payments
{
    public class OfflinePaymentInsertRequestV2Dto : OfflinePaymentInsertRequestBaseDto
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public OfflinePaymentMethodTypes? PaymentMethod { get; set; }
    }
}
