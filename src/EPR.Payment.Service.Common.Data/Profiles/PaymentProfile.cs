using AutoMapper;
using EPR.Payment.Service.Common.Data.Extensions;
using EPR.Payment.Service.Common.Dtos.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPR.Payment.Service.Common.Data.Profiles
{
    public class PaymentProfile : Profile
    {
        public PaymentProfile() 
        {
            CreateMap<PaymentStatusInsertRequestDto, DataModels.Payment>().ForMember(dest => dest.InternalStatusId, opt => opt.MapFrom(src => src.Status));

            CreateMap<PaymentStatusUpdateRequestDto, DataModels.Payment>().ForMember(dest => dest.InternalStatusId, opt => opt.MapFrom(src => src.Status)).ForMember(dest => dest.InternalErrorCode, opt => opt.MapFrom(src => src.ErrorCode));

        }
    }
}
