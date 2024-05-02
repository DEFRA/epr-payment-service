using AutoMapper;
using EPR.Payment.Service.Common.Data.DataModels.Lookups;
using EPR.Payment.Service.Common.Data.Extensions;
using EPR.Payment.Service.Common.Dtos.Responses;

namespace EPR.Payment.Service.Common.Data.Profiles
{
    public class FeesProfile : Profile
    {
        public FeesProfile() 
        {
            CreateMap<Fees, GetFeesResponse>()
                .MapOnlyNonDefault()
                .ReverseMap()
                .MapOnlyNonDefault();
        }
    }
}
