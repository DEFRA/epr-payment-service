using EPR.Payment.Service.Common.Data.DataModels.Lookups;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.Fees;
using EPR.Payment.Service.Common.Dtos.Request.AccreditationFees;
using EPR.Payment.Service.Common.Dtos.Response.AccreditationFees;
using EPR.Payment.Service.Common.ValueObjects.RegistrationFees;
using EPR.Payment.Service.Services.Interfaces.AccreditationFees;
using EPR.Payment.Service.Services.Interfaces.Payments;

namespace EPR.Payment.Service.Services.AccreditationFees
{
    public class AccreditationFeesCalculatorService(
        IAccreditationFeesRepository accreditationFeesRepository,
        IPreviousPaymentsHelper previousPaymentsHelper) : IAccreditationFeesCalculatorService
    {   
        public async Task<ReprocessorOrExporterAccreditationFeesResponseDto?> CalculateFeesAsync(
            ReprocessorOrExporterAccreditationFeesRequestDto request,
            CancellationToken cancellationToken)
        {
            ReprocessorOrExporterAccreditationFeesResponseDto? response = null;
            RegulatorType regulatorType = RegulatorType.Create(request.Regulator!);
           
            AccreditationFee? accreditationFeesEntity = await accreditationFeesRepository.GetFeeAsync(
                (int)request.RequestorType!.Value,
                (int)request.MaterialType!.Value,
                (int)request.TonnageBand!.Value,
                regulatorType,
                request.SubmissionDate,
                cancellationToken
             );

            if (accreditationFeesEntity is not null)
            {
                decimal totalOverseasSiteFees = request.NumberOfOverseasSites * accreditationFeesEntity.FeesPerSite;

                response = new ReprocessorOrExporterAccreditationFeesResponseDto
                {
                    OverseasSiteChargePerSite = accreditationFeesEntity.FeesPerSite,
                    TotalOverseasSitesCharges = totalOverseasSiteFees,
                    TonnageBandCharge = accreditationFeesEntity.Amount
                };

                if (!string.IsNullOrWhiteSpace(request.ApplicationReferenceNumber))
                {
                    response.PreviousPaymentDetail = await previousPaymentsHelper.GetPreviousPaymentAsync(
                        request.ApplicationReferenceNumber,
                        cancellationToken);
                }
            }          

            return response;
        }
    }
}