using EPR.Payment.Service.Common.Enums;

namespace EPR.Payment.Service.Common.Dtos.Response.RegistrationFees.ReprocessorOrExporter
{
    public class ReprocessorOrExporterRegistrationFeesResponseDto
    {
        public MaterialTypes? MaterialType { get; set; }

        public required decimal RegistrationFee { get; set; }

        public PreviousPaymentDetailDto? PreviousPaymentDetail { get; set; }
    }
}
