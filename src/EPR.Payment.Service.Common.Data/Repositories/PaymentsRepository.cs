using EPR.Payment.Service.Common.Data.Interfaces.Repositories;
using EPR.Payment.Service.Common.Dtos.Request;

namespace EPR.Payment.Service.Common.Data.Repositories
{
    public class PaymentsRepository : IPaymentsRepository
    {
        private readonly DataContext _dataContext;
        public PaymentsRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public Task InsertPaymentStatusAsync(string paymentId, PaymentStatusInsertRequestDto paymentStatusInsertRequest)
        {
            throw new NotImplementedException();
        }
    }
}
