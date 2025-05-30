using EPR.Payment.Service.Common.Dtos.Response.Common;
using EPR.Payment.Service.Common.Enums;
using EPR.Payment.Service.Services.Interfaces.Payments;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.Payments;
using EPR.Payment.Service.Common.Extensions;

namespace EPR.Payment.Service.Services.Payments
{
    public class PreviousPaymentsHelper(IPaymentsRepository paymentsRepository) : IPreviousPaymentsHelper
    {
        public async Task<T?> GetPreviousPaymentAsync<T>(string applicationReferenceNumber, CancellationToken cancellationToken) where T : BasePreviousPaymentDetailDto, new()
        {
            var payment = await paymentsRepository.GetPreviousPaymentIncludeChildrenByReferenceAsync(applicationReferenceNumber, cancellationToken);

            if (payment is null)
            {
                return default;
            }

            var previousPayment = new T
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

            return previousPayment;
        }
    }
}
