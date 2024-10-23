namespace EPR.Payment.Service.Common.Data.Interfaces.Repositories.Payments
{
    public interface IOnlinePaymentsRepository
    {
        Task<Guid> InsertOnlinePaymentAsync(DataModels.OnlinePayment? entity, CancellationToken cancellationToken);
        Task UpdateOnlinePaymentAsync(DataModels.OnlinePayment? entity, CancellationToken cancellationToken);
        Task<DataModels.OnlinePayment?> GetOnlinePaymentByExternalPaymentIdAsync(Guid externalPaymentId, CancellationToken cancellationToken);
        Task<int> GetPaymentStatusCount(CancellationToken cancellationToken);
    }
}
