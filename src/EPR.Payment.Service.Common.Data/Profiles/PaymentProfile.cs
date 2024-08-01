using AutoMapper;
using EPR.Payment.Service.Common.Dtos.Request;
using EPR.Payment.Service.Common.Dtos.Response;

namespace EPR.Payment.Service.Common.Data.Profiles
{
    public class PaymentProfile : Profile
    {
        public PaymentProfile() 
        {
            CreateMap<PaymentStatusInsertRequestDto, DataModels.Payment>().ForMember(dest => dest.InternalStatusId, opt => opt.MapFrom(src => src.Status));

            CreateMap<PaymentStatusUpdateRequestDto, DataModels.Payment>().ForMember(dest => dest.InternalStatusId, opt => opt.MapFrom(src => src.Status));

            CreateMap<PaymentResponseDto, DataModels.Payment>().ReverseMap();

        }
    }
}
