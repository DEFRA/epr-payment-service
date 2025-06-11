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

        public async Task<Guid> InsertOnlinePaymentAsync(DataModels.Payment? entity, CancellationToken cancellationToken)
        {
            if (entity == null)
            {
                throw new ArgumentException(PaymentConstants.InvalidInputToInsertPaymentError);
            }

            entity.CreatedDate = DateTime.UtcNow;
            entity.UpdatedDate = entity.CreatedDate;
            entity.UpdatedByUserId = entity.UserId;
            entity.OnlinePayment.UpdatedByOrgId = entity.OnlinePayment.OrganisationId;
            entity.OnlinePayment.GovPayStatus = Enum.GetName(typeof(Enums.Status), entity.InternalStatusId);

            _dataContext.Payment.Add(entity); 
            await _dataContext.SaveChangesAsync(cancellationToken);

            return entity.ExternalPaymentId;
        }

        public async Task UpdateOnlinePayment(DataModels.Payment? entity, CancellationToken cancellationToken)
        {
            if (entity == null)
            {
                throw new ArgumentException(PaymentConstants.InvalidInputToUpdatePaymentError);
            }

            entity.UpdatedDate = DateTime.UtcNow;
            entity.OnlinePayment.GovPayStatus = Enum.GetName(typeof(Enums.Status), entity.InternalStatusId);
            _dataContext.Payment.Update(entity);
            await _dataContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<DataModels.Payment> GetOnlinePaymentByExternalPaymentIdAsync(Guid externalPaymentId, CancellationToken cancellationToken)
        {
            var entity = await _dataContext.Payment
                .Include(p => p.OnlinePayment)
                .ThenInclude(op => op.RequestorType)
                .Where(a => a.ExternalPaymentId == externalPaymentId)
                .SingleOrDefaultAsync(cancellationToken);

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
