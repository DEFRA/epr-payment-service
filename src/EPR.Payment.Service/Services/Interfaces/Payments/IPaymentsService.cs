namespace EPR.Payment.Service.Services.Interfaces.Payments
{
    public interface IPaymentsService
    {
        Task<decimal> GetPreviousPaymentsByReferenceAsync(string reference, CancellationToken cancellationToken);

        Task<decimal> GetPreviousPaymentsByRegistrationBlobNameAsync(string registrationBlobName, CancellationToken cancellationToken);
    }
}
