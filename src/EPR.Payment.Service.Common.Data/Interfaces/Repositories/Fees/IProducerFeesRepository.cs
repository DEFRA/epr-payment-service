using EPR.Payment.Service.Common.ValueObjects.RegistrationFees;

namespace EPR.Payment.Service.Common.Data.Interfaces.Repositories.RegistrationFees
{
    public interface IProducerFeesRepository : IFeeRepository
    {
        Task<decimal> GetBaseFeeAsync(string producer, RegulatorType regulator, DateTime submissionDate, CancellationToken cancellationToken);

        Task<decimal> GetLateFeeAsync(RegulatorType regulator, DateTime submissionDate, CancellationToken cancellationToken);

        Task<decimal> GetResubmissionAsync(RegulatorType regulator, CancellationToken cancellationToken);
    }
}