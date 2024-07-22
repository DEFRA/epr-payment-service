using EPR.Payment.Service.Common.Constants;
using EPR.Payment.Service.Common.Data.Interfaces;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EPR.Payment.Service.Common.Data.Repositories
{
    public class PaymentsRepository : IPaymentsRepository
    {
        private readonly IAppDbContext _dataContext;
        public PaymentsRepository(IAppDbContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<Guid> InsertPaymentStatusAsync(DataModels.Payment? entity, CancellationToken cancellationToken)
        {

            if (entity == null)
            {
                throw new ArgumentException(PaymentConstants.InvalidInputToInsertPaymentError);
            }

            entity.CreatedDate = DateTime.Now;
            entity.UpdatedDate = entity.CreatedDate;
            entity.UpdatedByUserId = entity.UserId;
            entity.UpdatedByOrganisationId = entity.OrganisationId;
            entity.GovPayStatus = Enum.GetName(typeof(Enums.Status), entity.InternalStatusId);


            _dataContext.Payment.Add(entity);

            await _dataContext.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }

        public async Task UpdatePaymentStatusAsync(DataModels.Payment? entity, CancellationToken cancellationToken)
        {
            if (entity == null)
            {
                throw new ArgumentException(PaymentConstants.InvalidInputToUpdatePaymentError);
            }

            entity.UpdatedDate = DateTime.Now;
            entity.GovPayStatus = Enum.GetName(typeof(Enums.Status), entity.InternalStatusId);
            _dataContext.Payment.Update(entity);
            await _dataContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<DataModels.Payment> GetPaymentByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var entity =  await _dataContext.Payment.Where(a => a.Id == id).SingleOrDefaultAsync();

            if (entity == null)
            {
                throw new KeyNotFoundException($"{PaymentConstants.RecordNotFoundPaymentError}: {id}");
            }

            return entity;
        }

        public async Task<int> GetPaymentStatusCount(CancellationToken cancellationToken)
        {
            return await _dataContext.PaymentStatus.CountAsync();
        }
    }
}
