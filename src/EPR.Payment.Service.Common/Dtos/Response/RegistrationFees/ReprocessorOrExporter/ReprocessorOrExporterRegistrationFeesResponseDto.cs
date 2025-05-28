using EPR.Payment.Service.Common.Enums;

namespace EPR.Payment.Service.Common.Dtos.Response.RegistrationFees.ReprocessorOrExporter
{
    public class ReprocessorOrExporterRegistrationFeesResponseDto
    {

        public MaterialType MaterialType { get; set; } = default!;

        public decimal RegistrationFee { get; set; }

        public PreviousPaymentDetailDto? PreviousPaymentDetail { get; set; }
    }
}
