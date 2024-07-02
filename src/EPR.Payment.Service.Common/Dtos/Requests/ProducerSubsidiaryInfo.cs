namespace EPR.Payment.Service.Common.Dtos.Requests
{
    public class ProducerSubsidiaryInfo
    {
        public string ProducerType { get; set; } = null!;
        public string Country { get; set; } = null!;
        public int NumberOfSubsidiaries { get; set; }
        public bool PayBaseFee { get; set; } 
    }
}
