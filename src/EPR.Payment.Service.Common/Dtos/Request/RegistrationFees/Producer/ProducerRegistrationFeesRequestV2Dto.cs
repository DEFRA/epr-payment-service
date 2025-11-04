namespace EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.Producer
{
    public class ProducerRegistrationFeesRequestV2Dto : ProducerRegistrationFeesRequestDto
    {
        public Guid? FileId { get; set; }

        public int? PayerId { get; set; }

        public Guid? ExternalId { get; set; }

        public required DateTimeOffset InvoicePeriod { get; set; }

        public required int PayerTypeId { get; set; }
    }
}