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

namespace EPR.Payment.Service.Services.AccreditationFees
{
    public class AccreditationFeesCalculatorService(
        IAccreditationFeesRepository accreditationFeesRepository,
        IPaymentsRepository paymentsRepository) : IAccreditationFeesCalculatorService
    {   
        public async Task<AccreditationFeesResponseDto?> CalculateFeesAsync(
            AccreditationFeesRequestDto request,
            CancellationToken cancellationToken)
        {
            AccreditationFeesResponseDto? response = null;
            RegulatorType regulatorType = RegulatorType.Create(request.Regulator);

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
                    Common.Data.DataModels.Payment? payment = await paymentsRepository.GetPreviousPaymentIncludeChildrenByReferenceAsync(request.ApplicationReferenceNumber, cancellationToken);

                    if (payment is not null)
                    {
                        AccreditationFeesPreviousPayment previousPayment = new()
                        {
                            PaymentAmount = payment.Amount
                        };

                        if (payment.OfflinePayment is not null)
                        {
                            previousPayment.PaymentMode = PaymentTypes.Offline.GetDescription();
                            previousPayment.PaymentDate = payment.OfflinePayment.PaymentDate.GetValueOrDefault();
                            previousPayment.PaymentMethod = payment.OfflinePayment.PaymentMethod;
                        }
                        else if (payment.OnlinePayment is not null)
                        {
                            previousPayment.PaymentMode = PaymentTypes.Online.GetDescription();
                            previousPayment.PaymentDate = payment.UpdatedDate;
                        }

                        response.PreviousPaymentDetail = previousPayment;
                    }
                }

                return response;
            }          

            return response;
        }
    }
}