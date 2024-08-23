using EPR.Payment.Service.Common.Constants;
using EPR.Payment.Service.Common.Data.Enums;
using EPR.Payment.Service.Common.Data.Interfaces;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EPR.Payment.Service.Common.Data.Repositories
{
    public class RegistrationFeesRepository : IRegistrationFeesRepository
    {
        private readonly IAppDbContext _dataContext;
        public RegistrationFeesRepository(IAppDbContext dataContext)
        {
            _dataContext = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
        }
        public async Task<decimal?> GetProducerResubmissionAmountByRegulatorAsync(string regulator, CancellationToken cancellationToken)
        {
            var entity = await _dataContext.RegistrationFees.
                Include(s => s.Regulator).
                Where(a => 
                          a.GroupId == (int)Group.ProducerResubmission &&
                          a.SubGroupId == (int)SubGroup.ReSubmitting &&
                          a.Regulator.Type == regulator).SingleOrDefaultAsync();

            if (entity == null)
            {
                throw new KeyNotFoundException($"{FeesConstants.RecordNotFoundProducerResubmissionFeeError}: {regulator}");
            }

            return entity.Amount;
        }
    }
}
