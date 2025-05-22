using EPR.Payment.Service.Common.Constants.AccreditationFees.Exceptions;
using EPR.Payment.Service.Common.Constants.RegistrationFees.Exceptions;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.Fees;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.Payments;
using EPR.Payment.Service.Common.Data.Repositories.Payments;
using EPR.Payment.Service.Common.Dtos.Request.AccreditationFees;
using EPR.Payment.Service.Common.Dtos.Response.AccreditationFees;
using EPR.Payment.Service.Common.Enums;
using EPR.Payment.Service.Common.ValueObjects.RegistrationFees;
using EPR.Payment.Service.Helper;
using EPR.Payment.Service.Services.Interfaces.AccreditationFees;
using Microsoft.AspNetCore.Mvc;

namespace EPR.Payment.Service.Services.AccreditationFees
{
    public class AccreditationFeesCalculatorService : IAccreditationFeesCalculatorService
    {   
        private readonly IAccreditationFeesRepository _accreditationFeesRepository;
        private readonly IPaymentsRepository _paymentsRepository;

        public AccreditationFeesCalculatorService(IAccreditationFeesRepository accreditationFeesRepository,
            IPaymentsRepository paymentsRepository)
        {
            _accreditationFeesRepository = accreditationFeesRepository ?? throw new ArgumentNullException(nameof(accreditationFeesRepository));
            _paymentsRepository = paymentsRepository ?? throw new ArgumentNullException(nameof(paymentsRepository));
        }

        public async Task<AccreditationFeesResponseDto> CalculateFeesAsync(AccreditationFeesRequestDto request, CancellationToken cancellationToken)
        {
            AccreditationFeesResponseDto? response = null;
            var regulatorType = RegulatorType.Create(request.Regulator);
            var tonnageValues = TonnageHelper.GetTonnageBoundaryByTonnageBand(request.TonnageBand);            

            var accreditationFeesEntity = await _accreditationFeesRepository.GetFeeAsync(
                (int)request.RequestorType,
                (int)request.MaterialType,
                tonnageValues.Item1,
                tonnageValues.Item2,
                regulatorType,
                request.SubmissionDate,
                cancellationToken
             );

            if(accreditationFeesEntity != null)
            {
                var totalOverseasSiteFees = request.NumberOfOverseasSites * accreditationFeesEntity?.FeesPerSite;

                var payment = await _paymentsRepository.GetPreviousPaymentIncludeChildrenByReferenceAsync(request.ApplicationReferenceNumber, cancellationToken);                
                response = new AccreditationFeesResponseDto
                {
                    OverseasSiteChargePerSite = accreditationFeesEntity?.FeesPerSite,
                    TotalOverseasSitesCharges = totalOverseasSiteFees,
                    TonnageBandCharge = accreditationFeesEntity?.Amount
                };

                if (payment != null)
                {
                    var previousPayment = new AccreditationFeesPreviousPayment();
                    previousPayment.PaymentAmount = payment.Amount;
                    
                    if(payment.OfflinePayment != null)
                    {
                        previousPayment.PaymentMethod = Enum.GetName(PaymentType.Offline);
                        previousPayment.PaymentDate = payment.OfflinePayment?.PaymentDate;                        
                    }
                    else if(payment.OnlinePayment != null)
                    {
                        previousPayment.PaymentMethod = Enum.GetName(PaymentType.Online);
                        previousPayment.PaymentDate = payment.UpdatedDate;                        
                    }
                }

                return response;
            }          

            return response;
        }
    }
}