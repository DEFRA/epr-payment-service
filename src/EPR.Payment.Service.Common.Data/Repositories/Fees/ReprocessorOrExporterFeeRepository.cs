using EPR.Payment.Service.Common.Constants.RegistrationFees.Exceptions;
using EPR.Payment.Service.Common.Constants.RegistrationFees.LookUps;
using EPR.Payment.Service.Common.Data.Interfaces;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.Fees;
using EPR.Payment.Service.Common.Enums;
using EPR.Payment.Service.Common.ValueObjects.RegistrationFees;
using Microsoft.EntityFrameworkCore;

namespace EPR.Payment.Service.Common.Data.Repositories.Fees
{
    public class ReprocessorOrExporterFeeRepository : IReprocessorOrExporterFeeRepository
    {
        private readonly IAppDbContext _dataContext;

        public ReprocessorOrExporterFeeRepository(IAppDbContext dataContext)
        {
            _dataContext = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
        }

        public Task<decimal> GetBaseFeeAsync(RequestorType requestorType, MaterialType materialType, RegulatorType regulator, DateTime submissionDate, CancellationToken cancellationToken)
        {
            var fee = GetFeeAsync(requestorType, materialType, regulator, submissionDate, cancellationToken);
            ValidateFee(fee.Result, ReprocessorOrExporterFeesCalculationExceptions.InvalidFeeError);
            return fee;
        }

        protected async Task<decimal> GetFeeAsync(RequestorType requestorType, MaterialType materialType, RegulatorType regulator, DateTime submissionDate, CancellationToken cancellationToken)
        {
            var registrationFee = await _dataContext.RegistrationFees
                .Where(r => 
                    r.GroupId == (int)requestorType &&
                    r.SubGroupId == (int)materialType &&
                    r.Regulator.Type.ToLower() == regulator.Value.ToLower() &&
                    r.EffectiveFrom <= submissionDate &&
                    r.EffectiveTo > submissionDate)
               .SingleOrDefaultAsync(cancellationToken);

            return registrationFee?.Amount ?? 0m;
        }

        protected decimal ValidateFee(decimal fee, string errorMessage)
        {
            if (fee <= 0)
            {
                throw new ArgumentException(errorMessage);
            }
            return fee;
        }
    }
}
