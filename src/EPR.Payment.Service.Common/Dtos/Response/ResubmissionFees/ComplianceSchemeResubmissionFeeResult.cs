namespace EPR.Payment.Service.Common.Dtos.Response.ResubmissionFees
{
    public class ComplianceSchemeResubmissionFeeResult
    {
        public decimal TotalResubmissionFee { get; set; }
        public decimal PreviousPayments { get; set; }
        public decimal OutstandingPayment { get; set; }
        public int MemberCount { get; set; }
    }
}
