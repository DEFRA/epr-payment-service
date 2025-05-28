using AutoMapper;
using EPR.Payment.Service.Common.Enums;
using System.Diagnostics.CodeAnalysis;
using DTO = EPR.Payment.Service.Common.Dtos.Enums;

namespace EPR.Payment.Service.Common.Data.Profiles
{
    [ExcludeFromCodeCoverage]
    public class EnumProfile : Profile
    {
        public EnumProfile()
        {
            CreateMap<Status, DTO.Status>().ReverseMap();
        }
    }
}
