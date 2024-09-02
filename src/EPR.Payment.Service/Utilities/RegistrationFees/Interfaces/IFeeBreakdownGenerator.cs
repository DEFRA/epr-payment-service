namespace EPR.Payment.Service.Utilities.RegistrationFees.Interfaces
{
    public interface IFeeBreakdownGenerator<in TRequest, in TResponse>
    {
        Task GenerateFeeBreakdownAsync(TResponse response, TRequest request, CancellationToken cancellationToken);
    }
}
