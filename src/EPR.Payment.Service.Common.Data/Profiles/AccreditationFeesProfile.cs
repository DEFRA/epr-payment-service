using AutoMapper;
using EPR.Payment.Service.Common.Data.DataModels.Lookups;
using EPR.Payment.Service.Common.Data.Extensions;
using EPR.Payment.Service.Common.Dtos.Responses;
using System.Diagnostics.CodeAnalysis;

namespace EPR.Payment.Service.Common.Data.Profiles
{
    [ExcludeFromCodeCoverage]
    public class AccreditationFeesProfile : Profile
    {
        public AccreditationFeesProfile() 
        {
            CreateMap<AccreditationFees, GetAccreditationFeesResponse>()
                .MapOnlyNonDefault()
                .ReverseMap()
                .MapOnlyNonDefault();
        }
    }
}
