namespace EPR.Payment.Service.Common.Dtos.Response.RegistrationFees.ComplianceScheme
{
    public class ComplianceSchemeFeesResponseDto
    {
        public decimal TotalFee { get; set; }
        public decimal ComplianceSchemeRegistrationFee { get; set; }
        public decimal PreviousPayment { get; set; }
        public decimal OutstandingPayment { get; set; }
        public List<ComplianceSchemeMembersWithFeesDto> ComplianceSchemeMembersWithFees { get; set; } = new();
    }

    public class ComplianceSchemeMembersWithFeesDto
    {
        public required string MemberId { get; set; }
        public decimal MemberRegistrationFee { get; set; }
        public decimal MemberOnlineMarketPlaceFee { get; set; }
        public decimal MemberLateRegistrationFee { get; set; }        
        public decimal SubsidiariesFee { get; set; }
        public decimal TotalMemberFee { get; set; }
        public decimal SubsidiariesLateRegistrationFee { get; set; }
        public required SubsidiariesFeeBreakdown SubsidiariesFeeBreakdown { get; set; }
    }
}
