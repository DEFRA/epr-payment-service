namespace EPR.Payment.Service.Common.Data.Interfaces.Repositories.Payments
{
    public interface IPaymentsRepository
    {
        Task<decimal> GetPreviousPaymentsByReferenceAsync(string reference, CancellationToken cancellationToken);
    }
}
