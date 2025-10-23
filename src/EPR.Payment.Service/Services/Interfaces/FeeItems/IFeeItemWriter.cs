using EPR.Payment.Service.Common.Dtos.FeeItems;

namespace EPR.Payment.Service.Services.Interfaces.FeeItems
{
    public interface IFeeItemWriter
    {
        Task Save(FeeItemSaveRequest request, CancellationToken cancellationToken);

    }
}
