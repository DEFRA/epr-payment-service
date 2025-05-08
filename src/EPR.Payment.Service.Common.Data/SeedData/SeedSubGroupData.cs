using EPR.Payment.Service.Common.Data.DataModels.Lookups;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace EPR.Payment.Service.Common.Data.SeedData
{
    [ExcludeFromCodeCoverage]
    public class SeedSubGroupData
    {
        public static void SeedDataSubGroup(ModelBuilder builder)
        {
            builder.Entity<SubGroup>().HasData(
                 new SubGroup { Id = 1, Type = "Large", Description = "Large producer" },
                new SubGroup { Id = 2, Type = "Small", Description = "Small producer" },
                new SubGroup { Id = 3, Type = "Registration", Description = "Registration" },
                new SubGroup { Id = 4, Type = "Online", Description = "Online Market" },
                new SubGroup { Id = 5, Type = "UpTo20", Description = "Up to 20" },
                new SubGroup { Id = 6, Type = "MoreThan20", Description = "More than 20" },
                new SubGroup { Id = 7, Type = "ReSubmitting", Description = "Re-submitting a report" },
                new SubGroup { Id = 8, Type = "LateFee", Description = "Late Fee" },
                new SubGroup { Id = 9, Type = "Aluminium", Description = "Aluminium" },
                new SubGroup { Id = 10, Type = "Glass", Description = "Glass" },
                new SubGroup { Id = 11, Type = "PaperOrBoardOrFibreBasedCompositeMaterial", Description = "Paper, board or fibre-based composite material" },
                new SubGroup { Id = 12, Type = "Plastic", Description = "Plastic" },
                new SubGroup { Id = 13, Type = "Steel", Description = "Steel" },
                new SubGroup { Id = 14, Type = "Wood", Description = "Wood" });
        }
    }
}