using System.Diagnostics.CodeAnalysis;
using EPR.Payment.Service.Common.Data.DataModels.Lookups;
using EPR.Payment.Service.Common.Extensions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPR.Payment.Service.Common.Data.SeedData
{
    [ExcludeFromCodeCoverage]
    public static class GroupDataSeed
    {
        public static void SeedGroupData(EntityTypeBuilder<Group> builder)
        {
            builder.HasData(
               new Group { Id = (int)Enums.Group.ProducerType, Type = Enums.Group.ProducerType.ToString(), Description = Enums.Group.ProducerType.GetDescription() },
               new Group { Id = (int)Enums.Group.ComplianceScheme, Type = Enums.Group.ComplianceScheme.ToString(), Description = Enums.Group.ComplianceScheme.GetDescription() },
               new Group { Id = (int)Enums.Group.ProducerSubsidiaries, Type = Enums.Group.ProducerSubsidiaries.ToString(), Description = Enums.Group.ProducerSubsidiaries.GetDescription() },
               new Group { Id = (int)Enums.Group.ComplianceSchemeSubsidiaries, Type = Enums.Group.ComplianceSchemeSubsidiaries.ToString(), Description = Enums.Group.ComplianceSchemeSubsidiaries.GetDescription() },
               new Group { Id = (int)Enums.Group.ProducerResubmission, Type = Enums.Group.ProducerResubmission.ToString(), Description = Enums.Group.ProducerResubmission.GetDescription() },
               new Group { Id = (int)Enums.Group.ComplianceSchemeResubmission, Type = Enums.Group.ComplianceSchemeResubmission.ToString(), Description = Enums.Group.ComplianceSchemeResubmission.GetDescription() },
               new Group { Id = (int)Enums.Group.Exporters, Type = Enums.Group.Exporters.ToString(), Description = Enums.Group.Exporters.GetDescription() },
               new Group { Id = (int)Enums.Group.Reprocessors, Type = Enums.Group.Reprocessors.ToString(), Description = Enums.Group.Reprocessors.GetDescription() });
        }
    }
}