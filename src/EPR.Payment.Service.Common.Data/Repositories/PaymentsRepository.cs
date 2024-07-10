using EPR.Payment.Service.Common.Constants;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EPR.Payment.Service.Common.Data.Repositories
{
    public class PaymentsRepository : IPaymentsRepository
    {
        private readonly AppDbContext _dataContext;
        public PaymentsRepository(AppDbContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<Guid> InsertPaymentStatusAsync(DataModels.Payment? entity)
        {

            if (entity == null)
            {
                throw new ArgumentException(PaymentConstants.InvalidInputToInsertPaymentError);
            }

            entity.CreatedDate = DateTime.Now;
            entity.UpdatedDate = entity.CreatedDate;
            entity.UpdatedByUserId = entity.UserId;
            entity.UpdatedByOrganisationId = entity.OrganisationId;

           
            _dataContext.Payment.Add(entity);

            await _dataContext.SaveChangesAsync();

            return entity.Id;
        }

        public async Task UpdatePaymentStatusAsync(DataModels.Payment? entity)
        {
            if (entity == null)
            {
                throw new ArgumentException(PaymentConstants.InvalidInputToUpdatePaymentError);
            }

            entity.UpdatedDate = DateTime.Now;
            entity.GovPayStatus = Enum.GetName(typeof(Enums.Status), entity.InternalStatusId);
            _dataContext.Payment.Update(entity);
            await _dataContext.SaveChangesAsync();
        }

        public async Task<DataModels.Payment> GetPaymentByIdAsync(Guid id)
        {
            var entity =  await _dataContext.Payment.Where(a => a.Id == id).SingleOrDefaultAsync();

            if (entity == null)
            {
                throw new KeyNotFoundException($"{PaymentConstants.RecordNotFoundPaymentError}: {id}");
            }

            return entity;
        }

        public async Task<int> GetPaymentStatusCount()
        {
            return await _dataContext.PaymentStatus.CountAsync();
        }
    }
}
