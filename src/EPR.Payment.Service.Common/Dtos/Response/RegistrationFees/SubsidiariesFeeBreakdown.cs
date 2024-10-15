namespace EPR.Payment.Service.Common.Dtos.Response.RegistrationFees
{
    public class SubsidiariesFeeBreakdown
    {
        public decimal TotalSubsidiariesOMPFees { get; set; }
        public int CountOfOMPSubsidiaries { get; set; }
        public decimal UnitOMPFees { get; set; }
        public List<FeeBreakdown> FeeBreakdowns { get; set; } = new();
    }

    public class FeeBreakdown
    {
        public int BandNumber { get; set; }
        public int UnitCount { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
