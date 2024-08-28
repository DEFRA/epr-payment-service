namespace EPR.Payment.Service.Common.Data.Interfaces.Repositories.RegistrationFees
{
    public interface IProducerFeesRepository
    {
        Task<decimal> GetBaseFeeAsync(string producerType, string regulator, CancellationToken cancellationToken);

        Task<decimal> GetFirst20SubsidiariesFeeAsync(string regulator, CancellationToken cancellationToken);

        Task<decimal> GetAdditionalSubsidiariesFeeAsync(string regulator, CancellationToken cancellationToken);
    }
}