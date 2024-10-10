using EPR.Payment.Service.Common.Dtos.Response.RegistrationFees;

namespace EPR.Payment.Service.Strategies.Interfaces.RegistrationFees
{
    public interface IBaseSubsidiariesFeeCalculationStrategy<TRequestDto, TResponseDto> : IFeeCalculationStrategy<TRequestDto, TResponseDto>
    {
    }
}