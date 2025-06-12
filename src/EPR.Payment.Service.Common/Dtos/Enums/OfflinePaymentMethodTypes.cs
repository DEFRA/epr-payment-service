using System.ComponentModel;

namespace EPR.Payment.Service.Common.Dtos.Enums
{
    public enum OfflinePaymentMethodTypes
    {
        [Description("Bank transfer")]
        BankTransfer = 2,

        [Description("Credit or debit card")]
        CreditOrDebitCard = 3,

        [Description("Cheque")]
        Cheque = 4,

        [Description("Cash")]
        Cash = 5
    }
}
