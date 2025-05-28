using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.ReprocessorOrExporter;

namespace EPR.Payment.Service.Strategies.Interfaces.RegistrationFees.ReprocessorOrExporter
{
    public interface IBaseFeeCalculationStrategy
    {
        Task<decimal> CalculateFeeAsync(ReprocessorOrExporterRegistrationFeesRequestDto request, CancellationToken cancellationToken);
    }
}
