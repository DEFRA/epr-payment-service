﻿using EPR.Payment.Service.Common.Data.DataModels.Lookups;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EPR.Payment.Service.Common.Data.DataModels
{
    [Table("Payment")]
    [ExcludeFromCodeCoverage]
    public class Payment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }

        public Guid OrganisationId { get; set; }

        [MaxLength(50)]
        public string? GovpayPaymentId { get; set; }

        [ForeignKey("PaymentStatus")]
        public Enums.Status InternalStatusId { get; set; }

        [MaxLength(200)]
        public string Regulator { get; set; } = null!;

        [MaxLength(20)]
        public string? GovPayStatus { get; set; }

        [ForeignKey("InternalError")]
        [MaxLength(10)]
        public string? InternalErrorCode { get; set; }

        [MaxLength(255)]
        public string Reference { get; set; } = null!;

        [Column(TypeName = "decimal(19,4)")]
        public decimal Amount { get; set; }

        [MaxLength(255)]
        public string ReasonForPayment { get; set; } = null!;

        public DateTime CreatedDate { get; set; }

        public Guid UpdatedByUserId { get; set; }

        public Guid UpdatedByOrganisationId { get; set; }

        public DateTime UpdatedDate { get; set; }

        #region Navigation properties

        public virtual PaymentStatus PaymentStatus { get; set; } = null!;

        public virtual InternalError? InternalError { get; set; }
        #endregion Navigation properties
    }
}