namespace EPR.Payment.Service.Common.Dtos.Responses
{
    public class GetAccreditationFeesResponse
    {
        public int Id { get; set; }
        public bool Large { get; set; }

        public string Regulator { get; set; } = null!;

        public decimal Amount { get; set; }

        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
    }
}