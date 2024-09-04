namespace EPR.Payment.Service.Services.Interfaces.RegistrationFees
{
    public interface IProducerResubmissionService
    {
        Task<decimal?> GetResubmissionAsync(string regulator, CancellationToken cancellationToken);
    }
}
