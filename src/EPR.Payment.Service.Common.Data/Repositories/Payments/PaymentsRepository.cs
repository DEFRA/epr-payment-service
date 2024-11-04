using EPR.Payment.Service.Common.Data.Interfaces;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.Payments;
using Microsoft.EntityFrameworkCore;

namespace EPR.Payment.Service.Common.Data.Repositories.Payments
{
    public class PaymentsRepository : IPaymentsRepository
    {
        private readonly IAppDbContext _dataContext;
        public PaymentsRepository(IAppDbContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<decimal> GetPreviousPaymentsByReferenceAsync(string reference, CancellationToken cancellationToken)
        {
            return await _dataContext.Payment
                   .Where(a => a.Reference == reference && 
                               a.InternalStatusId == Enums.Status.Success)
                   .SumAsync(a => a.Amount, cancellationToken);
        }
    }
}
