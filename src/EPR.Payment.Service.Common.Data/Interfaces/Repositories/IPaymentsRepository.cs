namespace EPR.Payment.Service.Common.Data.Interfaces.Repositories
{
    public interface IPaymentsRepository
    {
        Task<Guid> InsertPaymentStatusAsync(DataModels.Payment entity);
        Task UpdatePaymentStatusAsync(DataModels.Payment entity);
        Task<DataModels.Payment> GetPaymentByExternalPaymentIdAsync(Guid externalPaymentId);
        Task<int> GetPaymentStatusCount();
    }
}
