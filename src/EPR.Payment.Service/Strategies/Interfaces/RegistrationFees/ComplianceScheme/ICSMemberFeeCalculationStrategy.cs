using EPR.Payment.Service.Strategies.Interfaces.Common;

namespace EPR.Payment.Service.Strategies.Interfaces.RegistrationFees.ComplianceScheme
{
    public interface ICSMemberFeeCalculationStrategy<TRequestDto, TResponse> : IFeeCalculationStrategy<TRequestDto, TResponse>
    {
    }
}
