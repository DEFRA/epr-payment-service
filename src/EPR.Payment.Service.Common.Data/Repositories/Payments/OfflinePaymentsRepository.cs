using EPR.Payment.Service.Common.Constants.Payments;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.Payments;
using EPR.Payment.Service.Common.Data.Interfaces;

namespace EPR.Payment.Service.Common.Data.Repositories.Payments
{
    public class OfflinePaymentsRepository : IOfflinePaymentsRepository
    {
        private readonly IAppDbContext _dataContext;
        public OfflinePaymentsRepository(IAppDbContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task InsertOfflinePaymentAsync(DataModels.Payment? entity, CancellationToken cancellationToken)
        {
            if (entity == null)
            {
                throw new ArgumentException(PaymentConstants.InvalidInputToInsertPaymentError);
            }

            entity.CreatedDate = DateTime.UtcNow;
            entity.UpdatedDate = entity.CreatedDate;
            entity.UpdatedByUserId = entity.UserId;

            _dataContext.Payment.Add(entity);

            await _dataContext.SaveChangesAsync(cancellationToken);
        }
    }
}
