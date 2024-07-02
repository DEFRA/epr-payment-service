using AutoMapper;
using EPR.Payment.Service.Common.Data.DataModels.Lookups;
using EPR.Payment.Service.Common.Data.Extensions;
using EPR.Payment.Service.Common.Dtos.Responses;
using System.Diagnostics.CodeAnalysis;

namespace EPR.Payment.Service.Common.Data.Profiles
{
    [ExcludeFromCodeCoverage]
    public class FeesProfile : Profile
    {
        public FeesProfile() 
        {
            CreateMap<ProducerRegitrationFees, RegistrationFeeResponseDto>()
                .MapOnlyNonDefault()
                .ReverseMap()
                .MapOnlyNonDefault();
        }
    }
}
