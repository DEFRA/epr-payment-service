using EPR.Payment.Service.Common.Data.Interfaces.Repositories.Payments;
using EPR.Payment.Service.Common.Dtos.Response.Payments;
using EPR.Payment.Service.Common.Enums;
using EPR.Payment.Service.Common.Extensions;
using EPR.Payment.Service.Services.Interfaces.Payments;

namespace EPR.Payment.Service.Services.Payments
{
    public class PreviousPaymentsHelper(IPaymentsRepository paymentsRepository) : IPreviousPaymentsHelper
    {
        public async Task<PreviousPaymentDetailResponseDto?> GetPreviousPaymentAsync(
            string applicationReferenceNumber,
            CancellationToken cancellationToken)
        {
            Common.Data.DataModels.Payment? payment = await paymentsRepository.GetPreviousPaymentIncludeChildrenByReferenceAsync(applicationReferenceNumber, cancellationToken);

            if (payment is null)
            {
                return default;
            }

            PreviousPaymentDetailResponseDto previousPayment = new()
            {
                PaymentAmount = payment.Amount
            };

            if (payment.OfflinePayment is not null)
            {
                previousPayment.PaymentMode = PaymentTypes.Offline.GetDescription();
                previousPayment.PaymentDate = payment.OfflinePayment.PaymentDate.GetValueOrDefault();
                previousPayment.PaymentMethod = payment.OfflinePayment.PaymentMethod.Type;
            }
            else if (payment.OnlinePayment is not null)
            {
                previousPayment.PaymentMode = PaymentTypes.Online.GetDescription();
                previousPayment.PaymentDate = payment.UpdatedDate;
                previousPayment.PaymentMethod = "GovPay";
            }

            return previousPayment;
        }
    }
}
