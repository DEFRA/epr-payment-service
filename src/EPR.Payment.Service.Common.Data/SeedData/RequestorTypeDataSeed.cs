using EPR.Payment.Service.Common.Data.Constants;
using EPR.Payment.Service.Common.Data.DataModels.Lookups;
using EPR.Payment.Service.Common.Enums;
using EPR.Payment.Service.Common.Extensions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace EPR.Payment.Service.Common.Data.SeedData
{
    [ExcludeFromCodeCoverage]
    public static class RequestorTypeDataSeed
    {
        public static void SeedRequestorTypeData(EntityTypeBuilder<RequestorType> builder)
        {
            builder.HasData(
               new RequestorType { Id = DefaultDataConstants.NotApplicableIdValue, Type = DefaultDataConstants.NotApplicableTypeValue, Description = DefaultDataConstants.NotApplicableDescriptionValue },
               new RequestorType { Id = (int)OnlinePaymentRequestorTypes.Producers, Type = OnlinePaymentRequestorTypes.Producers.ToString(), Description = OnlinePaymentRequestorTypes.Producers.GetDescription() },
               new RequestorType { Id = (int)OnlinePaymentRequestorTypes.ComplianceSchemes, Type = OnlinePaymentRequestorTypes.ComplianceSchemes.ToString(), Description = OnlinePaymentRequestorTypes.ComplianceSchemes.GetDescription() },
               new RequestorType { Id = (int)OnlinePaymentRequestorTypes.Exporters, Type = OnlinePaymentRequestorTypes.Exporters.ToString(), Description = OnlinePaymentRequestorTypes.Exporters.GetDescription() },
               new RequestorType { Id = (int)OnlinePaymentRequestorTypes.Reprocessors, Type = OnlinePaymentRequestorTypes.Reprocessors.ToString(), Description = OnlinePaymentRequestorTypes.Reprocessors.GetDescription() });
        }
    }
}