﻿using AutoMapper;
using EPR.Payment.Service.Common.Data.Constants;
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
                .ForMember(dest => dest.RequestorType, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<OnlinePaymentInsertRequestV2Dto, DataModels.Payment>()
                .IncludeBase<OnlinePaymentInsertRequestDto, DataModels.Payment>();

            CreateMap<OnlinePaymentInsertRequestV2Dto, DataModels.OnlinePayment>()
                .ForMember(dest => dest.RequestorTypeId, opt => opt.MapFrom(src => src.RequestorType.HasValue ? (int)src.RequestorType.Value : DefaultDataConstants.NotApplicableIdValue))
                .ForMember(dest => dest.RequestorType, opt => opt.Ignore())
                .IncludeBase<OnlinePaymentInsertRequestDto, DataModels.OnlinePayment>()
                .ReverseMap();

            CreateMap<OnlinePaymentUpdateRequestDto, DataModels.Payment>()
                .ForMember(dest => dest.InternalStatusId, opt => opt.MapFrom(src => src.Status));

            CreateMap<OnlinePaymentUpdateRequestDto, DataModels.OnlinePayment>()
                .ForMember(dest => dest.RequestorType, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<DataModels.Payment, OnlinePaymentResponseDto>()
                .ForMember(dest => dest.GovPayPaymentId, opt => opt.MapFrom(src => src.OnlinePayment.GovPayPaymentId))
                .ForMember(dest => dest.UpdatedByOrganisationId, opt => opt.MapFrom(src => src.OnlinePayment.UpdatedByOrgId))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.ReasonForPayment))
                .ForMember(dest => dest.RequestorType, opt => opt.MapFrom(src => src.OnlinePayment.RequestorType.Type));

            CreateMap<OfflinePaymentInsertRequestDto, DataModels.Payment>()
                .ForMember(dest => dest.ReasonForPayment, opt => opt.MapFrom(src => src.Description));

            CreateMap<OfflinePaymentInsertRequestDto, DataModels.OfflinePayment>()
                .ForMember(dest => dest.PaymentMethod, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<OfflinePaymentInsertRequestV2Dto, DataModels.Payment>()
                .ForMember(dest => dest.ReasonForPayment, opt => opt.MapFrom(src => src.Description));

            CreateMap<OfflinePaymentInsertRequestV2Dto, DataModels.OfflinePayment>()
                .ForMember(dest => dest.PaymentMethodId, opt => opt.MapFrom(src => src.PaymentMethod.HasValue ? (int)src.PaymentMethod.Value : DefaultDataConstants.NotApplicableIdValue))
                .ForMember(dest => dest.PaymentMethod, opt => opt.Ignore())
                .ReverseMap();
        }
    }
}
