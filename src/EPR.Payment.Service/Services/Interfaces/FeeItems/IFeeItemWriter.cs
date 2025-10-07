using EPR.Payment.Service.Common.Dtos.FeeItems;

namespace EPR.Payment.Service.Services.Interfaces.FeeItems
{
    public interface IFeeItemWriter
    {
        Task Save(FeeSummarySaveRequest request, CancellationToken cancellationToken);

    }
}
