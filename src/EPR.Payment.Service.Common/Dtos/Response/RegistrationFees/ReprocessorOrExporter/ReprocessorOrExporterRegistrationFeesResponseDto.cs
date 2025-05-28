using EPR.Payment.Service.Common.Enums;

namespace EPR.Payment.Service.Common.Dtos.Response.RegistrationFees.ReprocessorOrExporter
{
    public class ReprocessorOrExporterRegistrationFeesResponseDto
    {
        public MaterialTypes? MaterialType { get; set; } = default!;

        public decimal RegistrationFee { get; set; }

        public PreviousPaymentDetailDto? PreviousPaymentDetail { get; set; }
    }
}
