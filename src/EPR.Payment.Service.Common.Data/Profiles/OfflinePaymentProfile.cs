using AutoMapper;
using EPR.Payment.Service.Common.Dtos.Request.Payments;
using EPR.Payment.Service.Common.Dtos.Response.Payments;

namespace EPR.Payment.Service.Common.Data.Profiles
{
    public class OfflinePaymentProfile : Profile
    {
        public OfflinePaymentProfile()
        {
            CreateMap<OfflinePaymentStatusInsertRequestDto, DataModels.OfflinePayment>().ForMember(dest => dest.ReasonForPayment, opt => opt.MapFrom(src => src.Description));
        }
    }
}
