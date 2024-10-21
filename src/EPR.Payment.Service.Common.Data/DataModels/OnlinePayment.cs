using System.ComponentModel.DataAnnotations.Schema;

namespace EPR.Payment.Service.Common.Data.DataModels
{
    public class OnlinePayment : Payment
    {
        [Column(Order = 1)]
        public Guid OrganisationId { get; set; }

        [Column(TypeName = "varchar(50)", Order = 2)]
        public string? GovpayPaymentId { get; set; }

        [Column(TypeName = "varchar(20)", Order = 3)]
        public string? GovPayStatus { get; set; }

        [Column(TypeName = "varchar(255)", Order = 4)]
        public string? ErrorCode { get; set; }

        [Column(TypeName = "varchar(255)", Order = 5)]
        public string? ErrorMessage { get; set; }
    }
}