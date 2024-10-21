using EPR.Payment.Service.Strategies.Interfaces.RegistrationFees;

namespace EPR.Payment.Service.Strategies.Interfaces.ResubmissionFees.ComplianceScheme
{
    public interface IComplianceSchemeResubmissionFeeCalculationStrategy<TRequestDto, TResponseDto> : IFeeCalculationStrategy<TRequestDto, TResponseDto>
    {
    }
}