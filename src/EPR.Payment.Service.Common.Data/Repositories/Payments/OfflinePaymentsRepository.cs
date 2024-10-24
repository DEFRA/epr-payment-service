using EPR.Payment.Service.Common.Constants.Payments;
using EPR.Payment.Service.Common.Data.Interfaces;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.Payments;

namespace EPR.Payment.Service.Common.Data.Repositories.Payments
{
    public class OfflinePaymentsRepository : IOfflinePaymentsRepository
    {
        private readonly IAppDbContext _dataContext;
        public OfflinePaymentsRepository(IAppDbContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<Guid> InsertOfflinePaymentAsync(DataModels.OfflinePayment? entity, CancellationToken cancellationToken)
        {
            if (entity == null)
            {
                throw new ArgumentException(PaymentConstants.InvalidInputToInsertPaymentError);
            }

            //TODO: Should Regulator come from the user somehow???
            entity.Regulator = "GB-ENG";
            //TODO: is this correct???
            entity.ReasonForPayment = "registration fee";
            entity.CreatedDate = DateTime.UtcNow;
            entity.UpdatedDate = entity.CreatedDate;
            entity.UpdatedByUserId = entity.UserId;

            _dataContext.OfflinePayment.Add(entity);

            await _dataContext.SaveChangesAsync(cancellationToken);

            return entity.ExternalPaymentId;
        }
    }
}
