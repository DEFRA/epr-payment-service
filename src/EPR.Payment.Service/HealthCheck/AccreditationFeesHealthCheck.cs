using EPR.Payment.Service.Services.Interfaces;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace EPR.Payment.Service.HealthCheck
{
    public class AccreditationFeesHealthCheck : IHealthCheck
    {
        public const string HealthCheckResultDescription = "Accreditation Fees Health Check";

        private readonly IAccreditationFeesService _feesService;

        public AccreditationFeesHealthCheck(IAccreditationFeesService feesService)
        {
            _feesService = feesService ?? throw new ArgumentNullException(nameof(feesService));
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var feesCount = await _feesService.GetFeesCount();
            return feesCount == 0
                ? HealthCheckResult.Unhealthy(HealthCheckResultDescription)
                : HealthCheckResult.Healthy(HealthCheckResultDescription);
        }
    }
}
