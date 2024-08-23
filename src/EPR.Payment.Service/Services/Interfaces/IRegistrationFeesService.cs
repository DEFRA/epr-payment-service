namespace EPR.Payment.Service.Services.Interfaces
{
    public interface IRegistrationFeesService
    {
        Task<decimal?> GetProducerResubmissionAmountByRegulatorAsync(string regulator, CancellationToken cancellationToken);
    }
}
