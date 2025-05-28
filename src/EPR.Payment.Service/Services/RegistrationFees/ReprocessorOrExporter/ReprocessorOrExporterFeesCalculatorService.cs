using EPR.Payment.Service.Common.Data.Interfaces.Repositories.Fees;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.ReprocessorOrExporter;
using EPR.Payment.Service.Common.Dtos.Response.RegistrationFees.ReprocessorOrExporter;
using EPR.Payment.Service.Common.ValueObjects.RegistrationFees;
using EPR.Payment.Service.Services.Interfaces.Payments;
using EPR.Payment.Service.Services.Interfaces.RegistrationFees.ReprocessorOrExporter;

namespace EPR.Payment.Service.Services.RegistrationFees.ReprocessorOrExporter
{
    public class ReprocessorOrExporterFeesCalculatorService(
        IReprocessorOrExporterFeeRepository feeRepository,
        IPreviousPaymentsHelper previousPaymentsHelper) : IReprocessorOrExporterFeesCalculatorService
    {
        public async Task<ReprocessorOrExporterRegistrationFeesResponseDto?> CalculateFeesAsync(ReprocessorOrExporterRegistrationFeesRequestDto request, CancellationToken cancellationToken)
        {
            ReprocessorOrExporterRegistrationFeesResponseDto? response = default;

            var regulator = RegulatorType.Create(request.Regulator);
            
            var registrationFeeEntity = await feeRepository.GetFeeAsync((int)request.RequestorType!, (int)request.MaterialType!, regulator, request.SubmissionDate, cancellationToken);
            if (registrationFeeEntity is null)
            {
                return response;
            }

            response = new()
            {
                MaterialType = request.MaterialType,
                RegistrationFee = registrationFeeEntity.Amount
            };

            if (!string.IsNullOrWhiteSpace(request.ApplicationReferenceNumber))
            {
                response.PreviousPaymentDetail = await previousPaymentsHelper.GetPreviousPaymentAsync<PreviousPaymentDetailDto>(request.ApplicationReferenceNumber, cancellationToken);
            }

            return response;
        }
    }
}
