namespace EPR.Payment.Service.Common.Dtos.Request.ResubmissionFees.Producer
{
    public class ProducerResubmissionFeeRequestDto
    {
        public string Regulator { get; set; } = string.Empty;
        public DateTime ResubmissionDate { get; set; }
        public string ReferenceNumber { get; set; } = string.Empty;
    }
}