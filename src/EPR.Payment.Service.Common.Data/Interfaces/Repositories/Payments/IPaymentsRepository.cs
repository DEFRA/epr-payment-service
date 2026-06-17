using System.Threading.Tasks;

namespace EPR.Payment.Service.Common.Data.Interfaces.Repositories.Payments
{
    public interface IPaymentsRepository
    {
        Task<decimal> GetPreviousPaymentsByReferenceAsync(string reference, CancellationToken cancellationToken);

        Task<decimal> GetPreviousPaymentsByFileIdAsync(Guid fileId, CancellationToken cancellationToken);

        Task<DataModels.Payment?> GetPreviousPaymentIncludeChildrenByReferenceAsync(string reference, CancellationToken cancellationToken);
    }
}
