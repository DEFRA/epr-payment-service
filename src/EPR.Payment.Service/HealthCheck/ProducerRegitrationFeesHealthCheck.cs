using EPR.Payment.Service.Services.Interfaces;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace EPR.Payment.Service.HealthCheck
{
    public class ProducerRegitrationFeesHealthCheck : IHealthCheck
    {
        public const string HealthCheckResultDescription = " Fees Health Check";

        private readonly IProducerFeesService _feesService;

        public ProducerRegitrationFeesHealthCheck(IProducerFeesService feesService)
        {
            _feesService = feesService ?? throw new ArgumentNullException(nameof(feesService));
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var feesCount = await _feesService.GetProducerRegitrationFeesCount();
            return feesCount == 0
                ? HealthCheckResult.Unhealthy(HealthCheckResultDescription)
                : HealthCheckResult.Healthy(HealthCheckResultDescription);
        }
    }
}
