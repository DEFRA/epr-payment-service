using EPR.Payment.Service.Common.Data.Interfaces.Repositories.Fees;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.Payments;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.ReprocessorOrExporter;
using EPR.Payment.Service.Common.Enums;
using EPR.Payment.Service.Common.Extensions;
using EPR.Payment.Service.Common.ValueObjects.RegistrationFees;
using EPR.Payment.Service.Services.Interfaces.RegistrationFees.ReprocessorOrExporter;
namespace EPR.Payment.Service.Services.RegistrationFees.ReprocessorOrExporter
{
    public class ReprocessorOrExporterFeesCalculatorService(
        IReprocessorOrExporterFeeRepository feeRepository,
        IPaymentsRepository paymentsRepository) : IReprocessorOrExporterFeesCalculatorService
    {
        public async Task<Common.Dtos.Response.RegistrationFees.ReprocessorOrExporter.RegistrationFees?> CalculateFeesAsync(ReprocessorOrExporterRegistrationFeesRequestDto request, CancellationToken cancellationToken)
        {
            Common.Dtos.Response.RegistrationFees.ReprocessorOrExporter.RegistrationFees? response = default;

            var regulator = RegulatorType.Create(request.Regulator);
            
            var registrationFeeEntity = await feeRepository.GetFeeAsync((int)request.RequestorType, (int)request.MaterialType, regulator, request.SubmissionDate, cancellationToken);
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
                var payment = await paymentsRepository.GetPreviousPaymentIncludeChildrenByReferenceAsync(request.ApplicationReferenceNumber, cancellationToken);
                if (payment is null)
                {
                    return response;
                }

                response.PreviousPaymentDetail = new()
                {
                    PaymentAmount = payment.Amount
                };

                if (payment.OfflinePayment is not null)
                {
                    response.PreviousPaymentDetail.PaymentMode = PaymentTypes.Offline.GetDescription();
                    response.PreviousPaymentDetail.PaymentDate = payment.OfflinePayment.PaymentDate.GetValueOrDefault();
                    response.PreviousPaymentDetail.PaymentMethod = payment.OfflinePayment.PaymentMethod;
                }
                else if (payment.OnlinePayment is not null)
                {
                    response.PreviousPaymentDetail.PaymentMode = PaymentTypes.Online.GetDescription();
                    response.PreviousPaymentDetail.PaymentDate = payment.UpdatedDate;
                }
            }

            return response;
        }
    }
}
