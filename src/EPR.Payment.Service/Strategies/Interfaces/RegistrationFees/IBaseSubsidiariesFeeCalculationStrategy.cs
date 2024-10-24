using EPR.Payment.Service.Common.Dtos.Response.RegistrationFees;
using EPR.Payment.Service.Strategies.Interfaces.Common;

namespace EPR.Payment.Service.Strategies.Interfaces.RegistrationFees
{
    public interface IBaseSubsidiariesFeeCalculationStrategy<TRequestDto, TResponseDto> : IFeeCalculationStrategy<TRequestDto, TResponseDto>
    {
    }
}