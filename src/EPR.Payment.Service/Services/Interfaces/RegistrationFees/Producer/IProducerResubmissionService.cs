namespace EPR.Payment.Service.Services.Interfaces.RegistrationFees.Producer
{
    public interface IProducerResubmissionService
    {
        Task<decimal?> GetResubmissionAsync(string regulator, CancellationToken cancellationToken);
    }
}