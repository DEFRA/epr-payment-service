using EPR.Payment.Service.Common.Data.DataModels.Lookups;
using EPR.Payment.Service.Common.ValueObjects.RegistrationFees;

namespace EPR.Payment.Service.Common.Data.Interfaces.Repositories.Fees
{
    public interface IAccreditationFeesRepository 
    {
        Task<AccreditationFee?> GetFeeAsync(
            int groupId,
            int subGroupId,
            int tonnageBandId,
            RegulatorType regulator,
            DateTime submissionDate,
            CancellationToken cancellationToken);
    }
}