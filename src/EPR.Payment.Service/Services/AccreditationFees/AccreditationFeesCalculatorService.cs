using EPR.Payment.Service.Common.Data.DataModels.Lookups;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.Fees;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.Payments;
using EPR.Payment.Service.Common.Dtos.Request.AccreditationFees;
using EPR.Payment.Service.Common.Dtos.Response.AccreditationFees;
using EPR.Payment.Service.Common.Enums;
using EPR.Payment.Service.Common.Extensions;
using EPR.Payment.Service.Common.ValueObjects.RegistrationFees;
using EPR.Payment.Service.Helper;
using EPR.Payment.Service.Services.Interfaces.AccreditationFees;
using EPR.Payment.Service.Services.Interfaces.Payments;

namespace EPR.Payment.Service.Services.AccreditationFees
{
    public class AccreditationFeesCalculatorService(
        IAccreditationFeesRepository accreditationFeesRepository,
        IPreviousPaymentsHelper previousPaymentsHelper) : IAccreditationFeesCalculatorService
    {   
        public async Task<AccreditationFeesResponseDto?> CalculateFeesAsync(
            AccreditationFeesRequestDto request,
            CancellationToken cancellationToken)
        {
            AccreditationFeesResponseDto? response = null;
            RegulatorType regulatorType = RegulatorType.Create(request.Regulator!);

            (int tonnesOver, int tonnesUpto) = TonnageHelper.GetTonnageBoundaryByTonnageBand(request.TonnageBand);

            int requestorType = request.RequestorType.HasValue ? (int)request.RequestorType : 0;
            int materialType = request.MaterialType.HasValue ? (int)request.MaterialType : 0;

            AccreditationFee? accreditationFeesEntity = await accreditationFeesRepository.GetFeeAsync(
                requestorType,
                materialType,
                tonnesOver,
                tonnesUpto,
                regulatorType,
                request.SubmissionDate,
                cancellationToken
             );

            if (accreditationFeesEntity is not null)
            {
                decimal totalOverseasSiteFees = request.NumberOfOverseasSites * accreditationFeesEntity.FeesPerSite;

                response = new AccreditationFeesResponseDto
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