﻿using EPR.Payment.Service.Services.Interfaces.Payments;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace EPR.Payment.Service.HealthCheck
{
    public class PaymentStatusHealthCheck : IHealthCheck
    {
        public const string HealthCheckResultDescription = "Payment Status Health Check";

        private readonly IOnlinePaymentsService _paymentsService;

        public PaymentStatusHealthCheck(IOnlinePaymentsService paymentsService)
        {
            _paymentsService = paymentsService;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var paymentStatusCount = await _paymentsService.GetOnlinePaymentStatusCountAsync(cancellationToken);
            return paymentStatusCount == 0
                ? HealthCheckResult.Unhealthy(HealthCheckResultDescription)
                : HealthCheckResult.Healthy(HealthCheckResultDescription);
        }
    }
}