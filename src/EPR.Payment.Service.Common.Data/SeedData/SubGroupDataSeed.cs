using System.Diagnostics.CodeAnalysis;
using EPR.Payment.Service.Common.Data.DataModels.Lookups;
using EPR.Payment.Service.Common.Extensions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceCommonEnums = EPR.Payment.Service.Common.Enums;

namespace EPR.Payment.Service.Common.Data.SeedData
{
    [ExcludeFromCodeCoverage]
    public static class SubGroupDataSeed
    {
        public static void SeedSubGroupData(EntityTypeBuilder<SubGroup> builder)
        {
            builder.HasData(
                new SubGroup { Id = (int)ServiceCommonEnums.SubGroup.Large, Type = ServiceCommonEnums.SubGroup.Large.ToString(), Description = ServiceCommonEnums.SubGroup.Large.GetDescription() },
                new SubGroup { Id = (int)ServiceCommonEnums.SubGroup.Small, Type = ServiceCommonEnums.SubGroup.Small.ToString(), Description = ServiceCommonEnums.SubGroup.Small.GetDescription() },
                new SubGroup { Id = (int)ServiceCommonEnums.SubGroup.Registration, Type = ServiceCommonEnums.SubGroup.Registration.ToString(), Description = ServiceCommonEnums.SubGroup.Registration.GetDescription() },
                new SubGroup { Id = (int)ServiceCommonEnums.SubGroup.Online, Type = ServiceCommonEnums.SubGroup.Online.ToString(), Description = ServiceCommonEnums.SubGroup.Online.GetDescription() },
                new SubGroup { Id = (int)ServiceCommonEnums.SubGroup.UpTo20, Type = ServiceCommonEnums.SubGroup.UpTo20.ToString(), Description = ServiceCommonEnums.SubGroup.UpTo20.GetDescription() },
                new SubGroup { Id = (int)ServiceCommonEnums.SubGroup.MoreThan20, Type = ServiceCommonEnums.SubGroup.MoreThan20.ToString(), Description = ServiceCommonEnums.SubGroup.MoreThan20.GetDescription() },
                new SubGroup { Id = (int)ServiceCommonEnums.SubGroup.ReSubmitting, Type = ServiceCommonEnums.SubGroup.ReSubmitting.ToString(), Description = ServiceCommonEnums.SubGroup.ReSubmitting.GetDescription() },
                new SubGroup { Id = (int)ServiceCommonEnums.SubGroup.LateFee, Type = ServiceCommonEnums.SubGroup.LateFee.ToString(), Description = ServiceCommonEnums.SubGroup.LateFee.GetDescription() },
                new SubGroup { Id = (int)ServiceCommonEnums.SubGroup.Aluminium, Type = ServiceCommonEnums.SubGroup.Aluminium.ToString(), Description = ServiceCommonEnums.SubGroup.Aluminium.GetDescription() },
                new SubGroup { Id = (int)ServiceCommonEnums.SubGroup.Glass, Type = ServiceCommonEnums.SubGroup.Glass.ToString(), Description = ServiceCommonEnums.SubGroup.Glass.GetDescription() },
                new SubGroup { Id = (int)ServiceCommonEnums.SubGroup.PaperOrBoardOrFibreBasedCompositeMaterial, Type = ServiceCommonEnums.SubGroup.PaperOrBoardOrFibreBasedCompositeMaterial.ToString(), Description = ServiceCommonEnums.SubGroup.PaperOrBoardOrFibreBasedCompositeMaterial.GetDescription() },
                new SubGroup { Id = (int)ServiceCommonEnums.SubGroup.Plastic, Type = ServiceCommonEnums.SubGroup.Plastic.ToString(), Description = ServiceCommonEnums.SubGroup.Plastic.GetDescription() },
                new SubGroup { Id = (int)ServiceCommonEnums.SubGroup.Steel, Type = ServiceCommonEnums.SubGroup.Steel.ToString(), Description = ServiceCommonEnums.SubGroup.Steel.GetDescription() },
                new SubGroup { Id = (int)ServiceCommonEnums.SubGroup.Wood, Type = ServiceCommonEnums.SubGroup.Wood.ToString(), Description = ServiceCommonEnums.SubGroup.Wood.GetDescription() });
        }
    }
}