using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.Producer;
using EPR.Payment.Service.Strategies.Interfaces.Common;

namespace EPR.Payment.Service.Strategies.Interfaces.ResubmissionFees.Producer
{
    public interface IResubmissionAmountStrategy<TRequestDto, TResponseDto> : IFeeCalculationStrategy<TRequestDto, TResponseDto>
    {
    }
}
