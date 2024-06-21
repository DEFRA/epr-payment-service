using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPR.Payment.Service.Common.Data.DataModels.Lookups
{
    public class InternalError
    {
        [Key]
        public string InternalErrorCode { get; set; } = null!;

        [MaxLength(255)]
        public string? ErrorMessage { get; set; }

        [Required]
        [MaxLength(10)]
        public string GovPayErrorCode { get; set; } = null!;

        [MaxLength(255)]
        public string? GovPayErrorMessage { get; set; }

        #region Navigation properties
        public virtual ICollection<Payment> Payments { get; set; } = null!;
        #endregion
    }
}
