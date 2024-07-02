namespace EPR.Payment.Service.Common.Dtos.Responses
{
    public class RegistrationFeeResponseDto
    {
        public decimal? BaseFee { get; set; }
        public decimal? SubsidiariesFee { get; set; }
        public decimal? ProducersFee { get; set; }
        public decimal TotalFee { get; set; }
    }
}