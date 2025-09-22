using EPR.Payment.Service.Common.Dtos.FeeSummaries;

namespace EPR.Payment.Service.Services.Interfaces.FeeSummaries
{
    public interface IFeeSummaryWriter
    {
        Task Save(FeeSummarySaveRequest request, CancellationToken cancellationToken);

    }
}
