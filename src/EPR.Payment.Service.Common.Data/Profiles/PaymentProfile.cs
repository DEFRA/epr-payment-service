using AutoMapper;
using EPR.Payment.Service.Common.Dtos.Request;

namespace EPR.Payment.Service.Common.Data.Profiles
{
    public class PaymentProfile : Profile
    {
        public PaymentProfile() 
        {
            CreateMap<PaymentStatusInsertRequestDto, DataModels.Payment>().ForMember(dest => dest.InternalStatusId, opt => opt.MapFrom(src => src.Status));

            CreateMap<PaymentStatusUpdateRequestDto, DataModels.Payment>().ForMember(dest => dest.InternalStatusId, opt => opt.MapFrom(src => src.Status)).ForMember(dest => dest.InternalErrorCode, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.ErrorCode) ? null : src.ErrorCode));

        }
    }
}
