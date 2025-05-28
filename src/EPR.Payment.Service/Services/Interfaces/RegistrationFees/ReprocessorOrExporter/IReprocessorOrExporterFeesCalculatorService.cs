using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.ReprocessorOrExporter;
using EPR.Payment.Service.Common.Dtos.Response.RegistrationFees.ReprocessorOrExporter;

namespace EPR.Payment.Service.Services.Interfaces.RegistrationFees.ReprocessorOrExporter
{
    public interface IReprocessorOrExporterFeesCalculatorService
    {
        Task<Common.Dtos.Response.RegistrationFees.ReprocessorOrExporter.RegistrationFees?> CalculateFeesAsync(ReprocessorOrExporterRegistrationFeesRequestDto request, CancellationToken cancellationToken);
    }
}