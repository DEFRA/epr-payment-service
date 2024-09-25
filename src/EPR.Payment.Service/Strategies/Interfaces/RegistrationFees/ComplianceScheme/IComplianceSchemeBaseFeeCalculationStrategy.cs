using Azure;
using EPR.Payment.Service.Common.ValueObjects.RegistrationFees;

namespace EPR.Payment.Service.Strategies.Interfaces.RegistrationFees.ComplianceScheme
{
    public interface IComplianceSchemeBaseFeeCalculationStrategy<TRequestDto, TResponse> : IFeeCalculationStrategy<TRequestDto, TResponse>
    {
    }
}