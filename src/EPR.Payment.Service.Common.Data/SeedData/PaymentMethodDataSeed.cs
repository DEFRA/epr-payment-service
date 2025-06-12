using EPR.Payment.Service.Common.Data.Constants;
using EPR.Payment.Service.Common.Data.DataModels.Lookups;
using EPR.Payment.Service.Common.Dtos.Enums;
using EPR.Payment.Service.Common.Extensions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace EPR.Payment.Service.Common.Data.SeedData
{
    [ExcludeFromCodeCoverage]
    public static class PaymentMethodDataSeed
    {
        public static void SeedPaymentMethodData(EntityTypeBuilder<PaymentMethod> builder)
        {
            builder.HasData(
               new PaymentMethod { Id = DefaultDataConstants.NotApplicableIdValue, Type = DefaultDataConstants.NotApplicableTypeValue, Description = DefaultDataConstants.NotApplicableDescriptionValue },
               new PaymentMethod { Id = (int)OfflinePaymentMethodTypes.BankTransfer, Type = OfflinePaymentMethodTypes.BankTransfer.ToString(), Description = OfflinePaymentMethodTypes.BankTransfer.GetDescription() },
               new PaymentMethod { Id = (int)OfflinePaymentMethodTypes.CreditOrDebitCard, Type = OfflinePaymentMethodTypes.CreditOrDebitCard.ToString(), Description = OfflinePaymentMethodTypes.CreditOrDebitCard.GetDescription() },
               new PaymentMethod { Id = (int)OfflinePaymentMethodTypes.Cheque, Type = OfflinePaymentMethodTypes.Cheque.ToString(), Description = OfflinePaymentMethodTypes.Cheque.GetDescription() },
               new PaymentMethod { Id = (int)OfflinePaymentMethodTypes.Cash, Type = OfflinePaymentMethodTypes.Cash.ToString(), Description = OfflinePaymentMethodTypes.Cash.GetDescription() });
        }
    }
}