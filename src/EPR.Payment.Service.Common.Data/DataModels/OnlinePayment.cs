using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPR.Payment.Service.Common.Data.DataModels
{
    public class OnlinePayment 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(Order = 1)]
        public int Id { get; set; }

        [ForeignKey("Payment")]
        [Column(Order = 2)]
        public int PaymentId { get; set; }

        [Column(Order = 3)]
        public Guid OrganisationId { get; set; }

        [Column(TypeName = "varchar(50)", Order = 4)]
        public string? GovPayPaymentId{ get; set; }

        [Column(TypeName = "varchar(20)", Order = 5)]
        public string? GovPayStatus { get; set; }

        [Column(TypeName = "varchar(255)", Order = 6)]
        public string? ErrorCode { get; set; }

        [Column(TypeName = "varchar(255)", Order = 7)]
        public string? ErrorMessage { get; set; }

        [Column(Order = 8)]
        public Guid UpdatedByOrgId { get; set; }

        #region Navigation properties

        public virtual Payment Payment { get; set; } = null!;

        #endregion
    }
}