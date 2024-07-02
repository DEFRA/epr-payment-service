namespace EPR.Payment.Service.Common.Dtos.Requests
{
    public class ComplianceSchemeRegistrationRequestDto
    {
        public List<ProducerSubsidiaryInfo> Producers { get; set; } = new List<ProducerSubsidiaryInfo>();
        public bool PayComplianceSchemeBaseFee { get; set; }
    }
}
