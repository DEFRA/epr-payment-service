using System.Text.Json.Serialization;
using EPR.Payment.Service.Common.Enums;

namespace EPR.Payment.Service.Common.Dtos.Request.Payments
{
    public class OnlinePaymentInsertRequestV2Dto: OnlinePaymentInsertRequestDto
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public OnlinePaymentRequestorTypes? RequestorType { get; set; }
    }
}
