namespace EPR.Payment.Service.Strategies.Interfaces.Common
{
    public interface IFeeCalculationStrategy<TRequestDto, TResponse>
    {
        Task<TResponse> CalculateFeeAsync(TRequestDto request, CancellationToken cancellationToken);
    }
}