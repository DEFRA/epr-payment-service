using EPR.Payment.Service.Common.Dtos.Response.Payments;

namespace EPR.Payment.Service.Common.Dtos.Response.AccreditationFees
{
    public class AccreditationFeesResponseDto
    {
        public decimal OverseasSiteChargePerSite { get; set; }

        public decimal TotalOverseasSitesCharges { get; set; }

        public decimal TonnageBandCharge { get; set; }

        public decimal TotalAccreditationFees { get => TonnageBandCharge + TotalOverseasSitesCharges; }

        public PreviousPaymentDetailResponseDto? PreviousPaymentDetail { get; set; }
    }
}