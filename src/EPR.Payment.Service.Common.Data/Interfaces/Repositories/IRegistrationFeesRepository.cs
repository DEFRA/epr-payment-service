namespace EPR.Payment.Service.Common.Data.Interfaces.Repositories
{
    public interface IRegistrationFeesRepository
    {
        Task<decimal?> GetProducerResubmissionAmountByRegulatorAsync(string regulator, CancellationToken cancellationToken);
    }
}
