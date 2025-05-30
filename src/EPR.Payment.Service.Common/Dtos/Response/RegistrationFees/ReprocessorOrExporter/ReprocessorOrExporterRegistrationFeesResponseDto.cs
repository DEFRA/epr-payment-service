using EPR.Payment.Service.Common.Dtos.Response.Payments;

namespace EPR.Payment.Service.Common.Dtos.Response.RegistrationFees.ReprocessorOrExporter
{
    public class ReprocessorOrExporterRegistrationFeesResponseDto
    {
        public required decimal RegistrationFee { get; set; }

        public PreviousPaymentDetailResponseDto? PreviousPaymentDetail { get; set; }
    }
}
