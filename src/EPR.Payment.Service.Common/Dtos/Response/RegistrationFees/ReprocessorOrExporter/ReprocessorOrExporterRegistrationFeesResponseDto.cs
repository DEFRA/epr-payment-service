using System.Text.Json.Serialization;
using EPR.Payment.Service.Common.Dtos.Response.Payments;
using EPR.Payment.Service.Common.Enums;

namespace EPR.Payment.Service.Common.Dtos.Response.RegistrationFees.ReprocessorOrExporter
{
    public class ReprocessorOrExporterRegistrationFeesResponseDto
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public MaterialTypes? MaterialType { get; set; }

        public required decimal RegistrationFee { get; set; }

        public PreviousPaymentDetailResponseDto? PreviousPaymentDetail { get; set; }
    }
}
