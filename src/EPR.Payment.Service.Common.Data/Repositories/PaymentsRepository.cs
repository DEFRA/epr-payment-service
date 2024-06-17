using EPR.Payment.Service.Common.Data.Enums;
using EPR.Payment.Service.Common.Data.Exceptions;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories;
using EPR.Payment.Service.Common.Dtos.Request;
using Microsoft.EntityFrameworkCore;

namespace EPR.Payment.Service.Common.Data.Repositories
{
    public class PaymentsRepository : IPaymentsRepository
    {
        private readonly PaymentDataContext _dataContext;
        public PaymentsRepository(PaymentDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task InsertPaymentStatusAsync(Guid externalPaymentId, string paymentId, PaymentStatusInsertRequestDto paymentStatusInsertRequest)
        {
            var entity = await _dataContext.Payment.
                            Where(u => u.ExternalPaymentId == externalPaymentId).
                            FirstOrDefaultAsync();

            if (entity == null)
            {
                throw new NotFoundException($"Payment record not found for External Payment ID: {externalPaymentId}");
            }

            Enum.TryParse(paymentStatusInsertRequest.Status, out Status status);

            await _dataContext.Payment
                .Where(u => u.ExternalPaymentId == externalPaymentId)
                .ExecuteUpdateAsync(b => b
                             .SetProperty(u => u.GovpayPaymentId, paymentId)
                             .SetProperty(u => u.InternalStatusId, status)
                             .SetProperty(u => u.GovPayStatus, paymentStatusInsertRequest.Status));
        }
    }
}
