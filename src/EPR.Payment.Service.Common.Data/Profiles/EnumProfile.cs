using AutoMapper;
using EPR.Payment.Service.Common.Data.Enums;
using DTO = EPR.Payment.Service.Common.Dtos.Enums;

namespace EPR.Payment.Service.Common.Data.Profiles
{
    public class EnumProfile : Profile
    {
        public EnumProfile()
        {
            CreateMap<Status, DTO.Status>().ReverseMap();
        }
    }
}
