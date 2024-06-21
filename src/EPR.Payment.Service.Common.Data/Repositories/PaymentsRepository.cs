using EPR.Payment.Service.Common.Data.Exceptions;
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

        public async Task<Guid> InsertPaymentStatusAsync(DataModels.Payment entity)
        {
            entity.CreatedDate = DateTime.Now;
            entity.UpdatedDate = entity.CreatedDate;
            entity.UpdatedByUserId = entity.UserId;
            entity.UpdatedByOrganisationId = entity.OrganisationId;
            entity.ExternalPaymentId = Guid.NewGuid();

           
            _dataContext.Payment.Add(entity);

            await _dataContext.SaveChangesAsync();

            return entity.ExternalPaymentId;
        }

        public async Task UpdatePaymentStatusAsync(DataModels.Payment entity)
        {
            entity.UpdatedDate = DateTime.Now;
            entity.GovPayStatus = Enum.GetName(typeof(Enums.Status), entity.InternalStatusId);
            _dataContext.Payment.Update(entity);
            await _dataContext.SaveChangesAsync();
        }

        public async Task<DataModels.Payment> GetPaymentByExternalPaymentIdAsync(Guid externalPaymentId)
        {
            var entity =  await _dataContext.Payment.Where(a => a.ExternalPaymentId == externalPaymentId).SingleOrDefaultAsync();

            if (entity == null)
            {
                throw new NotFoundException($"Payment record not found for External Payment ID: {externalPaymentId}");
            }

            return entity;
        }

        public async Task<int> GetPaymentStatusCount()
        {
            return await _dataContext.PaymentStatus.CountAsync();
        }
    }
}
