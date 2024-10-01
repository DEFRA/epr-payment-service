using EPR.Payment.Service.Common.Dtos.Response.RegistrationFees;

namespace EPR.Payment.Service.Strategies.Interfaces.RegistrationFees.Producer
{
    public interface ISubsidiariesFeeCalculationStrategy<TRequestDto, TResponseDto> : IFeeCalculationStrategy<TRequestDto, TResponseDto>
    {
    }
}