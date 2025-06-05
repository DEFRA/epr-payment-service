using System.Diagnostics.CodeAnalysis;
using EPR.Payment.Service.Common.Data.DataModels.Lookups;
using EPR.Payment.Service.Common.Extensions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceCommonEnums = EPR.Payment.Service.Common.Enums;

namespace EPR.Payment.Service.Common.Data.SeedData
{
    [ExcludeFromCodeCoverage]
    public static class TonnageBandDataSeed
    {
        public static void SeedTonnageBandData(EntityTypeBuilder<TonnageBand> builder)
        {
            builder.HasData(
               new TonnageBand { Id = (int)ServiceCommonEnums.TonnageBands.Upto500, Type = ServiceCommonEnums.TonnageBands.Upto500.ToString(), Description = ServiceCommonEnums.TonnageBands.Upto500.GetDescription() },
               new TonnageBand { Id = (int)ServiceCommonEnums.TonnageBands.Over500To5000, Type = ServiceCommonEnums.TonnageBands.Over500To5000.ToString(), Description = ServiceCommonEnums.TonnageBands.Over500To5000.GetDescription() },
               new TonnageBand { Id = (int)ServiceCommonEnums.TonnageBands.Over5000To10000, Type = ServiceCommonEnums.TonnageBands.Over5000To10000.ToString(), Description = ServiceCommonEnums.TonnageBands.Over5000To10000.GetDescription() },
               new TonnageBand { Id = (int)ServiceCommonEnums.TonnageBands.Over10000, Type = ServiceCommonEnums.TonnageBands.Over10000.ToString(), Description = ServiceCommonEnums.TonnageBands.Over10000.GetDescription() });
        }
    }
}