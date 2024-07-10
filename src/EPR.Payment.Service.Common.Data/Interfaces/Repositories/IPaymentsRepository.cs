namespace EPR.Payment.Service.Common.Data.Interfaces.Repositories
{
    public interface IPaymentsRepository
    {
        Task<Guid> InsertPaymentStatusAsync(DataModels.Payment? entity);
        Task UpdatePaymentStatusAsync(DataModels.Payment? entity);
        Task<DataModels.Payment> GetPaymentByIdAsync(Guid id);
        Task<int> GetPaymentStatusCount();
    }
}
