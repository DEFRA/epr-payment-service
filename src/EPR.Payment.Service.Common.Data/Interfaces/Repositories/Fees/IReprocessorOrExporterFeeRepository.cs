using EPR.Payment.Service.Common.Enums;
using EPR.Payment.Service.Common.ValueObjects.RegistrationFees;

namespace EPR.Payment.Service.Common.Data.Interfaces.Repositories.Fees
{
    public interface IReprocessorOrExporterFeeRepository
    {
        Task<DataModels.Lookups.RegistrationFees?> GetFeeAsync(int groupId, int subgroupId, RegulatorType regulator, DateTime submissionDate, CancellationToken cancellationToken);
    }
}
