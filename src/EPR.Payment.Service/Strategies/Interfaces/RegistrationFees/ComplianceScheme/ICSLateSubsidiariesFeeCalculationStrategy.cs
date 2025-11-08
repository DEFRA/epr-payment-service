using EPR.Payment.Service.Strategies.Interfaces.Common;

namespace EPR.Payment.Service.Strategies.Interfaces.RegistrationFees.ComplianceScheme
{
    public interface ICSLateSubsidiariesFeeCalculationStrategy<TRequestDto, TResponse> : IFeeCalculationStrategy<TRequestDto, TResponse>
    {
    }
}
