namespace EPR.Payment.Service.Common.Dtos.Requests
{
    public class ProducerRegistrationRequestDto
    {
        public string ProducerType { get; set; } = null!;
        public int NumberOfSubsidiaries { get; set; }
        public string Country { get; set; } = null!;
        public bool PayBaseFee { get; set; }
    }
}
