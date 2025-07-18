﻿namespace EPR.Payment.Service.Common.Dtos.Response.Payments
{
    public class OnlinePaymentResponseDto
    {
        public Guid ExternalPaymentId { get; set; }

        public Guid UpdatedByUserId { get; set; }

        public Guid UpdatedByOrganisationId { get; set; }

        public string? GovPayPaymentId { get; set; }
        
        public string? Reference { get; set; }
        
        public decimal Amount { get; set; }
        
        public string? Regulator { get; set; }
        
        public string? Description { get; set; }

        public string RequestorType { get; set; } = string.Empty;
    }
}
