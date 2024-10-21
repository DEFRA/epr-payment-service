namespace EPR.Payment.Service.Common.Data.Interfaces.Repositories.Payments
{
    public interface IOnlinePaymentsRepository
    {
        Task<Guid> InsertPaymentStatusAsync(DataModels.OnlinePayment? entity, CancellationToken cancellationToken);
        Task UpdatePaymentStatusAsync(DataModels.OnlinePayment? entity, CancellationToken cancellationToken);
        Task<DataModels.OnlinePayment?> GetOnlinePaymentByExternalPaymentIdAsync(Guid externalPaymentId, CancellationToken cancellationToken);
        Task<int> GetPaymentStatusCount(CancellationToken cancellationToken);
    }
}
