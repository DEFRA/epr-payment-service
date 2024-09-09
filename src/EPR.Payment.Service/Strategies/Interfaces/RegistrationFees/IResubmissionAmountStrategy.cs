namespace EPR.Payment.Service.Strategies.Interfaces.RegistrationFees
{
    public interface IResubmissionAmountStrategy
    {
        Task<decimal?> GetResubmissionAsync(string regulator, CancellationToken cancellationToken);
    }
}
