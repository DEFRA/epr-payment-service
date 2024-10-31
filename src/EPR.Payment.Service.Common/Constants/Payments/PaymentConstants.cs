namespace EPR.Payment.Service.Common.Constants.Payments
{
    public static class PaymentConstants
    {
        public const string ReceivingPaymentError = "An error occured while receiving Payment";
        public const string InsertingPaymentError = "An error occured while inserting Payment";
        public const string UpdatingPaymentError = "An error occured while updating Payment";
        public const string InvalidInputToInsertPaymentError = "The payment that is being tried to be recorded is invalid.";
        public const string InvalidInputToUpdatePaymentError = "The payment that is being tried to be updated is invalid.";
        public const string RecordNotFoundPaymentError = "Payment record not found for ID";
    }
}
