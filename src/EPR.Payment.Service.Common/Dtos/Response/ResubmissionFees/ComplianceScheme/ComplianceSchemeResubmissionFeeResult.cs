namespace EPR.Payment.Service.Common.Dtos.Response.ResubmissionFees.ComplianceScheme
{
    public class ComplianceSchemeResubmissionFeeResult
    {
        public decimal TotalResubmissionFee { get; set; }
        public decimal PreviousPayments { get; set; }
        public decimal OutstandingPayment { get; set; }
        public int MemberCount { get; set; }
    }
}