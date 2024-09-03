namespace EPR.Payment.Service.Common.Data.Interfaces.Repositories.Payments
{
    public interface IPaymentsRepository
    {
        Task<Guid> InsertPaymentStatusAsync(DataModels.Payment? entity, CancellationToken cancellationToken);
        Task UpdatePaymentStatusAsync(DataModels.Payment? entity, CancellationToken cancellationToken);
        Task<DataModels.Payment?> GetPaymentByExternalPaymentIdAsync(Guid externalPaymentId, CancellationToken cancellationToken);
        Task<int> GetPaymentStatusCount(CancellationToken cancellationToken);
    }
}
