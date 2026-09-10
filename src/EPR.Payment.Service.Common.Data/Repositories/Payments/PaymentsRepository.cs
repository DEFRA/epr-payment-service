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

        public async Task<decimal> GetPreviousPaymentsByRegistrationBlobNameAsync(string registrationBlobName, CancellationToken cancellationToken)
        {
            return await (
                from p in _dataContext.Payment
                join rsd in _dataContext.RegistrationSubmissionData
                    on p.Reference equals rsd.RegistrationBlobName
                where rsd.RegistrationBlobName == registrationBlobName
                   && p.InternalStatusId == Enums.Status.Success
                select p.Amount
            ).SumAsync(cancellationToken);
        }

        public async Task<DataModels.Payment?> GetPreviousPaymentIncludeChildrenByReferenceAsync(string reference, CancellationToken cancellationToken)
        {
            return await _dataContext.Payment
                                     .Include(n => n.OfflinePayment).ThenInclude(op => op.PaymentMethod)
                                     .Include(n => n.OnlinePayment).ThenInclude(op => op.RequestorType)
                                     .Where(a => a.Reference == reference
                                     && a.InternalStatusId == Enums.Status.Success)
                                     .FirstOrDefaultAsync(cancellationToken);                   
        }
    }
}