using AutoMapper;
using EPR.Payment.Service.Common.Dtos.Request.Payments;
using EPR.Payment.Service.Common.Dtos.Response.Payments;

namespace EPR.Payment.Service.Common.Data.Profiles
{
    public class PaymentProfile : Profile
    {
        public PaymentProfile()
        {
            CreateMap<OnlinePaymentInsertRequestDto, DataModels.Payment>().ForMember(dest => dest.InternalStatusId, opt => opt.MapFrom(src => src.Status));

            CreateMap<OnlinePaymentUpdateRequestDto, DataModels.Payment>().ForMember(dest => dest.InternalStatusId, opt => opt.MapFrom(src => src.Status));

            CreateMap<OnlinePaymentInsertRequestDto, DataModels.OnlinePayment>().ReverseMap();

            CreateMap<OnlinePaymentUpdateRequestDto, DataModels.OnlinePayment>().ReverseMap();

            CreateMap<DataModels.Payment, OnlinePaymentResponseDto>().ForMember(dest => dest.GovPayPaymentId, opt => opt.MapFrom(src => src.OnlinePayment.GovPayPaymentId))
                                                                     .ForMember(dest => dest.UpdatedByOrganisationId, opt => opt.MapFrom(src => src.OnlinePayment.UpdatedByOrgId));
        }
    }
}
