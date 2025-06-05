using AutoMapper;
using EPR.Payment.Service.Common.Dtos.Request.Payments;
using EPR.Payment.Service.Common.Dtos.Response.Payments;

namespace EPR.Payment.Service.Common.Data.Profiles
{
    public class PaymentProfile : Profile
    {
        public PaymentProfile()
        {
            CreateMap<OnlinePaymentInsertRequestDto, DataModels.Payment>()
                .ForMember(dest => dest.InternalStatusId, opt => opt.MapFrom(src => src.Status));

            CreateMap<OnlinePaymentInsertRequestDto, DataModels.OnlinePayment>()
                .ReverseMap();

            CreateMap<OnlinePaymentInsertRequestV2Dto, DataModels.Payment>()
                .IncludeBase<OnlinePaymentInsertRequestDto, DataModels.Payment>();

            CreateMap<OnlinePaymentInsertRequestV2Dto, DataModels.OnlinePayment>()
                .IncludeBase<OnlinePaymentInsertRequestDto, DataModels.OnlinePayment>()
                // .ForMember(dest => dest.RequestorType, opt => opt.MapFrom(src => src.RequestorType))
                .ReverseMap();

            CreateMap<OnlinePaymentUpdateRequestDto, DataModels.Payment>()
                .ForMember(dest => dest.InternalStatusId, opt => opt.MapFrom(src => src.Status));

            CreateMap<OnlinePaymentUpdateRequestDto, DataModels.OnlinePayment>()
                .ReverseMap();

            CreateMap<DataModels.Payment, OnlinePaymentResponseDto>()
                .ForMember(dest => dest.GovPayPaymentId, opt => opt.MapFrom(src => src.OnlinePayment.GovPayPaymentId))
                .ForMember(dest => dest.UpdatedByOrganisationId, opt => opt.MapFrom(src => src.OnlinePayment.UpdatedByOrgId))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.ReasonForPayment));

            CreateMap<OfflinePaymentInsertRequestDto, DataModels.Payment>()
                .ForMember(dest => dest.ReasonForPayment, opt => opt.MapFrom(src => src.Description));

            CreateMap<OfflinePaymentInsertRequestDto, DataModels.OfflinePayment>()
                .ReverseMap();

            CreateMap<OfflinePaymentInsertRequestV2Dto, DataModels.Payment>()
                .ForMember(dest => dest.ReasonForPayment, opt => opt.MapFrom(src => src.Description));

            CreateMap<OfflinePaymentInsertRequestV2Dto, DataModels.OfflinePayment>()
                .ReverseMap();
        }
    }
}
