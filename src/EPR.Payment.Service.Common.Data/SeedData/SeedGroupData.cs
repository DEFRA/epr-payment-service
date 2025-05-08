using EPR.Payment.Service.Common.Data.DataModels.Lookups;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace EPR.Payment.Service.Common.Data.SeedData
{
    [ExcludeFromCodeCoverage]
    public class SeedGroupData
    {
        public static void SeedDataGroup(ModelBuilder builder)
        {
            builder.Entity<Group>().HasData(
               new Group { Id = 1, Type = "ProducerType", Description = "Producer Type" },
               new Group { Id = 2, Type = "ComplianceScheme", Description = "Compliance Scheme" },
               new Group { Id = 3, Type = "ProducerSubsidiaries", Description = "Producer Subsidiaries" },
               new Group { Id = 4, Type = "ComplianceSchemeSubsidiaries", Description = "Compliance Scheme Subsidiaries" },
               new Group { Id = 5, Type = "ProducerResubmission", Description = "Producer re-submitting a report" },
               new Group { Id = 6, Type = "ComplianceSchemeResubmission", Description = "Compliance Scheme re-submitting a report" },
               new Group { Id = 7, Type = "Exporters", Description = "Exporters" },
               new Group { Id = 8, Type = "Reprocessors", Description = "Reprocessors" });
        }
    }
}