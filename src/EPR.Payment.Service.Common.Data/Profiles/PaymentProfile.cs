using AutoMapper;
using EPR.Payment.Service.Common.Dtos.Request.Payments;
using EPR.Payment.Service.Common.Dtos.Response.Payments;

namespace EPR.Payment.Service.Common.Data.Profiles
{
    public class PaymentProfile : Profile
    {
        public PaymentProfile()
        {
            CreateMap<OnlinePaymentStatusInsertRequestDto, DataModels.OnlinePayment>().ForMember(dest => dest.InternalStatusId, opt => opt.MapFrom(src => src.Status));

            CreateMap<OnlinePaymentStatusUpdateRequestDto, DataModels.OnlinePayment>().ForMember(dest => dest.InternalStatusId, opt => opt.MapFrom(src => src.Status));

            CreateMap<OnlinePaymentResponseDto, DataModels.OnlinePayment>().ReverseMap();
        }
    }
}
