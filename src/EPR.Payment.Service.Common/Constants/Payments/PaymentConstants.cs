namespace EPR.Payment.Service.Common.Constants.Payments
{
    public static class PaymentConstants
    {
        public const string Status500InternalServerError = "Internal server error";
        public const string InvalidInputToInsertPaymentError = "The payment that is being tried to be recorded is invalid.";
        public const string InvalidInputToUpdatePaymentError = "The payment that is being tried to be updated is invalid.";
        public const string RecordNotFoundPaymentError = "Payment record not found for ID";
    }
}
