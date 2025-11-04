using EPR.Payment.Service.Common.Data.DataModels;
using EPR.Payment.Service.Common.Data.Dtos;

namespace EPR.Payment.Service.Common.Data.Interfaces.Repositories.FeeItems
{
    public interface IFeeItemRepository
    {
        Task UpsertAsync(FeeItemMappedRequest request, CancellationToken cancellationToken);
    }
}
