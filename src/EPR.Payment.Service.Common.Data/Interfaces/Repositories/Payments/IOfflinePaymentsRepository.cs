namespace EPR.Payment.Service.Common.Data.Interfaces.Repositories.Payments
{
    public interface IOfflinePaymentsRepository
    {
        Task InsertOfflinePaymentAsync(DataModels.Payment? entity, CancellationToken cancellationToken);
    }
}
