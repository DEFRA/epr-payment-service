namespace EPR.Payment.Service.Common.Data.Interfaces.Repositories.Payments
{
    public interface IOnlinePaymentsRepository
    {
        Task<Guid> InsertOnlinePaymentAsync(DataModels.Payment? entity, CancellationToken cancellationToken);
        Task UpdateOnlinePayment(DataModels.Payment? entity, CancellationToken cancellationToken);
        Task<DataModels.Payment> GetOnlinePaymentByExternalPaymentIdAsync(Guid externalPaymentId, CancellationToken cancellationToken);
        Task<int> GetPaymentStatusCount(CancellationToken cancellationToken);
    }
}
