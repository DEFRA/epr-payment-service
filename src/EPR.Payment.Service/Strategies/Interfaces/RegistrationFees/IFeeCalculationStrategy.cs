namespace EPR.Payment.Service.Strategies.Interfaces.RegistrationFees
{
    public interface IFeeCalculationStrategy<TRequestDto, TResponse>
    {
        Task<TResponse> CalculateFeeAsync(TRequestDto request, CancellationToken cancellationToken);
    }
}
