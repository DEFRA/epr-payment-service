namespace EPR.Payment.Service.Strategies.Interfaces.RegistrationFees
{
    public interface IFeeCalculationStrategy<TRequestDto>
    {
        Task<decimal> CalculateFeeAsync(TRequestDto request, CancellationToken cancellationToken);
    }
}
