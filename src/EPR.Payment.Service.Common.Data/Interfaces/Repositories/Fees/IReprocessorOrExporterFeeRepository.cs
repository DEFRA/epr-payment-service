using EPR.Payment.Service.Common.Enums;
using EPR.Payment.Service.Common.ValueObjects.RegistrationFees;

namespace EPR.Payment.Service.Common.Data.Interfaces.Repositories.Fees
{
    public interface IReprocessorOrExporterFeeRepository
    {
        Task<decimal> GetBaseFeeAsync(RequestorType requestorType, MaterialType materialType, RegulatorType regulator, DateTime submissionDate, CancellationToken cancellationToken);
    }
}
