using System.Text.Json.Serialization;
using EPR.Payment.Service.Common.Enums;

namespace EPR.Payment.Service.Common.Dtos.Request.Payments
{
    public class OfflinePaymentInsertRequestV2Dto : OfflinePaymentInsertRequestDto
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public OfflinePaymentMethodTypes? PaymentMethod { get; set; }
        public Guid? OrganisationId { get; set; }
    }
}
