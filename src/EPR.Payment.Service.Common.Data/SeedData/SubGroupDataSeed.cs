using System.Diagnostics.CodeAnalysis;
using EPR.Payment.Service.Common.Data.DataModels.Lookups;
using EPR.Payment.Service.Common.Extensions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPR.Payment.Service.Common.Data.SeedData
{
    [ExcludeFromCodeCoverage]
    public static class SubGroupDataSeed
    {
        public static void SeedSubGroupData(EntityTypeBuilder<SubGroup> builder)
        {
            builder.HasData(
                new SubGroup { Id = (int)Enums.SubGroup.Large, Type = Enums.SubGroup.Large.ToString(), Description = Enums.SubGroup.Large.GetDescription() },
                new SubGroup { Id = (int)Enums.SubGroup.Small, Type = Enums.SubGroup.Small.ToString(), Description = Enums.SubGroup.Small.GetDescription() },
                new SubGroup { Id = (int)Enums.SubGroup.Registration, Type = Enums.SubGroup.Registration.ToString(), Description = Enums.SubGroup.Registration.GetDescription() },
                new SubGroup { Id = (int)Enums.SubGroup.Online, Type = Enums.SubGroup.Online.ToString(), Description = Enums.SubGroup.Online.GetDescription() },
                new SubGroup { Id = (int)Enums.SubGroup.UpTo20, Type = Enums.SubGroup.UpTo20.ToString(), Description = Enums.SubGroup.UpTo20.GetDescription() },
                new SubGroup { Id = (int)Enums.SubGroup.MoreThan20, Type = Enums.SubGroup.MoreThan20.ToString(), Description = Enums.SubGroup.MoreThan20.GetDescription() },
                new SubGroup { Id = (int)Enums.SubGroup.ReSubmitting, Type = Enums.SubGroup.ReSubmitting.ToString(), Description = Enums.SubGroup.ReSubmitting.GetDescription() },
                new SubGroup { Id = (int)Enums.SubGroup.LateFee, Type = Enums.SubGroup.LateFee.ToString(), Description = Enums.SubGroup.LateFee.GetDescription() },
                new SubGroup { Id = (int)Enums.SubGroup.Aluminium, Type = Enums.SubGroup.Aluminium.ToString(), Description = Enums.SubGroup.Aluminium.GetDescription() },
                new SubGroup { Id = (int)Enums.SubGroup.Glass, Type = Enums.SubGroup.Glass.ToString(), Description = Enums.SubGroup.Glass.GetDescription() },
                new SubGroup { Id = (int)Enums.SubGroup.PaperOrBoardOrFibreBasedCompositeMaterial, Type = Enums.SubGroup.PaperOrBoardOrFibreBasedCompositeMaterial.ToString(), Description = Enums.SubGroup.PaperOrBoardOrFibreBasedCompositeMaterial.GetDescription() },
                new SubGroup { Id = (int)Enums.SubGroup.Plastic, Type = Enums.SubGroup.Plastic.ToString(), Description = Enums.SubGroup.Plastic.GetDescription() },
                new SubGroup { Id = (int)Enums.SubGroup.Steel, Type = Enums.SubGroup.Steel.ToString(), Description = Enums.SubGroup.Steel.GetDescription() },
                new SubGroup { Id = (int)Enums.SubGroup.Wood, Type = Enums.SubGroup.Wood.ToString(), Description = Enums.SubGroup.Wood.GetDescription() });
        }
    }
}