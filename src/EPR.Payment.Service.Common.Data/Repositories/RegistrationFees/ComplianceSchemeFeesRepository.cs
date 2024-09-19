using EPR.Payment.Service.Common.Constants.RegistrationFees.Exceptions;
using EPR.Payment.Service.Common.Constants.RegistrationFees.LookUps;
using EPR.Payment.Service.Common.Data.Interfaces;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.RegistrationFees;
using EPR.Payment.Service.Common.ValueObjects.RegistrationFees;
using Microsoft.EntityFrameworkCore;

namespace EPR.Payment.Service.Common.Data.Repositories.RegistrationFees
{
    public class ComplianceSchemeFeesRepository : IComplianceSchemeFeesRepository
    {
        private readonly IAppDbContext _dataContext;

        public ComplianceSchemeFeesRepository(IAppDbContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<decimal> GetBaseFeeAsync(RegulatorType regulator, CancellationToken cancellationToken)
        {
            var currentDate = DateTime.UtcNow.Date;

            var fee = await _dataContext.RegistrationFees
                .Where(r => r.Group.Type.ToLower() == GroupTypeConstants.ComplianceScheme.ToLower() &&
                            r.SubGroup.Type.ToLower() == ComplianceSchemeConstants.Registration.ToLower() &&
                            r.Regulator.Type.ToLower() == regulator.Value.ToLower() &&
                            r.EffectiveFrom.Date <= currentDate &&
                            r.EffectiveTo.Date >= currentDate)
                .OrderByDescending(r => r.EffectiveFrom)
                .Select(r => r.Amount)
                .FirstOrDefaultAsync(cancellationToken);

            if (fee == 0)
            {
                throw new KeyNotFoundException(string.Format(ComplianceSchemeFeeCalculationExceptions.InvalidComplianceSchemeOrRegulatorError, regulator.Value));
            }

            return fee;
        }
    }
}