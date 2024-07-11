namespace EPR.Payment.Service.Common.Data.Interfaces.Repositories
{
    public interface IPaymentsRepository
    {
        Task<Guid> InsertPaymentStatusAsync(DataModels.Payment? entity, CancellationToken cancellationToken);
        Task UpdatePaymentStatusAsync(DataModels.Payment? entity, CancellationToken cancellationToken);
        Task<DataModels.Payment> GetPaymentByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<int> GetPaymentStatusCount(CancellationToken cancellationToken);
    }
}
