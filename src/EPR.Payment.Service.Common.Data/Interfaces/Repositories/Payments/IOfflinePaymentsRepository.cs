namespace EPR.Payment.Service.Common.Data.Interfaces.Repositories.Payments
{
    public interface IOfflinePaymentsRepository
    {
        Task<Guid> InsertOfflinePaymentAsync(DataModels.OfflinePayment? entity, CancellationToken cancellationToken);
    }
}
