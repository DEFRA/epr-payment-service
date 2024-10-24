using EPR.Payment.Service.Strategies.Interfaces.Common;

namespace EPR.Payment.Service.Strategies.Interfaces.RegistrationFees.Producer
{
    public interface ILateFeeCalculationStrategy<TRequestDto, TResponse> : IFeeCalculationStrategy<TRequestDto, TResponse>
    {
    }
}
