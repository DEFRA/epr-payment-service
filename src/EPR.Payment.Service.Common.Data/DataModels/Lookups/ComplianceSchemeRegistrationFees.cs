﻿using EPR.Payment.Service.Common.Data.DataModels.BaseClasses;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EPR.Payment.Service.Common.Data.DataModels.Lookups
{
    [Table("ComplianceSchemeRegistrationFees", Schema = "Lookup")]
    [ExcludeFromCodeCoverage]
    public class ComplianceSchemeRegistrationFees : BaseEntity
    {
        [MaxLength(255)]
        public string FeesType { get; set; } = null!;

        [MaxLength(255)]
        public string Description { get; set; } = null!;

        [MaxLength(255)]
        public string Regulator { get; set; } = null!;

        [Column(TypeName = "decimal(19,4)")]
        public decimal Amount { get; set; }

        public DateTime EffectiveFrom { get; set; }
        public DateTime EffectiveTo { get; set; }
    }
}