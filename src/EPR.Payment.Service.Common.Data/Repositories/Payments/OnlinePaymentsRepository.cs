using EPR.Payment.Service.Common.Constants.Payments;
using EPR.Payment.Service.Common.Data.Interfaces;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.Payments;
using Microsoft.EntityFrameworkCore;

namespace EPR.Payment.Service.Common.Data.Repositories.Payments
{
    public class OnlinePaymentsRepository : IOnlinePaymentsRepository
    {
        private readonly IAppDbContext _dataContext;
        public OnlinePaymentsRepository(IAppDbContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<Guid> InsertOnlinePaymentAsync(DataModels.OnlinePayment? entity, CancellationToken cancellationToken)
        {

            if (entity == null)
            {
                throw new ArgumentException(PaymentConstants.InvalidInputToInsertPaymentError);
            }

            entity.CreatedDate = DateTime.UtcNow;
            entity.UpdatedDate = entity.CreatedDate;
            entity.UpdatedByUserId = entity.UserId;
            entity.UpdatedByOrganisationId = entity.OrganisationId;
            entity.GovPayStatus = Enum.GetName(typeof(Enums.Status), entity.InternalStatusId);


            _dataContext.OnlinePayment.Add(entity);

            await _dataContext.SaveChangesAsync(cancellationToken);

            return entity.ExternalPaymentId;
        }

        public async Task UpdateOnlinePaymentAsync(DataModels.OnlinePayment? entity, CancellationToken cancellationToken)
        {
            if (entity == null)
            {
                throw new ArgumentException(PaymentConstants.InvalidInputToUpdatePaymentError);
            }

            entity.UpdatedDate = DateTime.Now;
            entity.GovPayStatus = Enum.GetName(typeof(Enums.Status), entity.InternalStatusId);
            _dataContext.OnlinePayment.Update(entity);
            await _dataContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<DataModels.OnlinePayment?> GetOnlinePaymentByExternalPaymentIdAsync(Guid externalPaymentId, CancellationToken cancellationToken)
        {
            var entity = await _dataContext.OnlinePayment
                .Where(a => a.ExternalPaymentId == externalPaymentId)
                .SingleOrDefaultAsync(cancellationToken); // Pass the cancellationToken here

            if (entity == null)
            {
                throw new KeyNotFoundException($"{PaymentConstants.RecordNotFoundPaymentError}: {externalPaymentId}");
            }

            return entity;
        }

        public async Task<int> GetPaymentStatusCount(CancellationToken cancellationToken)
        {
            return await _dataContext.PaymentStatus.CountAsync(cancellationToken);
        }
    }
}
