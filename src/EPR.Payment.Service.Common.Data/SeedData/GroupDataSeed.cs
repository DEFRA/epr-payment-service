using System.Diagnostics.CodeAnalysis;
using EPR.Payment.Service.Common.Data.DataModels.Lookups;
using EPR.Payment.Service.Common.Extensions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceCommonEnums = EPR.Payment.Service.Common.Enums;

namespace EPR.Payment.Service.Common.Data.SeedData
{
    [ExcludeFromCodeCoverage]
    public static class GroupDataSeed
    {
        public static void SeedGroupData(EntityTypeBuilder<Group> builder)
        {
            builder.HasData(
               new Group { Id = (int)ServiceCommonEnums.Group.ProducerType, Type = ServiceCommonEnums.Group.ProducerType.ToString(), Description = ServiceCommonEnums.Group.ProducerType.GetDescription() },
               new Group { Id = (int)ServiceCommonEnums.Group.ComplianceScheme, Type = ServiceCommonEnums.Group.ComplianceScheme.ToString(), Description = ServiceCommonEnums.Group.ComplianceScheme.GetDescription() },
               new Group { Id = (int)ServiceCommonEnums.Group.ProducerSubsidiaries, Type = ServiceCommonEnums.Group.ProducerSubsidiaries.ToString(), Description = ServiceCommonEnums.Group.ProducerSubsidiaries.GetDescription() },
               new Group { Id = (int)ServiceCommonEnums.Group.ComplianceSchemeSubsidiaries, Type = ServiceCommonEnums.Group.ComplianceSchemeSubsidiaries.ToString(), Description = ServiceCommonEnums.Group.ComplianceSchemeSubsidiaries.GetDescription() },
               new Group { Id = (int)ServiceCommonEnums.Group.ProducerResubmission, Type = ServiceCommonEnums.Group.ProducerResubmission.ToString(), Description = ServiceCommonEnums.Group.ProducerResubmission.GetDescription() },
               new Group { Id = (int)ServiceCommonEnums.Group.ComplianceSchemeResubmission, Type = ServiceCommonEnums.Group.ComplianceSchemeResubmission.ToString(), Description = ServiceCommonEnums.Group.ComplianceSchemeResubmission.GetDescription() },
               new Group { Id = (int)ServiceCommonEnums.Group.Exporters, Type = ServiceCommonEnums.Group.Exporters.ToString(), Description = ServiceCommonEnums.Group.Exporters.GetDescription() },
               new Group { Id = (int)ServiceCommonEnums.Group.Reprocessors, Type = ServiceCommonEnums.Group.Reprocessors.ToString(), Description = ServiceCommonEnums.Group.Reprocessors.GetDescription() });
        }
    }
}