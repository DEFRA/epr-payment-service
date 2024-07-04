﻿using System.ComponentModel.DataAnnotations;

namespace EPR.Payment.Service.Common.Dtos.Request
{
    public class PaymentStatusUpdateRequestDto
    {
        [Required(ErrorMessage = "GovPay Payment ID is required")]
        public string? GovPayPaymentId { get; set; }

        [Required(ErrorMessage = "Updated By User ID is required")]
        public Guid? UpdatedByUserId { get; set; }

        [Required(ErrorMessage = "Updated By Organisation ID is required")]
        public Guid? UpdatedByOrganisationId { get; set; }

        [Required(ErrorMessage = "Reference is required")]
        public string? Reference { get; set; }

        [Required(ErrorMessage = "Status is required")]
        public Enums.Status Status { get; set; }

        public string? ErrorCode { get; set; }
    }
}
