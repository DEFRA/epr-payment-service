using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPR.Payment.Service.Common.Data.DataModels
{
    public class OfflinePayment : Payment
    {
        [Column(Order = 1)]
        public DateTime PaymentDate { get; set; }

        [MaxLength(255)]
        [Column(Order = 2)]
        public string Comments { get; set; } = null!;
    }
}